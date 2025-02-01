using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Godot;

public class CommandHandler
{
    private static Dictionary<string, Command> _commands = new Dictionary<string, Command>();
    private static Dictionary<Type, object> _recordData = new Dictionary<Type, object>();
    private static List<object> _instances = new List<object>();

    public void Initialize()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(CommandAttribute), true).Any()).ToArray();

        foreach (Type t in types) {
            CommandAttribute commandAttribute = t.GetCustomAttribute(typeof(CommandAttribute), true) as CommandAttribute;
            Command instance = Activator.CreateInstance(t) as Command;

            instance._Initialize(commandAttribute.KeyWord, commandAttribute.Record);

            if (instance != null && commandAttribute != null)
            {
                Console.Log($"Type with CommandAttribute: {t.FullName}, KeyWord: {commandAttribute.KeyWord}");
                _commands.Add(commandAttribute.KeyWord, instance);
            }
        }

        foreach (object obj in _instances) {
            Type type = obj.GetType();

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                CommandVarLinkAttribute attribute = field.GetCustomAttribute<CommandVarLinkAttribute>();
                
                if (attribute == null) continue;

                FieldInfo propertyInfo = attribute.Record.GetField(attribute.LinkWith);

                if (propertyInfo == null) {
                    Console.Error($"The \"{attribute.LinkWith}\" field or property does not exist!");
                    continue;
                } else {
                    if (_recordData.ContainsKey(attribute.Record)) {
                        propertyInfo.SetValue(_recordData[attribute.Record], field.GetValue(obj));
                    } else {
                        object instance = Activator.CreateInstance(attribute.Record);
                        propertyInfo.SetValue(instance, field.GetValue(obj));

                        _recordData.Add(attribute.Record, instance);
                    
                        Console.Warning($"attribute.LinkWith: {attribute.LinkWith}, attribute.Record: {attribute.Record}");
                    }
                }
            }
        } 
    }

    public static void RegisterInstance(object instance) {
        if (_instances.Contains(instance)) {
            Console.Error($"RegisterInstance : The {instance.GetHashCode()} instance already contains!");
            return;
        }
        _instances.Add(instance);
    }

    public static void LinkInstance<T>(object instance, string linkWith) {
        Type type = typeof(T);
        FieldInfo fieldInfo = type.GetField(linkWith);

        if (fieldInfo == null) {
            Console.Error($"The \"{linkWith}\" field or property does not exist!");
            return;
        } else {
            if (_recordData.ContainsKey(type)) {
                fieldInfo.SetValue(_recordData[type], instance);
            } else {
                object obj = Activator.CreateInstance(type);
                fieldInfo.SetValue(obj, instance);

                _recordData.Add(type, obj);
            
                Console.Warning($"attribute.LinkWith: {linkWith}, attribute.Record: {type}");
            }
        }
    }

    public void InvokeCommand(string text)
    {
        if (string.IsNullOrEmpty(text.Trim())) return;

        string[] keys = text.Split(" ");
        Command command = GetCommand(keys[0]);

        Console.Debug($"-> {Console.SetColor(text, Colors.WebGray)}");
        
        if (command != null) {
            command._Execute(keys, _recordData[command.Record]);
        } else {
            Console.Debug(Console.SetColor($"the \"{Console.SetColor(keys[0], Colors.Aqua)}\" command does not exist!", Colors.WebGray));
        }
    }

    public Command GetCommand(string key)
    {
        if (_commands.ContainsKey(key)) return _commands[key];
        else return null;
    }
}
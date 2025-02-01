using System;
using System.Collections.Generic;

public partial class Command
{
    private string _commandName;
    private Type _record;
    private Dictionary<string, Action<string[], object>> _branch = new Dictionary<string, Action<string[], object>>();

    public string CommandName { get => _commandName; }
    public Type Record { get => _record; }

    public virtual void _Initialize(string name, Type record) {
        _commandName = name;
        _record = record;

        AddFunction("help", _OnHelp);
    }

    public virtual void _Execute(string[] keys, object record) { 
        if (keys.Length <= 1) return;

        _branch[keys[1]].Invoke(keys, record);
    }

    protected virtual void _OnHelp(string[] keys, object record) {

    }

    protected void AddFunction(string key, Action<string[], object> action) {
        if (_branch.ContainsKey(key)) {
            Console.Error($"{this.GetType()} : The \"{key}\" command already exits!");
        } else {
            _branch.Add(key, action);
        }
    }
}
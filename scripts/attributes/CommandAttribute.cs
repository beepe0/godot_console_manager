using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
    public string KeyWord { get; set; }
    public Type Record { get; set; }

    public CommandAttribute(string keyWord, Type record)
    {
        KeyWord = keyWord;
        Record = record;
    }
}
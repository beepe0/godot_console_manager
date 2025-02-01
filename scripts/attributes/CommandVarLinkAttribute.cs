using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class CommandVarLinkAttribute : Attribute
{
    public Type Record { get; set; }
    public string LinkWith { get; set; }

    public CommandVarLinkAttribute(Type recordType, string linkWith) {
        Record = recordType;
        LinkWith = linkWith;    
    }
}
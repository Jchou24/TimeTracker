using System;

namespace TimeTracker.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueAttribute : Attribute
    {
        public string DefaultValue { get; set; }
    }
}

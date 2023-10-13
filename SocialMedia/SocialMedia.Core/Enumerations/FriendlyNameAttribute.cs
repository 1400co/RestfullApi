using System;

namespace SocialMedia.Core.Enumerations
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class FriendlyNameAttribute : Attribute
    {
        public string FriendlyName { get; }

        public FriendlyNameAttribute(string name)
        {
            this.FriendlyName = name;
        }
    }
}

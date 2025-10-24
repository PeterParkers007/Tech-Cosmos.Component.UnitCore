using System;

namespace UnitCore.Runtime.Abilities
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AbilityAttribute : Attribute
    {
        public string AbilityId { get; }

        public AbilityAttribute(string abilityId)
        {
            AbilityId = abilityId;
        }
    }
}
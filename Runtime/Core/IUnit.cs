using System;
using TechCosmos.UnitCore.Events;
using TechCosmos.UnitCore.Properties;
using TechCosmos.UnitCore.Abilities;
using UnityEngine;
namespace TechCosmos.UnitCore.Core
{
    public interface IUnit
    {
        string UnitId { get; }
        UnitTeam Team { get; }
        bool IsAlive { get; }
        Vector2 Position { get; }
        UnitConfig Config { get; }
        UnitEventSystem EventSystem { get; }

        event Action<IUnit, float> OnHealthChanged;
        event Action<IUnit> OnDeath;

        void Initialize(UnitConfig config);
        void Dispose();

        // 实用方法
        void ApplyDamage(float damage);
        void ApplyHeal(float healAmount);
        T GetAbility<T>() where T : class, IUnitAbility;
    }
}

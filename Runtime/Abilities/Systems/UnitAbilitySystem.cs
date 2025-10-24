using System;
using System.Collections.Generic;
using System.Linq;
using UnitCore.Runtime.Core;
namespace UnitCore.Runtime.Abilities.Systems
{
    public class UnitAbilitySystem
    {
        private readonly IUnit _unit;
        private readonly Dictionary<string, IUnitAbility> _abilities = new();

        public UnitAbilitySystem(IUnit unit, UnitConfig config)
        {
            _unit = unit;

            // 根据配置创建能力
            foreach (var abilityId in config.abilityIds)
            {
                var ability = AbilityFactory.CreateAbility(abilityId);
                if (ability != null)
                    _abilities[abilityId] = ability;
            }
        }

        public void InitializeAbilities()
        {
            foreach (var ability in _abilities.Values)
            {
                ability.Initialize(_unit);
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var ability in _abilities.Values)
            {
                ability.Update(deltaTime);
            }
        }

        public T GetAbility<T>() where T : class, IUnitAbility
        {
            return _abilities.Values.FirstOrDefault(ability => ability is T) as T;
        }

        public void StopAllAbilities()
        {
            foreach (var ability in _abilities.Values)
            {
                if (ability is IMoveAbility moveAbility)
                    moveAbility.Stop();

                if (ability is IAttackAbility attackAbility)
                    attackAbility.StopAttack();
            }
        }

        public void Dispose()
        {
            foreach (var ability in _abilities.Values)
                ability.Dispose();

            _abilities.Clear();
        }
    }
}

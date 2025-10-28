using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechCosmos.UnitCore.Abilities;
using TechCosmos.UnitCore.Core;
namespace TechCosmos.UnitCore.Abilities.Implementations
{
    [Ability("Attack")]
    public class AttackAbility : IAttackAbility
    {
        public string AbilityId => "Attack";
        public bool IsAttacking { get; private set; }

        private IUnit _unit;
        private UnitConfig _config;
        private MonoBehaviour _coroutineRunner;
        private Coroutine _attackCoroutine;
        private IUnit _currentTarget;

        // 用户注册的攻击逻辑回调
        public System.Action<IUnit, IUnit> OnPerformAttack { get; set; }

        public void Initialize(IUnit unit)
        {
            _unit = unit;
            _config = unit.Config;
            _coroutineRunner = unit as MonoBehaviour;
        }

        public void Attack(IUnit target)
        {
            StopAttack();

            if (target == null || !target.IsAlive) return;

            _currentTarget = target;
            IsAttacking = true;
            _attackCoroutine = _coroutineRunner.StartCoroutine(AttackCoroutine(target));
        }

        public void StopAttack()
        {
            if (_attackCoroutine != null && _coroutineRunner != null)
            {
                _coroutineRunner.StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            IsAttacking = false;
            _currentTarget = null;
        }

        private IEnumerator AttackCoroutine(IUnit target)
        {
            while (IsAttacking && target != null && target.IsAlive)
            {
                if (Vector2.Distance(_unit.Position, target.Position) <= _config.attackRange)
                {
                    // 直接调用用户注册的攻击逻辑
                    OnPerformAttack?.Invoke(_unit, target);
                    yield return new WaitForSeconds(_config.attackCooldown);
                }
                else
                {
                    yield return null;
                }
            }

            StopAttack();
        }

        public void Update(float deltaTime) { }

        public void Dispose()
        {
            StopAttack();
            OnPerformAttack = null; // 清理回调
        }
    }
}

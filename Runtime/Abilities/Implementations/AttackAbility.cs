using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitCore.Runtime.Abilities;
using UnitCore.Runtime.Core;
namespace UnitCore.Runtime.Abilities.Implementations
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
                // 检查是否在攻击范围内
                if (Vector2.Distance(_unit.Position, target.Position) <= _config.attackRange)
                {
                    // 执行攻击
                    PerformAttack(target);
                    yield return new WaitForSeconds(_config.attackCooldown);
                }
                else
                {
                    // 不在范围内，等待一帧
                    yield return null;
                }
            }

            StopAttack();
        }

        private void PerformAttack(IUnit target)
        {
            Debug.Log($"{_unit.UnitId} 攻击 {target.UnitId}");
            // 这里可以触发攻击效果、播放动画等
        }

        public void Update(float deltaTime) { }

        public void Dispose()
        {
            StopAttack();
        }
    }
}


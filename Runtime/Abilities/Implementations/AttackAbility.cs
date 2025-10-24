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
                // ����Ƿ��ڹ�����Χ��
                if (Vector2.Distance(_unit.Position, target.Position) <= _config.attackRange)
                {
                    // ִ�й���
                    PerformAttack(target);
                    yield return new WaitForSeconds(_config.attackCooldown);
                }
                else
                {
                    // ���ڷ�Χ�ڣ��ȴ�һ֡
                    yield return null;
                }
            }

            StopAttack();
        }

        private void PerformAttack(IUnit target)
        {
            Debug.Log($"{_unit.UnitId} ���� {target.UnitId}");
            // ������Դ�������Ч�������Ŷ�����
        }

        public void Update(float deltaTime) { }

        public void Dispose()
        {
            StopAttack();
        }
    }
}


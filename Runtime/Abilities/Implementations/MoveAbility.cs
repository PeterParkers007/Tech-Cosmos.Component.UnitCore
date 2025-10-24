using UnityEngine;
using System.Collections;
using UnitCore.Runtime.Core;
namespace UnitCore.Runtime.Abilities.Implementations
{
    [Ability("Move")]
    public class MoveAbility : IMoveAbility
    {
        public string AbilityId => "Move";
        public bool IsMoving { get; private set; }

        private IUnit _unit;
        private Rigidbody2D _rb;
        private UnitConfig _config;
        private MonoBehaviour _coroutineRunner;
        private Coroutine _moveCoroutine;

        public void Initialize(IUnit unit)
        {
            _unit = unit;
            _rb = (unit as MonoBehaviour)?.GetComponent<Rigidbody2D>();
            _config = unit.Config;
            _coroutineRunner = unit as MonoBehaviour;
        }

        public void MoveTo(Vector2 position)
        {
            Stop(); // 停止之前的移动

            if (_rb == null || _config == null || _coroutineRunner == null) return;

            IsMoving = true;
            _moveCoroutine = _coroutineRunner.StartCoroutine(MoveCoroutine(position));
        }

        public void Stop()
        {
            if (_moveCoroutine != null && _coroutineRunner != null)
            {
                _coroutineRunner.StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            if (_rb != null)
                _rb.velocity = Vector2.zero;

            IsMoving = false;
        }

        private IEnumerator MoveCoroutine(Vector2 target)
        {
            while (Vector2.Distance(_unit.Position, target) > _config.stoppingDistance)
            {
                Vector2 direction = (target - _unit.Position).normalized;
                Vector2 newPosition = _unit.Position + direction * _config.movementSpeed * Time.deltaTime;

                _rb.MovePosition(newPosition);
                yield return null;
            }

            IsMoving = false;
            _moveCoroutine = null;
        }

        public void Update(float deltaTime)
        {
            // 可以在这里处理移动相关的逻辑
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

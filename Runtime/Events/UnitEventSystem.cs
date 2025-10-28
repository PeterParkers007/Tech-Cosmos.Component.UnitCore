using System;
using System.Collections.Generic;
using TechCosmos.UnitCore.Core;
namespace TechCosmos.UnitCore.Events
{
    /// <summary>
    /// ͨ�õ�λ�¼�ϵͳ��֧��ǿ�����¼��ַ�
    /// </summary>
    public class UnitEventSystem
    {
        private readonly Dictionary<Type, Delegate> _eventHandlers = new();
        private readonly IUnit _owner;

        public UnitEventSystem(IUnit owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        public void Subscribe<T>(Action<T> handler) where T : UnitEvent
        {
            var eventType = typeof(T);
            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = handler;
            }
            else
            {
                _eventHandlers[eventType] = Delegate.Combine(_eventHandlers[eventType], handler);
            }
        }

        /// <summary>
        /// ȡ�������¼�
        /// </summary>
        public void Unsubscribe<T>(Action<T> handler) where T : UnitEvent
        {
            var eventType = typeof(T);
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = Delegate.Remove(_eventHandlers[eventType], handler);

                if (_eventHandlers[eventType] == null)
                {
                    _eventHandlers.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        public void Publish<T>(T eventData) where T : UnitEvent
        {
            var eventType = typeof(T);
            if (_eventHandlers.ContainsKey(eventType))
            {
                var handler = _eventHandlers[eventType] as Action<T>;
                handler?.Invoke(eventData);
            }
        }

        /// <summary>
        /// ��������¼�����
        /// </summary>
        public void Clear()
        {
            _eventHandlers.Clear();
        }
    }

    /// <summary>
    /// ��λ�¼�����
    /// </summary>
    public abstract class UnitEvent
    {
        public IUnit Source { get; protected set; }
        public float Timestamp { get; protected set; }

        protected UnitEvent(IUnit source)
        {
            Source = source;
            Timestamp = UnityEngine.Time.time;
        }
    }

    // ========== �����¼����� ==========

    /// <summary>
    /// ��λ�����¼�
    /// </summary>
    public class UnitCreatedEvent : UnitEvent
    {
        public UnitCreatedEvent(IUnit source) : base(source) { }
    }

    /// <summary>
    /// ��λ�����¼�
    /// </summary>
    public class UnitDestroyedEvent : UnitEvent
    {
        public UnitDestroyedEvent(IUnit source) : base(source) { }
    }

    /// <summary>
    /// ����ֵ�仯�¼�
    /// </summary>
    public class HealthChangedEvent : UnitEvent
    {
        public float CurrentHealth { get; }
        public float PreviousHealth { get; }
        public float Delta { get; }

        public HealthChangedEvent(IUnit source, float current, float previous) : base(source)
        {
            CurrentHealth = current;
            PreviousHealth = previous;
            Delta = current - previous;
        }
    }

    /// <summary>
    /// �ܵ��˺��¼�
    /// </summary>
    public class DamageReceivedEvent : UnitEvent
    {
        public float Damage { get; }
        public IUnit DamageSource { get; }

        public DamageReceivedEvent(IUnit source, float damage, IUnit damageSource = null) : base(source)
        {
            Damage = damage;
            DamageSource = damageSource;
        }
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    public class HealReceivedEvent : UnitEvent
    {
        public float HealAmount { get; }
        public IUnit HealSource { get; }

        public HealReceivedEvent(IUnit source, float healAmount, IUnit healSource = null) : base(source)
        {
            HealAmount = healAmount;
            HealSource = healSource;
        }
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    public class UnitDeathEvent : UnitEvent
    {
        public IUnit Killer { get; }

        public UnitDeathEvent(IUnit source, IUnit killer = null) : base(source)
        {
            Killer = killer;
        }
    }

    /// <summary>
    /// �ƶ���ʼ�¼�
    /// </summary>
    public class MoveStartedEvent : UnitEvent
    {
        public UnityEngine.Vector2 TargetPosition { get; }

        public MoveStartedEvent(IUnit source, UnityEngine.Vector2 targetPosition) : base(source)
        {
            TargetPosition = targetPosition;
        }
    }

    /// <summary>
    /// �ƶ�����¼�
    /// </summary>
    public class MoveCompletedEvent : UnitEvent
    {
        public UnityEngine.Vector2 FinalPosition { get; }

        public MoveCompletedEvent(IUnit source, UnityEngine.Vector2 finalPosition) : base(source)
        {
            FinalPosition = finalPosition;
        }
    }

    /// <summary>
    /// ������ʼ�¼�
    /// </summary>
    public class AttackStartedEvent : UnitEvent
    {
        public IUnit Target { get; }

        public AttackStartedEvent(IUnit source, IUnit target) : base(source)
        {
            Target = target;
        }
    }

    /// <summary>
    /// ���������¼�
    /// </summary>
    public class AttackLandedEvent : UnitEvent
    {
        public IUnit Target { get; }
        public float Damage { get; }

        public AttackLandedEvent(IUnit source, IUnit target, float damage) : base(source)
        {
            Target = target;
            Damage = damage;
        }
    }

    /// <summary>
    /// ״̬�仯�¼�
    /// </summary>
    public class StateChangedEvent : UnitEvent
    {
        public string PreviousState { get; }
        public string NewState { get; }

        public StateChangedEvent(IUnit source, string previousState, string newState) : base(source)
        {
            PreviousState = previousState;
            NewState = newState;
        }
    }
}
using System;
using System.Collections.Generic;
using TechCosmos.UnitCore.Core;
namespace TechCosmos.UnitCore.Events
{
    /// <summary>
    /// 通用单位事件系统，支持强类型事件分发
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
        /// 订阅事件
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
        /// 取消订阅事件
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
        /// 触发事件
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
        /// 清空所有事件订阅
        /// </summary>
        public void Clear()
        {
            _eventHandlers.Clear();
        }
    }

    /// <summary>
    /// 单位事件基类
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

    // ========== 具体事件类型 ==========

    /// <summary>
    /// 单位创建事件
    /// </summary>
    public class UnitCreatedEvent : UnitEvent
    {
        public UnitCreatedEvent(IUnit source) : base(source) { }
    }

    /// <summary>
    /// 单位销毁事件
    /// </summary>
    public class UnitDestroyedEvent : UnitEvent
    {
        public UnitDestroyedEvent(IUnit source) : base(source) { }
    }

    /// <summary>
    /// 生命值变化事件
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
    /// 受到伤害事件
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
    /// 治疗事件
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
    /// 死亡事件
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
    /// 移动开始事件
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
    /// 移动完成事件
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
    /// 攻击开始事件
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
    /// 攻击命中事件
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
    /// 状态变化事件
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
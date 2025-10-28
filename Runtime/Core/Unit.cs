using System;
using UnityEngine;
using TechCosmos.UnitCore.Abilities;
using TechCosmos.UnitCore.Abilities.Systems;
using TechCosmos.UnitCore.Properties;
using TechCosmos.UnitCore.Events;
namespace TechCosmos.UnitCore.Core
{
    public class Unit : MonoBehaviour, IUnit
    {
        [SerializeField] private UnitConfig _config;

        // 组件引用
        private Rigidbody2D _rb;

        // 核心系统
        private UnitPropertySystem _propertySystem;
        private UnitAbilitySystem _abilitySystem;
        private UnitEventSystem _eventSystem;  // 添加事件系统字段

        // 接口实现
        public string UnitId => _config?.unitId ?? "Unknown";
        public UnitTeam Team => _config?.team ?? UnitTeam.Neutral;
        public bool IsAlive => _propertySystem?.CurrentHealth > 0;
        public Vector2 Position => transform.position;
        public UnitConfig Config => _config;
        public UnitEventSystem EventSystem => _eventSystem;  // 实现接口属性

        public event Action<IUnit, float> OnHealthChanged;
        public event Action<IUnit> OnDeath;

        private void Awake()
        {
            InitializeComponents();
        }

        public void Initialize(UnitConfig config)
        {
            _config = config;

            // 初始化各系统
            _propertySystem = new UnitPropertySystem(config);
            _abilitySystem = new UnitAbilitySystem(this, config);
            _eventSystem = new UnitEventSystem(this);  // 初始化事件系统

            // 注册事件
            _propertySystem.OnHealthChanged += HandleHealthChanged;
            _propertySystem.OnDeath += HandleDeath;

            // 初始化能力
            _abilitySystem.InitializeAbilities();

            // 发布单位创建事件
            _eventSystem.Publish(new UnitCreatedEvent(this));
        }

        public void UnitUpdate()
        {
            if (!IsAlive) return;
            _abilitySystem?.Update(Time.deltaTime);
        }

        public void Dispose()
        {
            _abilitySystem?.Dispose();
            _propertySystem?.Dispose();
            _eventSystem?.Clear();  // 清理事件系统

            OnHealthChanged = null;
            OnDeath = null;
        }

        private void InitializeComponents()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void HandleHealthChanged(float newHealth, float delta)
        {
            OnHealthChanged?.Invoke(this, newHealth);

            // 发布详细事件
            _eventSystem.Publish(new HealthChangedEvent(this, newHealth, newHealth - delta));

            if (delta < 0) // 受到伤害
            {
                _eventSystem.Publish(new DamageReceivedEvent(this, -delta));
            }
            else if (delta > 0) // 受到治疗
            {
                _eventSystem.Publish(new HealReceivedEvent(this, delta));
            }
        }

        private void HandleDeath()
        {
            OnDeath?.Invoke(this);
            _abilitySystem?.StopAllAbilities();
            _eventSystem.Publish(new UnitDeathEvent(this));  // 发布死亡事件
        }

        private void OnDestroy()
        {
            Dispose();
        }

        // 公共方法供外部调用
        public void ApplyDamage(float damage)
        {
            _propertySystem?.ApplyDamage(damage);
        }

        public void ApplyHeal(float healAmount)
        {
            _propertySystem?.ApplyHeal(healAmount);
        }

        public T GetAbility<T>() where T : class, IUnitAbility
        {
            return _abilitySystem?.GetAbility<T>();
        }
    }
}

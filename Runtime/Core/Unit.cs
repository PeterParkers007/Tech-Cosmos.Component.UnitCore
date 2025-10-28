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

        // �������
        private Rigidbody2D _rb;

        // ����ϵͳ
        private UnitPropertySystem _propertySystem;
        private UnitAbilitySystem _abilitySystem;
        private UnitEventSystem _eventSystem;  // ����¼�ϵͳ�ֶ�

        // �ӿ�ʵ��
        public string UnitId => _config?.unitId ?? "Unknown";
        public UnitTeam Team => _config?.team ?? UnitTeam.Neutral;
        public bool IsAlive => _propertySystem?.CurrentHealth > 0;
        public Vector2 Position => transform.position;
        public UnitConfig Config => _config;
        public UnitEventSystem EventSystem => _eventSystem;  // ʵ�ֽӿ�����

        public event Action<IUnit, float> OnHealthChanged;
        public event Action<IUnit> OnDeath;

        private void Awake()
        {
            InitializeComponents();
        }

        public void Initialize(UnitConfig config)
        {
            _config = config;

            // ��ʼ����ϵͳ
            _propertySystem = new UnitPropertySystem(config);
            _abilitySystem = new UnitAbilitySystem(this, config);
            _eventSystem = new UnitEventSystem(this);  // ��ʼ���¼�ϵͳ

            // ע���¼�
            _propertySystem.OnHealthChanged += HandleHealthChanged;
            _propertySystem.OnDeath += HandleDeath;

            // ��ʼ������
            _abilitySystem.InitializeAbilities();

            // ������λ�����¼�
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
            _eventSystem?.Clear();  // �����¼�ϵͳ

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

            // ������ϸ�¼�
            _eventSystem.Publish(new HealthChangedEvent(this, newHealth, newHealth - delta));

            if (delta < 0) // �ܵ��˺�
            {
                _eventSystem.Publish(new DamageReceivedEvent(this, -delta));
            }
            else if (delta > 0) // �ܵ�����
            {
                _eventSystem.Publish(new HealReceivedEvent(this, delta));
            }
        }

        private void HandleDeath()
        {
            OnDeath?.Invoke(this);
            _abilitySystem?.StopAllAbilities();
            _eventSystem.Publish(new UnitDeathEvent(this));  // ���������¼�
        }

        private void OnDestroy()
        {
            Dispose();
        }

        // �����������ⲿ����
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

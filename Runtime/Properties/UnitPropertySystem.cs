using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitCore.Runtime.Core;
using UnityEngine;
namespace UnitCore.Runtime.Properties
{
    public class UnitPropertySystem
    {
        private readonly UnitConfig _config;
        private float _currentHealth;

        public float CurrentHealth => _currentHealth;
        public event Action<float, float> OnHealthChanged; // newHealth, delta
        public event Action OnDeath;

        public UnitPropertySystem(UnitConfig config)
        {
            _config = config;
            _currentHealth = config.maxHealth;
        }

        public void ApplyDamage(float damage)
        {
            if (_currentHealth <= 0) return;

            var previousHealth = _currentHealth;
            _currentHealth = Mathf.Max(0, _currentHealth - damage);

            OnHealthChanged?.Invoke(_currentHealth, _currentHealth - previousHealth);

            if (_currentHealth <= 0)
                OnDeath?.Invoke();
        }

        public void ApplyHeal(float healAmount)
        {
            if (_currentHealth <= 0) return;

            var previousHealth = _currentHealth;
            _currentHealth = Mathf.Min(_config.maxHealth, _currentHealth + healAmount);

            OnHealthChanged?.Invoke(_currentHealth, _currentHealth - previousHealth);
        }

        public void Dispose()
        {
            OnHealthChanged = null;
            OnDeath = null;
        }
    }
}
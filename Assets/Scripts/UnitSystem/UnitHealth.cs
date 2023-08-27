using System;
using UnityEngine;

namespace UnitSystem
{
    public class UnitHealth : MonoBehaviour
    {
        public event Action OnDead;

        [SerializeField]
        private int maxHealth = 100;

        private int _health;

        private void Awake()
        {
            _health = maxHealth;
        }

        private void Die()
        {
            OnDead?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
            }

            Debug.Log($"Unit {gameObject.name} took {damage} damage, remaining health: {_health}");

            if (_health == 0)
            {
                Die();
            }
        }
    }
}

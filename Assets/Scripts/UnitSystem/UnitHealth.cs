using System;
using UnityEngine;
using UnityEngine.Analytics;

namespace UnitSystem
{
    public class UnitHealth : MonoBehaviour
    {
        public event Action OnDead;
        public event Action OnHealthChanged;

        [SerializeField]
        private int health = 100;

        public int MaxHealth { get; private set; }
        public int Health
        {
            get => health;
        }

        private void Awake()
        {
            MaxHealth = health;
        }

        private void Die()
        {
            OnDead?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            var actualDamage = damage;
            if (actualDamage > health)
            {
                actualDamage = health;
            }
            health -= actualDamage;

            Debug.Log($"Unit {gameObject.name} took {actualDamage} damage, remaining health: {health}");
            OnHealthChanged?.Invoke();

            if (health == 0)
            {
                Die();
            }
        }
    }
}

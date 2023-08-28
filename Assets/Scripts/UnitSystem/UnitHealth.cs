using System;
using UnityEngine;

namespace UnitSystem
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField]
        private int health = 100;

        public int MaxHealth { get; private set; }

        public int Health => health;

        private void Awake() { MaxHealth = health; }

        public event Action OnDead;
        public event Action OnHealthChanged;

        private void Die() { OnDead?.Invoke(); }

        public void TakeDamage(int damage)
        {
            var actual = damage;
            if (actual > health)
            {
                actual = health;
            }

            health -= actual;

            Debug.Log($"Unit {gameObject.name} took {actual} damage, remaining health: {health}");
            OnHealthChanged?.Invoke();

            if (health == 0)
            {
                Die();
            }
        }
    }
}

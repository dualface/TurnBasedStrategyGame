using UnityEngine;
using TMPro;
using UnitSystem;
using UnityEngine.UI;

namespace UI
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text actionPointsUI;

        [SerializeField]
        private Image healthBar;

        [SerializeField]
        private Unit unit;

        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            unit.OnActionPointsChanged += UpdateActionPoints;
            unit.Health.OnHealthChanged += UpdateHealth;

            UpdateActionPoints();
            UpdateHealth();
        }

        private void Update()
        {
            transform.LookAt(_cameraTransform);
        }

        private void UpdateActionPoints()
        {
            actionPointsUI.text = unit.ActionPoints.ToString();
        }

        private void UpdateHealth()
        {
            healthBar.fillAmount = (float)unit.Health.Health / (float)unit.Health.MaxHealth;
        }
    }
}

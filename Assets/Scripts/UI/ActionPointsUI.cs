using TMPro;
using UnitSystem;
using UnityEngine;

namespace UI
{
    public class ActionPointsUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text pointsText;

        public void UpdateActionPoints(Unit unit)
        {
            if (!unit)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            pointsText.text = $"ActionPoints: {unit.ActionPoints}";
        }

        public void HideActionPoints() { gameObject.SetActive(false); }
    }
}

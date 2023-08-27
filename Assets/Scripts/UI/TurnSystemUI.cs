using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text turnText;

        [SerializeField]
        private Button endTurnButton;

        [SerializeField]
        private GameObject enemyTurnUI;

        private void Start()
        {
            var instance = TurnSystem.Instance;
            instance.OnStartNewTurn += UpdateTurnUI;
            UpdateTurnUI(instance.IsPlayerTurn, instance.Round);
            endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
        }

        private void OnEndTurnButtonClicked()
        {
            TurnSystem.Instance.NextTurn();
        }

        private void UpdateTurnUI(bool isPlayerTurn, int round)
        {
            var turnName = isPlayerTurn ? "player" : "enemy";
            turnText.text = $"Turn {round}: {turnName}";

            enemyTurnUI.SetActive(!isPlayerTurn);
            endTurnButton.gameObject.SetActive(isPlayerTurn);
        }
    }
}

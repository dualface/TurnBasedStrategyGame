using UnityEngine;

namespace UnitSystem
{
    public class EnemyAI : MonoBehaviour
    {
        private float _timer;

        private void Start() { TurnSystem.Instance.OnStartNewTurn += OnStartNewTurn; }

        private void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn)
            {
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= 2f)
            {
                TurnSystem.Instance.NextTurn();
            }
        }

        private void OnStartNewTurn(TurnSystem.StartNewTurnArgs args)
        {
            if (!args.IsPlayerTurn)
            {
                _timer = 0;
            }
        }
    }
}

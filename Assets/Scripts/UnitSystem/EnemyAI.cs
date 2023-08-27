using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer;

    private void Start()
    {
        TurnSystem.Instance.OnStartNewTurn += OnStartNewTurn;
    }

    private void OnStartNewTurn(bool isPlayerTurn, int round)
    {
        if (!isPlayerTurn)
        {
            _timer = 0;
        }
    }

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
}

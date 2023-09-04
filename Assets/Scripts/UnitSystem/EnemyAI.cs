using System;
using UnityEngine;

namespace UnitSystem
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForEnemyTurn,
            TakingTurn,
            Busy,
        }

        private float _timer;
        private State _state = State.WaitingForEnemyTurn;

        private void Start() { TurnSystem.Instance.OnStartNewTurn += OnStartNewTurn; }

        private void Update()
        {
            switch (_state)
            {
            case State.TakingTurn:
                _timer += Time.deltaTime;
                if (_timer >= 0.5f)
                {
                    if (!TryTakeEnemyAction())
                    {
                        AllEnemiesTakeActionComplete();
                    }
                }

                break;

            case State.Busy:
                break;

            case State.WaitingForEnemyTurn:
            default:
                return;
            }
        }

        private void OnStartNewTurn(TurnSystem.StartNewTurnArgs args)
        {
            if (args.IsPlayerTurn)
            {
                return;
            }

            _timer = 0;
            _state = State.TakingTurn;
        }

        private bool TryTakeEnemyAction()
        {
            foreach (var enemy in UnitActionSystem.Instance.EnemyUnits)
            {
                var action = enemy.defaultAction;
                if (!enemy.TrySpendActionPointToTakeAction(action))
                {
                    continue;
                }

                action.TakeAction(enemy.GridPosition, EnemyTakeActionComplete);
                _state = State.Busy;
                return true;
            }

            return false;
        }

        private void EnemyTakeActionComplete()
        {
            _timer = 0;
            _state = State.TakingTurn;
        }

        private void AllEnemiesTakeActionComplete()
        {
            _state = State.WaitingForEnemyTurn;
            TurnSystem.Instance.NextTurn();
        }
    }
}

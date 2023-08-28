using System;
using UnityEngine;

namespace UnitSystem
{
    public class TurnSystem : MonoBehaviour
    {
        public Action<StartNewTurnArgs> OnStartNewTurn;

        public static TurnSystem Instance { get; private set; }

        public int Round { get; private set; } = 1;

        public bool IsPlayerTurn { get; private set; } = true;

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogError("TurnSystem instance already exist");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void NextTurn()
        {
            IsPlayerTurn = !IsPlayerTurn;
            if (IsPlayerTurn)
            {
                Round++;
            }

            OnStartNewTurn?.Invoke(new StartNewTurnArgs(IsPlayerTurn, Round));
        }

        public class StartNewTurnArgs
        {
            public StartNewTurnArgs(bool isPlayerTurn, int round)
            {
                IsPlayerTurn = isPlayerTurn;
                Round = round;
            }

            public bool IsPlayerTurn { get; }
            public int Round { get; }
        }
    }
}

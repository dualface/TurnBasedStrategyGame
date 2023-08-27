using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public Action<bool, int> OnStartNewTurn;

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

        OnStartNewTurn?.Invoke(IsPlayerTurn, Round);
        var turnName = IsPlayerTurn ? "player" : "enemy";
        Debug.Log($"Start new {turnName} turn, round {Round}");
    }
}

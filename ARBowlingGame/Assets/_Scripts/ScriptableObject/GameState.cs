using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/CreateGameStateAsset")]
public class GameState : ScriptableObject
{
    public enum GameStateEnum
    {
        TitleScreen,
        PlacingPinDeckAndLane,
        SetupBalls,
        ReadyToThrow,
        BallInPlay,
        BallPlayEnd,
        StrikeAchieved,
        TurnEnd,
        ResettingDeck,
        GameEnded
    }

    [HideInInspector] public UnityEvent<int> OnScoreChanged;
    [HideInInspector] public UnityEvent OnEnterBallSetup;
    [HideInInspector] public UnityEvent OnReadyToThrow;
    [HideInInspector] public UnityEvent OnBallInPlay;
    [HideInInspector] public UnityEvent OnBallPlayEnd;
    [HideInInspector] public UnityEvent OnStrikeAchieved;
    [HideInInspector] public UnityEvent OnResettingDeck;
    [HideInInspector] public UnityEvent OnGameEnded;
    [HideInInspector] public UnityEvent OnTurnEnded;

    [SerializeField] private GameStateEnum _currentGameState;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _remainingBalls = 0;
    [SerializeField] private int _currentTurn = 0;
    [SerializeField] private int _maxTurns = 5;
    [SerializeField] private int _strikeCounter = 0;
    [SerializeField] private int _strikeExtraPoints = 10;
    [SerializeField] private float _throwPowerMultiplier = 0.05f;

    #region Properties
    public GameStateEnum CurrentGameState
    {
        get => _currentGameState;
        set
        {
            _currentGameState = value;

            switch (value)
            {
                case GameStateEnum.SetupBalls:
                    OnEnterBallSetup?.Invoke();
                    break;
                case GameStateEnum.ReadyToThrow:
                    OnReadyToThrow?.Invoke();
                    break;
                case GameStateEnum.BallInPlay:
                    OnBallInPlay?.Invoke();
                    break;
                case GameStateEnum.BallPlayEnd:
                    OnBallPlayEnd?.Invoke();
                    break;
                case GameStateEnum.StrikeAchieved:
                    OnStrikeAchieved?.Invoke();
                    break;
                case GameStateEnum.ResettingDeck:
                    OnResettingDeck?.Invoke();
                    break;
                case GameStateEnum.GameEnded:
                    OnGameEnded?.Invoke();
                    break;
                case GameStateEnum.TitleScreen:
                    break;
                case GameStateEnum.PlacingPinDeckAndLane:
                    break;
                case GameStateEnum.TurnEnd:
                    OnTurnEnded?.Invoke();
                    break;
            }
        }


    }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke(_score);
        }
    }
    public int RemainingBalls
    {
        get => _remainingBalls;
        set => _remainingBalls = value;
    }
    public int CurrentTurn
    {
        get => _currentTurn;
        set => _currentTurn = value;
    }
    public int StrikeCounter
    {
        get => _strikeCounter;
        set => _strikeCounter = value;
    }
    public int MaxTurns
    {
        get => _maxTurns;
        set => _maxTurns = value;
    }
    public int StrikeExtraPoints
    {
        get => _strikeExtraPoints;
        set => _strikeExtraPoints = value;
    }
    public float ThrowPowerMultiplier
    {
        get => _throwPowerMultiplier;
        set => _throwPowerMultiplier = value;
    }

    #endregion

    public void ResetState()
    {
        _currentTurn = 1;
        _score = 0;
        _remainingBalls = MaxTurns;
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

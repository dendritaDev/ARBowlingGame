using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private Transform _arCamera;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameState _gameState;

    private GameObject _currentBall;
    private Vector2 _touchInitialPosition;
    private Vector2 _touchFinalPosition;
    private float _ySwipeDelta; //difference between initial and final touch positions
    private float throwPowerMultiplier = 0.05f;
    private Quaternion lookRotation;

    private void Awake()
    {
        

        // Resets game state to the needed values for a new game
        _gameState.ResetState();

        _gameState.CurrentGameState = GameState.GameStateEnum.PlacingPinDeckAndLane;


    }

    private void OnEnable()
    {
        
        // listen to Enter Ball Setup Event
        _gameState.OnEnterBallSetup.AddListener(EnableSelf);
    }

    private void OnDisable()
    {
        _gameState.OnEnterBallSetup.RemoveListener(EnableSelf);
    }

    void Update()
    {
        switch (_gameState.CurrentGameState)
        {
            case GameState.GameStateEnum.ReadyToThrow:
                // track touch to throw, device only
                DetectScreenSwipe();

#if UNITY_EDITOR
                // desktop editor only, track mouse button to throw
                if (Input.GetMouseButtonDown(1)) ThrowBall();
#endif
            break;

            case GameState.GameStateEnum.BallInPlay:
                // if the ball falls below -20, you have ended the play
                if (_currentBall.transform.position.y < -20)
                {
                    Debug.Log("PLAYER PLAY END!");

                    // reset ball
                    _currentBall.transform.position = new Vector3(0, 1000, 0);
                    _currentBall.transform.rotation = Quaternion.identity;

                    // reset rigidbody force
                    Rigidbody rb = _currentBall.GetComponent<Rigidbody>();
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.useGravity = false;

                    // ball has been thrown, set ball play end state
                    _gameState.CurrentGameState = GameState.GameStateEnum.BallPlayEnd;
                }
            break;
        }
    }

    void EnableSelf()
    {
        enabled = true;
        BallInitialSetup();
    }

    void BallInitialSetup()
    {
        // instantiate ball
        _currentBall = Instantiate(_ballPrefab, new Vector3(0, 1000, 0), Quaternion.identity);

        // switch to ReadyToThrow state
        _gameState.CurrentGameState = GameState.GameStateEnum.ReadyToThrow;
    }

    // Detects screen swipe and calls ThrowBall
    void DetectScreenSwipe()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _touchInitialPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _touchFinalPosition = touch.position;

                if (_touchFinalPosition.y > _touchInitialPosition.y)
                {
                    _ySwipeDelta = _touchFinalPosition.y - _touchInitialPosition.y;
                }

                ThrowBall();
            }
        }
    }

    void ThrowBall()
    {
        
        _currentBall.GetComponent<Rigidbody>().useGravity = true;

        float throwPowerMultiplier = 0.05f;
        Quaternion lookRotation = _arCamera.rotation;

#if UNITY_EDITOR
        
        Camera cam = _arCamera.GetComponent<Camera>();
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseDir = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane));

        lookRotation = Quaternion.LookRotation(mouseDir, Vector3.up);

        _ySwipeDelta = 1.5f;
        throwPowerMultiplier = 60.00f;
#endif

        _currentBall.transform.position = _arCamera.position;
        _currentBall.transform.rotation = lookRotation;

        Vector3 forceVector = _currentBall.transform.forward * (_ySwipeDelta * throwPowerMultiplier);
        _currentBall.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);

        // update balls remaining
        _gameState.RemainingBalls--;

        // set ball in play state
        _gameState.CurrentGameState = GameState.GameStateEnum.BallInPlay;

    }
}

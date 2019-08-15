using System;
using UnityEngine;

public class GameManager : MonoBehaviour{
    [SerializeField] private int _scoreToWin;
    [SerializeField] private int _currScore;

    [SerializeField] public bool _gamePaused;

    public static GameManager _instance;

    private void Awake(){
        _instance = this;
    }

    private void Start(){
        Time.timeScale = 1.0f;
    }

    private void Update(){
        InputListener();
    }

    private void InputListener(){
        if(Input.GetButtonDown("Cancel"))
            TogglePauseGame();
    }

    public void TogglePauseGame(){
        _gamePaused = !_gamePaused;
        Time.timeScale = _gamePaused ? 0.0f : 1.0f;

        Cursor.lockState = _gamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        
        GameUIController._instance.TogglePauseMenu(_gamePaused);
    }

    public void AddScore(int score){
        _currScore += score;
        GameUIController._instance.UpdateScoreText(_currScore);

        if (_currScore >= _scoreToWin)
            HandleWin();
    }

    private void HandleWin(){
        GameUIController._instance.SetEndGameScreen(true, _currScore);
        HandleEnd();

    }

    public void HandleLoss(){
        GameUIController._instance.SetEndGameScreen(false, _currScore);
       HandleEnd();
    }

    public void HandleEnd(){
        Time.timeScale = 0.0f;
        _gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

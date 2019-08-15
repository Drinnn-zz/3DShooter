using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour{
    [Header("HUD")] 
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _healthBarFill;
    
    [Header("Pause Menu")] 
    [SerializeField] private GameObject _pauseMenu;
    
    [Header("End Game Screen")] 
    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private TextMeshProUGUI _endGameHeaderText;
    [SerializeField] private TextMeshProUGUI _endGameScoreText;

    public static GameUIController _instance;

    private void Awake(){
        _instance = this;
    }

    public void UpdateHealthBar(int currHp, int maxHp){
        _healthBarFill.fillAmount = (float) currHp / (float) maxHp;
    }

    public void UpdateScoreText(int score){
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmoText(int currAmmo, int maxAmmo){
        _ammoText.text = "Ammo: " + currAmmo + " / " + maxAmmo;
    }

    public void TogglePauseMenu(bool paused){
        _pauseMenu.SetActive(paused);
    }

    public void SetEndGameScreen(bool won, int finalScore){
        _endGameScreen.SetActive(true);
        _endGameHeaderText.text = won ? "You Win!" : "You Lose!";
        _endGameHeaderText.color = won ? Color.green : Color.red;
        _endGameScoreText.text = "<b>Score</b>\n" + finalScore;
    }

    public void OnResumeButton(){
        GameManager._instance.TogglePauseGame();
    }

    public void OnRestartButton(){
        SceneManager.LoadScene("Game");
    }

    public void OnMenuButton(){
        SceneManager.LoadScene("Menu");
    }
}

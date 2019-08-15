using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnStartButton(){
        SceneManager.LoadScene("Game");
    }

    public void OnQuitButton(){
        Application.Quit();
    }
}

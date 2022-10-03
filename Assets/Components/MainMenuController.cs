using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    void OnEnable() {
        Time.timeScale = 0;
    }

    void OnDisable() {
        Time.timeScale = 1;
    }
    
    public void OnStart() {
        gameObject.SetActive(false);
    }
    
    public void OnTutorial() {
        // TODO
    }
    
    public void OnExit() {
        Application.Quit();
    }
    
    public void OnMainMenu() {
        SceneManager.LoadScene("Scenes/Main");
    }
}

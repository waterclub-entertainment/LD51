using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    public AudioMixer mixer;
    public GameObject pauseMenu;
    public GameObject pauseMenuButton;
    public Sprite iconMaximize;
    public Sprite iconMinimize;
    public Sprite iconMute;
    public Sprite iconUnmute;
    public float volume;
    
    private bool fullscreen = false;

    public void OnToggleMute(Image image) {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        if (volume >= -40f) {
            mixer.SetFloat("MasterVolume", -80f);
            image.sprite = iconUnmute;
        } else {
            mixer.SetFloat("MasterVolume", 0f);
            image.sprite = iconMute;
        }
    }

    public void OnChangeVolume(float Volume)
    {
        mixer.SetFloat("MasterVolume", Volume);
    }
    
    public void OnToggleFullscreen(Image image) {
        fullscreen = !fullscreen;
        Screen.fullScreen = fullscreen;
        if (fullscreen) {
            image.sprite = iconMinimize;
        } else {
            image.sprite = iconMaximize;
        }
    }
    
    public void OnTogglePauseMenu() {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }
    
    void Update() {
        if (Time.timeScale == 0 && !pauseMenu.activeInHierarchy) {
            // Main menu is open
            pauseMenuButton.SetActive(false);       
        } else {
            pauseMenuButton.SetActive(true);
            if (Input.GetButtonDown("Cancel")) {
                OnTogglePauseMenu();
            }
        }
    }

}

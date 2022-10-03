using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    public AudioMixer mixer;
    public GameObject pauseMenu;
    public GameObject pauseMenuButton;
    public Sprite iconMaximize;
    public Sprite iconMinimize;
    public Sprite iconMasterMute;
    public Sprite iconMasterUnmute;
    public Sprite iconMusicMute;
    public Sprite iconMusicUnmute;
    public Slider MasterSlider;
    public Slider MusicSlider;
    
    private bool fullscreen = false;

    public void OnToggleMuteMaster(Image image) {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        if (volume >= -79f) {
            mixer.SetFloat("MasterVolume", -80f);
            image.sprite = iconMasterMute;
        } else {
            mixer.SetFloat("MasterVolume", MasterSlider.value);
            image.sprite = iconMasterUnmute;
        }
    }

   
    public void OnChangeVolumeMaster(Image image)
    {
        image.sprite = iconMasterUnmute;
        mixer.SetFloat("MasterVolume", MasterSlider.value);
        Debug.Log(MasterSlider.value);
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

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    public AudioMixer mixer;
    public Sprite iconMaximize;
    public Sprite iconMinimize;
    public Sprite iconMute;
    public Sprite iconUnmute;
    
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
    
    public void OnToggleFullscreen(Image image) {
        fullscreen = !fullscreen;
        Screen.fullScreen = fullscreen;
        if (fullscreen) {
            image.sprite = iconMinimize;
        } else {
            image.sprite = iconMaximize;
        }
    }

}

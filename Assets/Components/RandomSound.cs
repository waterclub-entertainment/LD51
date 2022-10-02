using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSound : MonoBehaviour {

    public AudioClip[] audioClips;
    
    public void PlayRandomSound() {
        GetComponent<AudioSource>().PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }

}

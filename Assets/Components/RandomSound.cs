using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class RandomSound : MonoBehaviour {

    [Serializable]
    public class HarmonicEvent {
        public float timestamp;
        public AudioClip[] harmonics;
    }

    public AudioSource music;
    public HarmonicEvent[] harmonicEvents;
    
    public void PlayRandomSound() {
        float playbackTime = music.time;
        int index = 0;
        while (index + 1 < harmonicEvents.Length && harmonicEvents[index + 1].timestamp < playbackTime) {
            index++;
        }
        AudioClip sound = harmonicEvents[index].harmonics[UnityEngine.Random.Range(0, harmonicEvents[index].harmonics.Length)];
        GetComponent<AudioSource>().PlayOneShot(sound);
    }

}

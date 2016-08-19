using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class OneShotAudioPlayer : MonoBehaviour {

    [SerializeField]
    private AudioClip[] clips;

    [SerializeField]
    [Range(0f, 1f)]
    private float minVolume = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    private float maxVolume = 1f;

    [SerializeField]
    [Range(0.1f, 3f)]
    private float minPitch = 1f;

    [SerializeField]
    [Range(0.1f, 3f)]
    private float maxPitch = 1f;

    private AudioSource source;
    private bool hasPlayed = false;

	void Awake () 
    {
        source = GetComponent<AudioSource>();

        if (source.playOnAwake)
            source.playOnAwake = false;

        source.clip = FindClipToPlay();
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
        hasPlayed = true;

	}

    AudioClip FindClipToPlay()
    {
        int clipIndex = Random.Range(0, clips.Length);
        AudioClip clip = clips[clipIndex];
        return clip;
    }
	
	void Update () 
    {
        if (hasPlayed && !source.isPlaying)
            Destroy(gameObject);
	}
}

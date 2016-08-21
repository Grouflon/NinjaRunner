using UnityEngine;
using System.Collections;

public class LoopAudioPlayer : MonoBehaviour {

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    [Range (0f, 20f)]
    private float fadeInTime;

    [SerializeField]
    [Range (0f, 20f)]
    private float fadeOutTime;

    [SerializeField]
    [Range (0.1f, 3f)]
    private float minPitch = 1f;

    [SerializeField]
    [Range (0.1f, 3f)]
    private float maxPitch = 1f;

    [SerializeField]
    private bool randomStartPoint = false;

    private float targetVolume;
    private float volumeMultiplier;
    private float volumeMultiplierBeforeFadeOut;
    private bool fadingIn = false;
    private bool fadingOut = false;

    void Awake()
    {
        if (source == null)
            source = gameObject.GetComponent<AudioSource>();
        
        source.playOnAwake = false;
        source.loop = true;

        targetVolume = source.volume;
        source.pitch = Random.Range(minPitch, maxPitch);
        source.volume = 0f;
    }

	void Start () 
    {   
        if (randomStartPoint)
            source.timeSamples = (Random.Range(0, source.clip.samples));
        
        source.Play();
        fadingIn = true;
	}

    public void Stop()
    {
        volumeMultiplierBeforeFadeOut = volumeMultiplier;
        fadingOut = true;
        fadingIn = false;

    }
	
	// Update is called once per frame
	void Update () 
    {
        if (volumeMultiplier < 0f)
            volumeMultiplier = 0f;
        else if (volumeMultiplier > 1f)
            volumeMultiplier = 1f;

        if (fadingIn && volumeMultiplier == 1f)
            fadingIn = false;

        if (fadingIn)
            volumeMultiplier += Time.deltaTime * (1/fadeInTime);


        if (fadingOut)
            volumeMultiplier -= Time.deltaTime * (1/fadeOutTime) * volumeMultiplierBeforeFadeOut;

        source.volume = targetVolume * volumeMultiplier;

        if (fadingOut && source.volume == 0f)
            Destroy(this.gameObject);
	}
}

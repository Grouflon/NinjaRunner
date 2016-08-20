using UnityEngine;
using System.Collections;

public class KillSoundOver : MonoBehaviour {

    private AudioSource source;

    void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

	void Update () 
    {
        if (!source.isPlaying)
            Destroy(this.gameObject);
	}
}

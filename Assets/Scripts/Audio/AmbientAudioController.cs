using UnityEngine;
using System.Collections;

public class AmbientAudioController : MonoBehaviour {

    public static AmbientAudioController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }
}

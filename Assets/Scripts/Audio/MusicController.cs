using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour 
{
    [SerializeField]
    private GameObject musicLayers;
    private AudioSource source;
    private int syncPointSample = 2822400;

    public static MusicController instance;

    // Singleton
    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        InstantiateMusic();
    }

    void FixedUpdate()
    {
        if (source.timeSamples >= syncPointSample)
            InstantiateMusic();
    }

    void InstantiateMusic()
    {
        GameObject go = (GameObject)Instantiate(musicLayers, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
        source = go.GetComponent<AudioSource>();
    }


}

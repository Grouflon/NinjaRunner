using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicController : MonoBehaviour 
{
    [SerializeField]
    private GameObject musicLayers;
    private AudioSource source;
    private int syncPointSample = 2822400;

    [SerializeField]
    private AudioMixerSnapshot[] snapshots;
    [SerializeField]
    [Range(0, 10)]
    private float musicFadeTime;

    [SerializeField]
    [Range(0, 30)]
    private float lowMidCross;
    [SerializeField]
    [Range(0, 30)]
    private float midHighCross;


    [HideInInspector]
    public AudioMixerSnapshot currentSnapshot;

    [HideInInspector]
    public GameController game;

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

    void Update()
    {
        if (game.gameSpeed < lowMidCross && currentSnapshot != snapshots[0])
        {
            snapshots[0].TransitionTo(musicFadeTime);
            currentSnapshot = snapshots[0];
        }
            

        else if (game.gameSpeed >= lowMidCross && game.gameSpeed < midHighCross && currentSnapshot != snapshots[1])
        {
            snapshots[1].TransitionTo(musicFadeTime);
            currentSnapshot = snapshots[1];
        }

        else if (game.gameSpeed >= midHighCross && currentSnapshot != snapshots[2])
        {
            snapshots[2].TransitionTo(musicFadeTime);
            currentSnapshot = snapshots[2];
        }
    }

    void InstantiateMusic()
    {
        GameObject go = (GameObject)Instantiate(musicLayers, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
        source = go.GetComponent<AudioSource>();
    }
}

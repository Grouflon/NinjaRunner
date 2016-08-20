using UnityEngine;
using System.Collections;

public class GranularAudioPlayer : MonoBehaviour {

    [SerializeField] 
    private GameObject soundObject;

    [SerializeField]
    [Range(0,30)]
    private float minInitialDelay = 0f;
    [SerializeField]
    [Range(0,30)]
    private float maxInitialDelay = 0f;
    [SerializeField]
    [Range(0,30)]
    private float minSpawnTime = 0f;
    [SerializeField]
    [Range(0,30)]
    private float maxSpawnTime = 0f;
    [SerializeField]
    [Range(0,1)]
    private float maxRandomRadius = 0f;

    private AudioSource source;
    private bool hasPlayedOnce = false;


    void Awake()
    {
        source = soundObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(PlaySound());
    }

    Vector3 GetSpawnPosition()
    {
        float maxRandomDistance = source.maxDistance * maxRandomRadius;

        float randomXOffset = Random.Range(0f, maxRandomDistance);
        float randomYOffset = Random.Range(0f, maxRandomDistance);
        float randomZOffset = Random.Range(0f, maxRandomDistance);

        float randomX = Random.Range(Camera.main.transform.position.x - randomXOffset, Camera.main.transform.position.x + randomXOffset);
        float randomY = Random.Range(Camera.main.transform.position.y - randomYOffset, Camera.main.transform.position.y + randomYOffset);
        float randomZ = Random.Range(Camera.main.transform.position.z - randomZOffset, Camera.main.transform.position.z + randomZOffset);

        Vector3 position = new Vector3(randomX, randomY, randomZ);
        return position;
    }

    IEnumerator PlaySound()
    {
        if (!hasPlayedOnce)
        {
            yield return new WaitForSeconds(Random.Range(minInitialDelay, maxInitialDelay));
            hasPlayedOnce = true;
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }

        GameObject go = (GameObject)Instantiate(soundObject, GetSpawnPosition(), Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
        StartCoroutine(PlaySound());
    }

}

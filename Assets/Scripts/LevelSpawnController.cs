using UnityEngine;
using System.Collections;

public class LevelSpawnController : MonoBehaviour
{
    public float spawnInterval;
    public GameController game;
    public GameObject blockPrefab;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        while (m_distancePassed > spawnInterval)
        {
            GameObject block = (GameObject)Instantiate(blockPrefab, new Vector3(transform.position.x, blockPrefab.transform.position.y, 0.0f), Quaternion.identity);
            block.GetComponent<BlockController>().game = game;
            m_distancePassed -= spawnInterval;
        }

        m_distancePassed += game.gameSpeed * Time.deltaTime;
	}

    private float m_distancePassed = 0.0f;
}

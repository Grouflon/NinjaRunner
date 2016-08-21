using UnityEngine;
using System.Collections;

public class SoundTester : MonoBehaviour 
{
    [SerializeField]
    private GameObject soundPrefab;
    private LoopAudioPlayer loopPlayer;

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject go = (GameObject)Instantiate(soundPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            go.transform.parent = this.transform;
            loopPlayer = go.GetComponent<LoopAudioPlayer>();
        }
            

        if (Input.GetKeyDown(KeyCode.Z))
            loopPlayer.Stop();
	}
}

using UnityEngine;
using System.Collections;

public class SoundTester : MonoBehaviour 
{
    [SerializeField]
    private GameObject soundPrefab;

	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A))
            Instantiate(soundPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
}

using UnityEngine;
using System.Collections;

public class PlayerSfx : MonoBehaviour {

    [SerializeField]
    private GameObject footstepGo;

    [SerializeField]
    private GameObject rollGo;

    [SerializeField]
    private GameObject voiceGo;

    public void PlayFootsteps() 
    {
        GameObject go = (GameObject)Instantiate(footstepGo, transform.position, Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
	}

    public void PlayRollSfx()
    {
        GameObject go = (GameObject)Instantiate(rollGo, transform.position, Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
    }

    public void PlayVoiceSfx()
    {
        GameObject go = (GameObject)Instantiate(voiceGo, transform.position, Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
    }

}

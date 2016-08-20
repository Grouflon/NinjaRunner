using UnityEngine;
using System.Collections;

public class FootstepsController : MonoBehaviour {

    [SerializeField]
    private GameObject footstepGo;

    public void PlayFootsteps() 
    {
        GameObject go = (GameObject)Instantiate(footstepGo, transform.position, Quaternion.identity);
        go.transform.parent = this.gameObject.transform;
	}

}

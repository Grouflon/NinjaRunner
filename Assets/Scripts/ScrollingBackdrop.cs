using UnityEngine;
using System.Collections;

public class ScrollingBackdrop : MonoBehaviour
{
    public GameController game;
    public CameraController camera;
    public float xScrollingRatio = 1.0f;
    public float maxTextureOffset = 0.08f;

    void Start ()
	{
        m_material = GetComponent<MeshRenderer>().material;
	}
	
	void Update ()
	{
        Vector2 offset = m_material.mainTextureOffset;
        offset.x += game.gameSpeed * Time.deltaTime * xScrollingRatio;
        while(offset.x > 2.0f)
        {
            offset.x -= 1.0f;
        }


        offset.y = camera.GetCurrentHeightRatio() * maxTextureOffset;

        Debug.Log(offset.y);


        m_material.mainTextureOffset = offset;
	}

    Material m_material;
}

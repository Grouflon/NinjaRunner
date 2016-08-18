using UnityEngine;
using System.Collections;

public class ScrollingBackdrop : MonoBehaviour
{
    public GameController game;
    public float xScrollingRatio = 1.0f;
    public float yScrollingRatio = 0.0f;

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
        m_material.mainTextureOffset = offset;
	}

    Material m_material;
}

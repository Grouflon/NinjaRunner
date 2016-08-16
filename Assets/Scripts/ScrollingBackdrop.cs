using UnityEngine;
using System.Collections;

public class ScrollingBackdrop : MonoBehaviour
{
    public GameController game;
    public float scrollingRatio = 1.0f;

	void Start ()
	{
        m_material = GetComponent<MeshRenderer>().material;
	}
	
	void Update ()
	{
        Vector2 offset = m_material.mainTextureOffset;
        offset.x += game.gameSpeed * Time.deltaTime * scrollingRatio;
        while(offset.x > 2.0f)
        {
            offset.x -= 1.0f;
        }
        m_material.mainTextureOffset = offset;
	}

    Material m_material;
}

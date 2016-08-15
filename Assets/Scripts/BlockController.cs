using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public GameController game;

    void Start ()
	{
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
        m_rigidbody.velocity = new Vector2(-game.gameSpeed, 0.0f);

        if (transform.position.x < -25.0f)
        {
            Destroy(gameObject);
        }
	}

    Rigidbody2D m_rigidbody;
}

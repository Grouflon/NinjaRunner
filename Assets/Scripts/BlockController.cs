using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public GameController game;
    public float width;

    void Start ()
	{
        m_viewportStart = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x;
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
    void FixedUpdate()
    {
        m_rigidbody.velocity = new Vector2(-game.GetGameSpeed(), 0.0f);
    }

    void Update ()
	{
        if (transform.position.x + width < m_viewportStart)
        {
            Destroy(gameObject);
        }
	}

    Rigidbody2D m_rigidbody;
    float m_viewportStart;
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpStrength = 100.0f;
    public InputController input;
    public GameController game;
    public TouchGroundController groundChecker;

	void Start ()
	{
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
	    if (input.GetJumpInput() && groundChecker.IsTouchingGround() && m_rigidbody.velocity.y < 0.1f)
        {
            m_rigidbody.AddForce(new Vector2(0.0f, jumpStrength), ForceMode2D.Impulse);
        }

        if (transform.position.y < -10.0f)
        {
            game.SendMessage("OnPlayerDied");
        }
	}

    Rigidbody2D m_rigidbody;
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpStrength = 100.0f;
    public float cruiseSpeed = 20.0f;
    public InputController input;
    public GameController game;
    public GameObject graphic;
    public TouchGroundController groundChecker;

	void Start ()
	{
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = graphic.GetComponent<Animator>();
	}
	
	void Update ()
	{
        m_animator.SetBool("IsTouchingGround", groundChecker.IsTouchingGround());
        m_animator.SetFloat("SpeedRatio", game.gameSpeed / cruiseSpeed);

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
    Animator m_animator;
}

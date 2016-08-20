using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpStrength = 100.0f;
    public float cruiseSpeed = 20.0f;
    public InputController input;
    public GameController game;
    public GameObject graphic;
    public float stepDownDistance = 0.5f;
    public float raycastUpperDistance = 0.5f;

    public GameObject jumpSoundPrefab;

    public bool IsTouchingGround()
    {
        return m_touchingGround;
    }

	void Start ()
	{
        m_animator = graphic.GetComponent<Animator>();
	}

    void UpdatePosition(float _dt)
    {
        Vector3 position = transform.position;

        if (!m_touchingGround)
        {
            m_verticalVelocity += game.gravity * _dt;
            position.y += m_verticalVelocity * _dt;
        }

        // TODO: if the point collision is too hardcore, maybe spherecast and some projection on slopes since we know their angle
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(position.x, position.y + raycastUpperDistance), Vector2.down, 100.0f, LayerMask.GetMask("Ground"));
        if (hit.collider == null)
        {
            m_touchingGround = false;
            m_sliding = false;
        }
        else
        {
            if (m_touchingGround)
            {
                if (hit.collider.gameObject.tag != "downhill")
                {
                    m_sliding = false;
                }

                if (hit.point.y >= position.y - stepDownDistance)
                {
                    position.y = hit.point.y;
                    m_verticalVelocity = 0.0f;
                }
                else
                {
                    m_touchingGround = false;
                    m_sliding = false;
                }
            }
            else
            {
                if (hit.point.y > position.y)
                {
                    m_touchingGround = true;
                    position.y = hit.point.y;
                    m_verticalVelocity = 0.0f;

                    if (hit.collider.gameObject.tag == "downhill")
                    {
                        m_sliding = true;
                    }
                }
            }
        }

        transform.position = position;
    }
	
	void Update ()
	{
        UpdatePosition(Time.deltaTime);

        m_animator.SetBool("IsTouchingGround", IsTouchingGround());
        m_animator.SetBool("IsSliding", m_sliding);
        m_animator.SetFloat("SpeedRatio", Mathf.Min(game.gameSpeed / cruiseSpeed, 1.0f));

        if (input.GetJumpInput() && IsTouchingGround())
        {
            Instantiate(jumpSoundPrefab);

            m_touchingGround = false;
            m_verticalVelocity = jumpStrength;
        }

        if (transform.position.y < -10.0f)
        {
            game.SendMessage("OnPlayerDied");
        }
	}

    float m_verticalVelocity = 0.0f;
    Animator m_animator;
    bool m_touchingGround = false;
    bool m_sliding = false;
}

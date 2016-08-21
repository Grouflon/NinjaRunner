using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpStrength = 100.0f;
    public float cruiseSpeed = 20.0f;
    public InputController input;
    public GameController game;
    public LevelSpawnController level;
    public GameObject graphic;
    public float stepDownDistance = 0.5f;
    public float outStepDownTime = 0.02f;
    public float raycastUpperDistance = 0.5f;
    public float jumpControlTime = 1.0f;

    public GameObject jumpSoundPrefab;

    public bool IsTouchingGround()
    {
        return m_touchingGround;
    }

	void Start ()
	{
        m_animator = graphic.GetComponent<Animator>();

        Vector3 position = transform.position;
        position.y = level.startHeight * level.unitSize;
        transform.position = position;
	}

    void UpdatePosition(float _dt)
    {
        Vector3 position = transform.position;

        float gravityDt = _dt;
        if (m_isPressingJump && !IsTouchingGround() && m_jumpControlTimer <= jumpControlTime)
        {
            m_verticalVelocity = jumpStrength;

            if (jumpControlTime - m_jumpControlTimer <= _dt)
            {
                gravityDt = _dt - (jumpControlTime - m_jumpControlTimer);
            }
            m_jumpControlTimer += _dt;
        }

        if (!m_touchingGround && !m_isPressingJump || m_jumpControlTimer > jumpControlTime)
        {
            m_verticalVelocity += game.gravity * gravityDt;
        }

        if (!IsTouchingGround())
        {
            position.y += m_verticalVelocity * gravityDt;
        }

        // TODO: if the point collision is too hardcore, maybe spherecast and some projection on slopes since we know their angle
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(position.x, position.y + raycastUpperDistance), Vector2.down, 100.0f, LayerMask.GetMask("Ground"));

        if (hit.collider == null)
        {
            if (m_touchingGround)
            {
                if (m_outStepDownTimer > outStepDownTime)
                {
                    m_touchingGround = false;
                    m_sliding = false;
                    m_outStepDownTimer = 0.0f;
                }
                else
                {
                    m_verticalVelocity = 0.0f;
                    m_outStepDownTimer += _dt;
                }
            }
        }
        else
        {
            if (m_touchingGround)
            {
                if (hit.collider.gameObject.tag == "downhill")
                {
                    m_sliding = true;
                }
                else
                {
                    m_sliding = false;
                }

                if (hit.point.y >= position.y - stepDownDistance)
                {
                    position.y = hit.point.y;
                    m_verticalVelocity = 0.0f;
                    m_outStepDownTimer = 0.0f;
                }
                else
                {
                    if (m_outStepDownTimer > outStepDownTime)
                    {
                        m_touchingGround = false;
                        m_sliding = false;
                        m_outStepDownTimer = 0.0f;
                    }
                    else
                    {
                        position.y = hit.point.y;
                        m_verticalVelocity = 0.0f;
                        m_outStepDownTimer += _dt;
                    }
                }
            }
            else
            {
                m_sliding = false;

                if (hit.point.y > position.y)
                {
                    m_touchingGround = true;
                    position.y = hit.point.y;
                    m_verticalVelocity = 0.0f;

                    if (hit.collider.gameObject.tag == "downhill")
                    {
                        m_sliding = true;
                    }

                    if (hit.collider.gameObject.tag != "downhill" && hit.collider.gameObject.tag != "uphill" && position.y < m_jumpStartHeight)
                    {
                        m_needRoll = true;
                    }
                }
            }
        }

        transform.position = position;
    }
	
	void Update ()
	{
        // JUMP / INPUT
        /*if (input.GetJumpInput() && IsTouchingGround())
        {
            Instantiate(jumpSoundPrefab);

            m_touchingGround = false;
            m_verticalVelocity = jumpStrength;
            m_jumpStartHeight = transform.position.y;
        }*/

        if (IsTouchingGround() && !m_isPressingJump && input.IsJumping())
        {
            Instantiate(jumpSoundPrefab);

            m_isPressingJump = true;
            m_touchingGround = false;
            m_jumpStartHeight = transform.position.y;
            m_verticalVelocity = 0.0f;
        }

        if (m_isPressingJump && !input.IsJumping())
        {
            m_isPressingJump = false;
            m_jumpControlTimer = 0.0f;

        }

        UpdatePosition(Time.deltaTime);

        // ANIMATOR
        m_animator.SetBool("IsTouchingGround", IsTouchingGround());
        m_animator.SetBool("IsSliding", m_sliding);
        m_animator.SetFloat("SpeedRatio", Mathf.Min(game.gameSpeed / cruiseSpeed, 1.0f));
        if (m_needRoll)
        {
            m_animator.SetTrigger("Roll");
        }
        else
        {
            m_animator.ResetTrigger("Roll");
        }
        
        // EVENTS
        if (transform.position.y < -10.0f)
        {
            game.SendMessage("OnPlayerDied");
        }

        // RESET
        m_needRoll = false;
	}

    float m_verticalVelocity = 0.0f;
    float m_jumpStartHeight = 0.0f;
    Animator m_animator;
    bool m_touchingGround = false;
    bool m_sliding = false;
    bool m_needRoll = false;

    float m_jumpControlTimer = 0.0f;
    bool m_isPressingJump = false;

    float m_outStepDownTimer = 0.0f;
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public float niceLandingSpeedBoost = 3.0f;
    public int niceLandingScore = 100;
    public float speedBoostTime = 4.0f;
    public float speedBoostDecayTime = 2.0f;
    public float speedBoostPlayerAdvance = 2.0f;
    public float speedBoostPlayerAdvanceTime = 0.5f;


    public float startingSpeed = 6.0f;
    public float acceleration = 0.1f;
    public float gravity = -15.0f;
    public Text scoreText;

    public float scoreBumpScale = 2.0f;
    public float scoreBumpTime = 0.5f;

    public PlayerController player;

    public GameObject boostSfx;
    public GameObject deathSfx;
    private bool deathSfxhasPlayed = false;

    public float blackoutTime = 0.5f;
    public RawImage blackout;
    public float gameOverDuration = 2.0f;

    public float GetGameSpeed()
    {
        return m_gameSpeed + m_speedBoost;
    }

    void Start ()
	{
        MusicController.instance.game = this;
        deathSfxhasPlayed = false;
        m_gameSpeed = startingSpeed;

        m_scoreBumpTimer = scoreBumpTime;
        m_playerStartPosition = player.transform.position.x;
    }

    void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (Time.timeSinceLevelLoad < blackoutTime)
        {
            Color c = blackout.color;
            c.a = Mathf.Lerp(1f, 0f, Ease.QuadIn(Time.timeSinceLevelLoad / blackoutTime));
            blackout.color = c;
        }

        if (m_restartRequired)
        {
            float time = Time.timeSinceLevelLoad - m_restartRequestTime;
            Color c = blackout.color;
            c.a = Mathf.Lerp(0f, 1f, Ease.QuadIn(time / blackoutTime));
            blackout.color = c;

            if (time > blackoutTime)
            {
                // RESTART
                SceneManager.LoadScene("Main");
            }
        }

        // RESTART REQUEST
        if (m_gameOver)
        {
            if (!m_restartRequired && (Input.anyKeyDown || (Time.time - m_gameOverTime) > gameOverDuration))
            {
                m_restartRequired = true;
                m_restartRequestTime = Time.timeSinceLevelLoad;
            }
        }

        if (m_speedBoost > 0.0f)
        {
            if (m_speedBoostTimer > speedBoostTime)
            {
                Vector3 position = player.transform.position;
                position.x = m_playerStartPosition;

                if (m_speedBoostTimer < speedBoostTime + speedBoostDecayTime)
                {
                    m_speedBoost -= (Time.deltaTime / speedBoostDecayTime) * m_speedBoostReference;
                    m_speedBoost = Mathf.Max(m_speedBoost, 0.0f);

                    float ratio = Mathf.Clamp01((m_speedBoostTimer - speedBoostTime) / speedBoostDecayTime);
                    position.x = m_playerStartPosition + speedBoostPlayerAdvance * Ease.QuadOut(1.0f - ratio);
                }

                player.transform.position = position;
            }
            else
            {
                float ratio = Mathf.Clamp01(m_speedBoostPlayerAdvanceTimer / speedBoostPlayerAdvanceTime);
                Vector3 position = player.transform.position;
                position.x = Mathf.Lerp(m_playerBoostStartPosition, m_playerStartPosition + speedBoostPlayerAdvance, Ease.QuadInOut(ratio));
                player.transform.position = position;

                m_speedBoostPlayerAdvanceTimer += Time.deltaTime;
            }

            m_speedBoostTimer += Time.deltaTime;
        }

        if (!m_gameOver)
        {
            m_totalDistance += GetGameSpeed() * Time.deltaTime;
            m_gameSpeed += acceleration * Time.deltaTime;
        }

        scoreText.text = m_totalDistance.ToString("0");

        float t = Ease.QuintOut(Mathf.Clamp01(m_scoreBumpTimer / scoreBumpTime));
        scoreText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Mathf.Lerp(scoreBumpScale, 1.0f, t);

        if (m_scoreEffect)
        {
            m_scoreEffect.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f) * Mathf.Lerp(scoreBumpScale, scoreBumpScale * 3.0f, t);
            Color color = m_scoreEffect.GetComponent<Text>().color;
            color.a = Mathf.Lerp(1.0f, 0.0f, t);
            m_scoreEffect.GetComponent<Text>().color = color;
        }

        if (m_scoreBumpTimer < scoreBumpTime)
        {
            m_scoreBumpTimer += Time.deltaTime;
        }
        else
        {
            if (m_scoreEffect)
            {
                Destroy(m_scoreEffect);
                m_scoreEffect = null;
            }
        }
    }

    public void OnNiceLanding()
    {
        m_totalDistance += niceLandingScore;

        SpeedBoost(niceLandingSpeedBoost);
        BumpScore();
        Instantiate(boostSfx);
    }

    void OnPlayerDied()
    {     
        if (!deathSfxhasPlayed)
        {
            deathSfxhasPlayed = true;
            GameObject go = (GameObject)Instantiate(deathSfx, new Vector3(0, 0, 0), Quaternion.identity);
            go.transform.parent = AmbientAudioController.instance.gameObject.transform;
        }

        m_gameOver = true;
        m_gameOverTime = Time.time;
    }

    void BumpScore()
    {
        if (m_scoreEffect)
        {
            Destroy(m_scoreEffect);
            m_scoreEffect = null;
        }

        m_scoreBumpTimer = 0.0f;
        m_scoreEffect = (GameObject)Instantiate(scoreText.gameObject);
        m_scoreEffect.GetComponent<RectTransform>().SetParent(scoreText.rectTransform.parent, false);
        Destroy(m_scoreEffect, scoreBumpTime);
    }

    void SpeedBoost(float _boost)
    {
        m_speedBoost += _boost;
        m_speedBoostReference = m_speedBoost;
        m_speedBoostTimer = 0.0f;
        m_speedBoostPlayerAdvanceTimer = 0.0f;
        m_playerBoostStartPosition = player.transform.position.x;
    }

    private GameObject m_scoreEffect;

    private float m_gameSpeed = 5.0f;
    private float m_speedBoost = 0.0f;
    private float m_speedBoostTimer = 0.0f;
    private float m_speedBoostPlayerAdvanceTimer = 0.0f;
    private float m_speedBoostReference = 0.0f;
    private float m_totalDistance = 0.0f;
    private float m_scoreBumpTimer = 0.0f;
    private float m_playerStartPosition = 0.0f;
    private float m_playerBoostStartPosition = 0.0f;

    private bool m_gameOver = false;
    private bool m_restartRequired = false;
    private float m_restartRequestTime;
    private float m_gameOverTime = 0.0f;
}

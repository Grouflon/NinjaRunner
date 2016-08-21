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

    public float GetGameSpeed()
    {
        return m_gameSpeed + m_speedBoost;
    }

    void Start ()
	{
        MusicController.instance.game = this;
        m_gameSpeed = startingSpeed;

        m_scoreBumpTimer = scoreBumpTime;
        m_playerStartPosition = player.transform.position.x;
    }

    void Update ()
	{
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

        m_totalDistance += GetGameSpeed() * Time.deltaTime;
        m_gameSpeed += acceleration * Time.deltaTime;

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
    }

    void OnPlayerDied()
    {
        //SceneManager.LoadScene("Main");
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
}

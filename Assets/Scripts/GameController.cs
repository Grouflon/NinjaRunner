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


    public float startingSpeed = 6.0f;
    public float acceleration = 0.1f;
    public float gravity = -15.0f;
    public Text scoreText;

    public float scoreBumpScale = 2.0f;
    public float scoreBumpTime = 0.5f;

    public float GetGameSpeed()
    {
        return m_gameSpeed + m_speedBoost;
    }

    void Start ()
	{
        MusicController.instance.game = this;
        m_gameSpeed = startingSpeed;

        m_scoreBumpTimer = scoreBumpTime;
    }

    void Update ()
	{
        if (m_speedBoost > 0.0f)
        {
            if (m_speedBoostTimer > speedBoostTime)
            {
                if (m_speedBoostTimer < speedBoostTime + speedBoostDecayTime)
                {
                    m_speedBoost -= (Time.deltaTime / speedBoostDecayTime) * m_speedBoostReference;
                    m_speedBoost = Mathf.Max(m_speedBoost, 0.0f);
                }
            }

            m_speedBoostTimer += Time.deltaTime;
        }

        m_totalDistance += GetGameSpeed() * Time.deltaTime;
        m_gameSpeed += acceleration * Time.deltaTime;

        scoreText.text = m_totalDistance.ToString("0");

        float t = Ease.QuintOut(Mathf.Clamp01(m_scoreBumpTimer / scoreBumpTime));
        scoreText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * Mathf.Lerp(scoreBumpScale, 1.0f, t);

        if (m_scoreBumpTimer < scoreBumpTime)
            m_scoreBumpTimer += Time.deltaTime;
    }

    public void OnNiceLanding()
    {
        m_totalDistance += niceLandingScore;

        SpeedBoost(niceLandingSpeedBoost);
        BumpScore();
    }

    void OnPlayerDied()
    {
        SceneManager.LoadScene("Main");
    }

    void BumpScore()
    {
        m_scoreBumpTimer = 0.0f;
    }

    void SpeedBoost(float _boost)
    {
        m_speedBoost += _boost;
        m_speedBoostReference = m_speedBoost;
        m_speedBoostTimer = 0.0f;
    }

    private float m_gameSpeed = 5.0f;
    private float m_speedBoost = 0.0f;
    private float m_speedBoostTimer = 0.0f;
    private float m_speedBoostReference = 0.0f;
    private float m_totalDistance = 0.0f;
    private float m_scoreBumpTimer = 0.0f;
}

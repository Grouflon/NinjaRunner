using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public float gameSpeed = 5.0f;
    public float acceleration = 0.1f;
    public Text scoreText;

	void Start ()
	{
	
	}
	
	void Update ()
	{
        m_totalDistance += gameSpeed * Time.deltaTime;
        gameSpeed += acceleration * Time.deltaTime;

        scoreText.text = m_totalDistance.ToString("0.0");
	}

    void OnPlayerDied()
    {
        SceneManager.LoadScene("Main");
    }

    private float m_totalDistance = 0.0f;
}

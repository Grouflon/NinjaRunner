using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public LevelSpawnController level;
    public GameController game;
    public float acc = 0.5f;
    public float predictionTime = 1.0f;
    public bool showPrediction = false;

	void Start ()
	{
        m_camera = GetComponent<Camera>();
        m_minCameraHeight = transform.position.y;
        m_targetHeight = level.startHeight * level.unitSize;

        Vector3 position = transform.position;
        position.y = m_targetHeight;
        transform.position = position;
    }
	
	void Update ()
	{
        m_viewportHeight = Mathf.Abs(m_camera.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y - m_camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y);

        float minPlayerHeight = level.minHeight * level.unitSize;
        float maxPlayerHeight = level.maxHeight * level.unitSize;
        float maxCameraHeight = maxPlayerHeight + 4 * level.unitSize - m_viewportHeight;

        float predictionDistance = Mathf.Min(predictionTime * game.gameSpeed, m_camera.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x - player.transform.position.x - 0.5f);

        if (showPrediction)
            Debug.DrawLine(player.transform.position + new Vector3(predictionDistance, 50.0f, 0.0f), player.transform.position + new Vector3(predictionDistance, -100.0f, 0.0f), Color.red);
        
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position + new Vector3(predictionDistance, 50.0f, 0.0f), new Vector2(0.0f, -1.0f), 100.0f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag != "uphill" || hit.point.y >= m_targetHeight)
                m_targetHeight = hit.point.y;
        }

        float wantedRatio = Mathf.Clamp01((m_targetHeight - minPlayerHeight) / (maxPlayerHeight - minPlayerHeight));
        float wantedCameraHeight = m_minCameraHeight + (maxCameraHeight - m_minCameraHeight) * wantedRatio;

        Vector3 position = transform.position;
        position.y = (1.0f - acc) * position.y + acc * wantedCameraHeight;
        transform.position = position;
    }

    float m_viewportHeight;
    float m_targetHeight;
    float m_minCameraHeight;
    Camera m_camera;
}

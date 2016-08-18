using UnityEngine;
using System.Collections;

public class LevelSpawnController : MonoBehaviour
{
    public GameController game;
    public GameObject blockPrefab;

    public float unitSize = 1.0f;
    public int minBlockWidth = 5;
    public int maxBlockWidth = 12;
    public int minGapWidth = 3;
    public int maxGapWidth = 5;
    public int maxUpperDiff = 3;
    public int minLowerDiff = 5;
    public int minHeight = 1;
    public int maxHeight = 20;

    enum ModuleType
    {
        Block,
        Gap
    }

    void Start ()
	{
        m_viewportStart = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x;
        m_viewportEnd = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, 0.0f)).x;
        m_viewportLength = m_viewportEnd - m_viewportStart;
        m_predictionDistance = Mathf.Ceil(m_viewportLength / unitSize) * unitSize;

        m_currentHeight = minHeight;

        float blockWidth = m_predictionDistance;
        GameObject block = (GameObject)Instantiate(blockPrefab, new Vector3(m_viewportStart, m_currentHeight * unitSize, 0.0f), Quaternion.identity);
        BlockController blockController = block.GetComponent<BlockController>();
        blockController.game = game;
        Vector3 scale = block.transform.localScale;
        scale.x = blockWidth;
        blockController.width = blockWidth;
        block.transform.localScale = scale;

        m_previousModuleType = ModuleType.Block;
        m_distancePredicted += blockWidth;
    }
	
	void Update ()
	{
        // KEEP VARS UPDATED IF WE TWEAK DEBUGGER
        m_predictionDistance = Mathf.Ceil(m_viewportLength / unitSize) * unitSize;


        while (m_distancePredicted < m_distancePassed + m_predictionDistance)
        {
            GenerateNewModule();
        }

        m_distancePassed += game.gameSpeed * Time.deltaTime;
	}

    void GenerateNewModule()
    {
        ModuleType nextModule = ModuleType.Gap;
        float moduleSize = 0f;
        switch(m_previousModuleType)
        {
            case ModuleType.Block:  nextModule = ModuleType.Gap;    break;
            case ModuleType.Gap:    nextModule = ModuleType.Block;  break;
        }

        switch(nextModule)
        {
            case ModuleType.Gap:
                {
                    int gapWidth = Random.Range(minGapWidth, maxGapWidth);

                    moduleSize = (float)gapWidth * unitSize;
                }
                break;

            case ModuleType.Block:
                {
                    int blockWidth = Random.Range(minBlockWidth, maxBlockWidth);
                    int minHeightRange = Mathf.Max(m_currentHeight - minLowerDiff, minHeight);
                    int maxHeightRange = Mathf.Min(m_currentHeight + maxUpperDiff, maxHeight);
                    int blockHeight = Random.Range(minHeightRange, maxHeightRange);
                    m_currentHeight = blockHeight;

                    GameObject block = (GameObject)Instantiate(blockPrefab, new Vector3(m_viewportStart + (m_distancePredicted - m_distancePassed), blockHeight * unitSize, 0.0f), Quaternion.identity);
                    BlockController blockController = block.GetComponent<BlockController>();
                    blockController.game = game;
                    Vector3 scale = block.transform.localScale;
                    scale.x = blockWidth;
                    blockController.width = blockWidth;
                    block.transform.localScale = scale;

                    moduleSize = (float)blockWidth * unitSize;
                }
                break;
        }

        m_distancePredicted += moduleSize;
        m_previousModuleType = nextModule;
    }

    int m_currentHeight = 0;
    ModuleType m_previousModuleType = ModuleType.Gap;

    float m_viewportLength = 0.0f;
    float m_viewportEnd = 0.0f;
    float m_viewportStart = 0.0f;
    float m_predictionDistance = 0.0f;
    float m_distancePredicted = 0.0f;
    float m_distancePassed = 0.0f;
}

using UnityEngine;
using System.Collections;
using System;

public class LevelSpawnController : MonoBehaviour
{
    public GameController game;
    public BlockController blockPrefab;
    public SlopeController slopePrefab;

    public float unitSize = 1.0f;
    public int minBlockWidth = 5;
    public int maxBlockWidth = 12;
    public int minGapWidth = 3;
    public int maxGapWidth = 5;
    public float gapWidthSpeedMultiplier = 1.0f;
    public int maxUpperDiff = 3;
    public int minLowerDiff = 5;
    public int minHeight = 1;
    public int maxHeight = 20;

    public int minUphillWidth = 1;
    public int maxUphillWidth = 2;

    public int minDownhillWidth = 2;
    public int maxDownhillWidth = 5;

    public float blockWeight = 5.0f;
    public float gapWeight = 5.0f;
    public float uphillWeight = 1.0f;
    public float downhillWeight = 1.0f;

    // TODO: speed modifiers

    enum ModuleType
    {
        Block = 0,
        Gap,
        Uphill,
        Downhill
    }

    void Start ()
	{
        m_viewportStart = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x;
        m_viewportEnd = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f, 0.0f)).x;
        m_viewportLength = m_viewportEnd - m_viewportStart;
        m_predictionDistance = Mathf.Ceil(m_viewportLength / unitSize) * unitSize;

        m_currentHeight = minHeight;

        float blockWidth = m_predictionDistance;
        BlockController block = (BlockController)Instantiate(blockPrefab, new Vector3(m_viewportStart, m_currentHeight * unitSize, 0.0f), Quaternion.identity);
        block.game = game;
        Vector3 scale = block.transform.localScale;
        scale.x = blockWidth;
        block.width = blockWidth;
        block.transform.localScale = scale;

        m_previousModuleType = ModuleType.Block;
        m_distancePredicted += blockWidth;
    }
	
	void FixedUpdate ()
	{
        // KEEP VARS UPDATED IF WE TWEAK THINGS IN EDITOR
        m_predictionDistance = Mathf.Ceil(m_viewportLength / unitSize) * unitSize;


        while (m_distancePredicted < m_distancePassed + m_predictionDistance)
        {
            GenerateNewModule();
        }

        m_distancePassed += game.gameSpeed * Time.fixedDeltaTime;
	}

    void GenerateNewModule()
    {
        ModuleType nextModule = PickNextModuleType(m_previousModuleType);

        float moduleSize = 0f;
        switch(nextModule)
        {
            case ModuleType.Gap:
                {
                    float ratio = (game.gameSpeed / game.startingSpeed);
                    int gapWidth = UnityEngine.Random.Range(Mathf.FloorToInt((float)minGapWidth * ratio), Mathf.FloorToInt((float)maxGapWidth * ratio));

                    moduleSize = (float)gapWidth * unitSize;
                }
                break;

            case ModuleType.Block:
                {
                    int blockWidth = UnityEngine.Random.Range(minBlockWidth, maxBlockWidth);

                    if (m_previousModuleType == ModuleType.Gap)
                    {
                        int minHeightRange = Mathf.Max(m_currentHeight - minLowerDiff, minHeight);
                        int maxHeightRange = Mathf.Min(m_currentHeight + maxUpperDiff, maxHeight);
                        int blockHeight = UnityEngine.Random.Range(minHeightRange, maxHeightRange);
                        m_currentHeight = blockHeight;
                    }

                    BlockController block = (BlockController)Instantiate(blockPrefab, new Vector3(m_viewportStart + (m_distancePredicted - m_distancePassed), m_currentHeight * unitSize, 0.0f), Quaternion.identity);
                    block.game = game;
                    Vector3 scale = block.transform.localScale;
                    scale.x = blockWidth;
                    block.width = blockWidth;
                    block.transform.localScale = scale;

                    moduleSize = (float)blockWidth * unitSize;
                }
                break;

            case ModuleType.Uphill:
                {
                    int blockWidth = UnityEngine.Random.Range(minUphillWidth, maxUphillWidth);
                    blockWidth = Mathf.Min(blockWidth, maxHeight - m_currentHeight);

                    int minHeightRange = Mathf.Max(m_currentHeight - minLowerDiff, minHeight);
                    int maxHeightRange = Mathf.Min(m_currentHeight + maxUpperDiff, maxHeight - blockWidth);
                    int blockHeight = UnityEngine.Random.Range(minHeightRange, maxHeightRange);
                    m_currentHeight = blockHeight;

                    SlopeController slope = (SlopeController)Instantiate(slopePrefab, new Vector3(m_viewportStart + (m_distancePredicted - m_distancePassed), m_currentHeight * unitSize, 0.0f), Quaternion.identity);
                    slope.uphill = true;
                    BlockController block = slope.GetComponent<BlockController>();
                    block.game = game;
                    block.width = blockWidth;
                    block.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * blockWidth;

                    moduleSize = (float)blockWidth * unitSize;
                    m_currentHeight += blockWidth;
                }
                break;

            case ModuleType.Downhill:
                {
                    int blockWidth = UnityEngine.Random.Range(minDownhillWidth, maxDownhillWidth);
                    blockWidth = Mathf.Min(blockWidth, m_currentHeight - minHeight);
                    /*int minHeightRange = Mathf.Max(m_currentHeight - minLowerDiff, minHeight);
                    int maxHeightRange = Mathf.Min(m_currentHeight + maxUpperDiff, maxHeight);
                    int blockHeight = UnityEngine.Random.Range(minHeightRange, maxHeightRange);
                    m_currentHeight = blockHeight;*/

                    SlopeController slope = (SlopeController)Instantiate(slopePrefab, new Vector3(m_viewportStart + (m_distancePredicted - m_distancePassed), m_currentHeight * unitSize, 0.0f), Quaternion.identity);
                    BlockController block = slope.GetComponent<BlockController>();
                    block.game = game;
                    block.width = blockWidth;
                    block.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * blockWidth;

                    moduleSize = (float)blockWidth * unitSize;
                    m_currentHeight -= blockWidth;
                }
                break;
        }

        m_distancePredicted += moduleSize;
        m_previousModuleType = nextModule;
    }

    ModuleType PickNextModuleType(ModuleType _previousModuleType)
    {
        float[] weights = new float[Enum.GetNames(typeof(ModuleType)).Length];

        float totalWeights = 0.0f;
        switch(_previousModuleType)
        {
            case ModuleType.Block:
                {
                    weights[(int)ModuleType.Block] = 0.0f;
                    weights[(int)ModuleType.Gap] = gapWeight;
                    weights[(int)ModuleType.Uphill] = 0.0f;
                    weights[(int)ModuleType.Downhill] = m_currentHeight > (minDownhillWidth + minHeight) ? downhillWeight : 0.0f;
                }
                break;

            case ModuleType.Gap:
                {
                    weights[(int)ModuleType.Block] = blockWeight;
                    weights[(int)ModuleType.Gap] = 0.0f;
                    weights[(int)ModuleType.Uphill] = (maxHeight - m_currentHeight) >= (minUphillWidth) ? uphillWeight : 0.0f;
                    weights[(int)ModuleType.Downhill] = 0.0f;
                }
                break;

            case ModuleType.Uphill:
                {
                    weights[(int)ModuleType.Block] = blockWeight;
                    weights[(int)ModuleType.Gap] = 0.0f;
                    weights[(int)ModuleType.Uphill] = 0.0f;
                    weights[(int)ModuleType.Downhill] = 0.0f; // We may want to authorize downhill here, but I can already smell the tricky stuff
                }
                break;

            case ModuleType.Downhill:
                {
                    weights[(int)ModuleType.Block] = 0.0f;
                    weights[(int)ModuleType.Gap] = gapWeight;
                    weights[(int)ModuleType.Uphill] = 0.0f;
                    weights[(int)ModuleType.Downhill] = 0.0f;
                }
                break;
        }

        foreach (float weight in weights)
            totalWeights += weight;

        float choiceWeight = UnityEngine.Random.Range(0.0001f, totalWeights);
        int choice = 0;
        float workWeight = weights[0];
        while (choiceWeight > workWeight)
        {
            ++choice;
            workWeight += weights[choice];
        }

        return (ModuleType)choice;
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

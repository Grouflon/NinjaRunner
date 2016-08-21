using UnityEngine;
using System.Collections;

public class ModuleGroupController : MonoBehaviour
{
    public LevelSpawnController level;

    public int startingHeight = 0;
    public Sprite[] spriteSheet;

    public int m_uphillSize = 0;
    public int m_blockSize = 0;
    public int m_downhillSize = 0;

    void Start ()
	{
        /*SetUphillSize(4); //2
        SetBlockSize(4);
        SetDownhillSize(7); //7
        startingHeight = 10;*/


        Vector3 position = transform.position;
        position.y = startingHeight * level.unitSize;
        transform.position = position;

        GenerateGroup();
	}
	
    void GenerateGroup()
    {
        Dimensions d = GetDimensions();

        int startX = m_uphillSize == 0 ? -1 : 0;
        int endX = d.width + (m_downhillSize == 0 ? 1 : 0);
        int startY = d.heightUpperDiff + 1;
        int endY = -startingHeight;

        for (int y = startY; y >= endY; --y)
        {
            for (int x = startX; x < endX; ++x)
            {
                int sprite = -1;

                // FIRST ROW
                if (y == startY)
                {
                    if (x == m_uphillSize - 1)
                    {
                        sprite = 0;
                    }
                    else if (x == m_uphillSize)
                    {
                        sprite = 1;
                    }
                    else if (x == (m_uphillSize + m_blockSize - 1))
                    {
                        sprite = 3;
                    }
                    else if (x == (m_uphillSize + m_blockSize))
                    {
                        sprite = 4;
                    }
                    else if (x >= m_uphillSize && x < m_uphillSize + m_blockSize)
                    {
                        sprite = 2;
                    }
                }
                else if (x >= 0 && x < (m_uphillSize + m_blockSize + m_downhillSize))
                {
                    int downhillSize = m_downhillSize == 0 ? 2 : m_downhillSize;
                    int uphillSize = m_uphillSize == 0 ? 2 : m_uphillSize;
                    int verticalDiff = uphillSize - downhillSize;

                    // UPHILL SLOPES
                    if (x + (startY - y) == m_uphillSize)
                    {
                        if (x == 0)
                        {
                            sprite = 8;
                        }
                        else
                        {
                            sprite = 5;
                        }
                    }
                    // UPHILL SLOPES BOTTOM
                    else if (m_uphillSize != 0 && x == 0 && y == 0)
                    {
                        sprite = 10;
                    }

                    // DOWNHILL SLOPES
                    else if ((x - (m_uphillSize + m_blockSize)) == (startY - y - 1))
                    {
                        if (x == endX - 1)
                        {
                            sprite = 9;
                        }
                        else
                        {
                            sprite = 7;
                        }
                    }
                    // DOWNHILL SLOPES BOTTOM
                    else if (downhillSize != 0 && x == endX - 1 && y == (m_uphillSize - downhillSize))
                    {
                        sprite = 11;
                    }

                    // BOTTOM
                    else if (x < endX - 1)
                    {
                        // REGULAR 
                        if (verticalDiff == 0)
                        {
                            if (y == 0)
                            {
                                sprite = 12;
                            }
                            else if (y < 0)
                            {
                                if (x > 0 || m_uphillSize == 0)
                                {
                                    sprite = 15;
                                }
                            }
                            else
                            {
                                if (((startY - y) + x > m_uphillSize) && (x - (m_uphillSize + m_blockSize)) < (startY - y))
                                {
                                    sprite = 6;
                                }
                            }
                        }
                        // ACENDING
                        else if (verticalDiff > 0)
                        {
                            // UPHILL
                            if (x < uphillSize - verticalDiff)
                            {
                                if (x > 0)
                                {
                                    if (y == 0)
                                    {
                                        sprite = 12;
                                    }
                                    else if (y < 0)
                                    {
                                        sprite = 15;
                                    }
                                    else if ((startY - y) + x > uphillSize)
                                    {
                                        sprite = 6;
                                    }
                                }
                            }
                            // DOWNHILL + BLOCK
                            else if (x >= uphillSize)
                            {
                                if (y == verticalDiff)
                                {
                                    sprite = 12;
                                }
                                else if (y < verticalDiff)
                                {
                                    sprite = 15;
                                }
                                else if ((x - (uphillSize + m_blockSize)) < (startY - y))
                                {
                                    sprite = 6;
                                }

                            }
                            // ARCH
                            else if (y == 0 && x == downhillSize)
                            {
                                sprite = 17;
                            }
                            // CHANGE ZONE
                            else
                            {
                                if (y > 0 && y <= verticalDiff && x + (startY - y) == uphillSize + downhillSize)
                                {
                                    sprite = 14;
                                }
                                else if (x + (startY - y) > uphillSize + downhillSize)
                                {
                                    sprite = 15;
                                }
                                else if (x + (startY - y) < uphillSize + downhillSize && (startY - y) + x > uphillSize)
                                {
                                    sprite = 6;
                                }
                            }

                        }
                        // DESCENDING
                        else if (verticalDiff < 0)
                        {
                            float baseHeight = m_uphillSize == 0 ? -2 : 0; // Don't ask

                            // UPHILL + BLOCK
                            if (x < m_uphillSize + m_blockSize && x > startX)
                            {
                                if (y == baseHeight)
                                {
                                    sprite = 12;
                                }
                                else if (y < baseHeight)
                                {
                                    sprite = 15;
                                }
                                else if ((x + (startY - y) > m_uphillSize))
                                {
                                    sprite = 6;
                                }
                            }
                            // DOWNHILL
                            else if (x >= d.width - uphillSize)
                            {
                                if (y == verticalDiff + baseHeight)
                                {
                                    sprite = 12;
                                }
                                else if ( y < verticalDiff + baseHeight)
                                {
                                    sprite = 15;
                                }
                                else if ((x - (m_uphillSize + m_blockSize)) < (startY - y))
                                {
                                    sprite = 6;
                                }
                            }
                            // ARCH
                            else if (y == verticalDiff + baseHeight && x == (d.width - m_uphillSize - 1))
                            {
                                sprite = 16;
                            }
                            // CHANGE ZONE
                            else if (x > 0)
                            {
                                
                                if (y > verticalDiff + baseHeight && y <= baseHeight && (x - m_blockSize - baseHeight + 1) == (startY - y))
                                {
                                    sprite = 13;
                                }
                                else if ((x - m_blockSize - baseHeight + 1) > (startY - y) && (x - (m_uphillSize + m_blockSize )) < (startY - y))
                                {
                                    sprite = 6;
                                }
                                else if ((x - m_blockSize + 1) < (startY - y))
                                {
                                    sprite = 15;
                                }
                            }
                            
                        }
                    }
                }

                CreateBlock(x, y, sprite);
            }
        }
    }

	void Update ()
	{
	
	}

    struct Dimensions
    {
        public int width;
        public int heightUpperDiff;
        public int heightLowerDiff;
    }

    void CreateBlock(int _x, int _y, int _sprite)
    {
        if (_sprite < 0)
            return;

        GameObject block = new GameObject();
        block.name = "Block";
        SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
        sr.sprite = spriteSheet[_sprite];

        block.transform.SetParent(transform);
        Vector3 position = block.transform.localPosition;
        position.x = _x * level.unitSize;
        position.y = _y * level.unitSize;
        block.transform.localPosition = position;
    }

    Dimensions GetDimensions()
    {
        Dimensions result = new Dimensions();
        result.width = m_uphillSize + m_blockSize + m_downhillSize;
        result.heightUpperDiff = m_uphillSize;
        result.heightLowerDiff = Mathf.Min(m_uphillSize - m_downhillSize, 0);
        return result;
    }
}

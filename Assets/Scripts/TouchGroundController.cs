using UnityEngine;
using System.Collections;

public class TouchGroundController : MonoBehaviour
{
    public bool IsTouchingGround()
    {
        return m_triggerCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ++m_triggerCount;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            --m_triggerCount;
        }
    }

    int m_triggerCount = 0;
}

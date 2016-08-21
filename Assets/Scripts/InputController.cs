using UnityEngine;
using System.Collections;

public abstract class InputController : MonoBehaviour
{
    public abstract bool GetJumpInput();
    public abstract bool IsJumping();
}

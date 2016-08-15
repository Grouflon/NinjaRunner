using UnityEngine;
using System.Collections;
using System;

public class KeyboardInputController : InputController
{
    public override bool GetJumpInput()
    {
        return Input.anyKeyDown;
    }
}

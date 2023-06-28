using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAction : MonoBehaviour
{
    public abstract void Set();
    public abstract void Unset();
    public abstract void KeyAction();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePathBase : ScriptableObject
{
    public enum Type
    {
        SceneOnly,
        Tinder,
        Chat,
        Tour,
        Break
    }

    [SerializeField]
    public Type type = Type.SceneOnly;
}

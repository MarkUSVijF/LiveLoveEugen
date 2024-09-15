using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TinderPath", menuName = "Scriptable Objects/Scene Path/Tinder")]
public class TinderPath : ScenePathBase
{
    TinderPath()
    {
        type = Type.Tinder;
    }

    // each no-match continues the scene
    [SerializeField]
    public TinderScene[] scene;

    [SerializeField]
    public ScenePathBase any_accept;
    [SerializeField]
    public ScenePathBase last_nope;
}

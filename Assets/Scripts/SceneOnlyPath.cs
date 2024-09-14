using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneOnlyPath", menuName = "Scriptable Objects/Scene Path/Scene Only")]
public class SceneOnlyPath : ScenePathBase
{
    [SerializeField]
    SceneBase scene;

    [SerializeField]
    public ScenePathBase follow_up;
}

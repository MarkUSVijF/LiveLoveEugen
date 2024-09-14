using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "TourPath", menuName = "Scriptable Objects/Scene Path/Tour")]
public class TourPath : ScenePathBase
{
    TourPath()
    {
        type = Type.Tour;
    }
    [SerializeField]
    Scene[] linearPath;
}

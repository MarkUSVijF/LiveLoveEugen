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
    public TourScene[] scene;

    [SerializeField]
    public ScenePathBase go_right;
    [SerializeField]
    public ScenePathBase go_left;
    [HideInInspector]
    public ScenePathBase next
    {
        get { return go_right; }
    }
}

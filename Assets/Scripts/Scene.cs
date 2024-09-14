using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Scene : SwipeController//, MonoBehaviour
{
    public new void Update()
    {
        var old_state = state;
        ((SwipeController)this).Update();

        if (state != old_state)
        {

        }
    }
}
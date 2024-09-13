using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public Rigidbody2D controllable;
    public Rigidbody2D getController()
    {
        return controllable;
    }
}

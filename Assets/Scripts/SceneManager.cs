using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    public enum Subclass
    {
        Horizontal,
        Upward,
        Horizontal_Paning
    }

    public Scene scene;

    public Scene[] scenes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Scene getActiveScene()
    {
        return scene;
    }
}

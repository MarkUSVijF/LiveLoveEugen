using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static SceneDescribtion;

[System.Serializable]
public class ImageOptions
{
    public Vector3 offset;
    public bool mirror_sprite = false;
}

[System.Serializable]
public class SingleDialog
{
    public Sprite Emotion;
    public string dialoge;
    public ImageOptions options;
}

[System.Serializable]
public class SingleChoice
{
    public Sprite Emotion;
    public string choice_left;
    public string choice_right;
    public ImageOptions options;
}

[CreateAssetMenu(fileName = "TourScene", menuName = "Scriptable Objects/Scene/Tour")]
public class TourScene : SceneBase
{
    /*
     * Tour
     * -) Background
     * -) Eugen
     * -) is choice
     * -) Text (Left/Right)
    */
    
    public Sprite Background;
    [SerializeField]
    public SingleDialog[] dialog = { };

    public bool has_end_choice = false;
    [SerializeField]
    [ConditionalField(nameof(has_end_choice))] public SingleChoice end_choice = null;
    // if no endchoice goto next scene
}
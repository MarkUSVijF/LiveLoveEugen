using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SceneDescribtion
{
    public enum SceneType
    {
        None,
        Tinder,
        Tour,
        Break
    }
    public Scene scene;
    public SceneType type;
    [ConditionalField(nameof(type), false, SceneType.Tinder)] public Scene acceptScene;
    [ConditionalField(nameof(type), false, SceneType.Tinder)] public Scene denyScene;
}
public class SceneManager : MonoBehaviour
{
    [SerializeField]
    public SceneDescribtion[] scenes;
    public Dictionary<Scene, SceneDescribtion> scene_map;
    public SceneDescribtion active_scene;


    // Start is called before the first frame update
    void Awake()
    {
        scene_map = new Dictionary<Scene, SceneDescribtion>();
        for (int i = 0; i < scenes.Length; i++)
        {
            scene_map.Add(scenes[i].scene, scenes[i]);
        }
        active_scene = scenes[0];
        active_scene.scene.manager = this;
        active_scene.scene.gameObject.SetActive(true);
    }

    public Scene getActiveScene()
    {
        return active_scene.scene;
    }

    public void switchScene(float status)
    {
        var old_scene = active_scene;
        switch (active_scene.type)
        {
            case SceneDescribtion.SceneType.Tinder:
                {
                    if (status == 1)
                    {
                        active_scene = scene_map[active_scene.acceptScene];
                    }
                    if (status == -1)
                    {
                        active_scene = scene_map[active_scene.denyScene];
                    }
                }
                break;
            default:
                break;
        }
        if (active_scene != old_scene)
        {
            old_scene.scene.gameObject.SetActive(false);
            active_scene.scene.manager = this;
            active_scene.scene.gameObject.SetActive(true);
        }
    }
}

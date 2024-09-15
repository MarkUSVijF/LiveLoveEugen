using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public ScenePathBase startpoint;

    private ScenePathBase current_path;
    private int[] sub_scene = { 0, 0, 0 };
    private Scene active_scene = null;

    public GameObject TinderPrefab;
    public GameObject TourPrefab;
    public GameObject ChoicePrefab;

    public void Awake()
    {
        current_path = startpoint;
        sub_scene = new int[] { 0, 0, 0 };

        initScene();
    }

    public void initScene()
    {
        var old_scene = active_scene;
        switch (current_path.type)
        {
            case ScenePathBase.Type.SceneOnly:
                {

                }
                break;
            case ScenePathBase.Type.Tinder:
                {
                    var instance = Instantiate(TinderPrefab, transform);
                    active_scene = instance.GetComponent<Scene>();
                    active_scene.loader = this;
                    instance.GetComponent<LoadTinder>().Load((current_path as TinderPath).scene[sub_scene[0]]);
                }
                break;
            case ScenePathBase.Type.Chat:
                {

                }
                break;
            case ScenePathBase.Type.Tour:
                {
                    var _scene = (current_path as TourPath).scene[sub_scene[0]];
                    if (_scene.has_end_choice && sub_scene[1] >= _scene.dialog.Length)
                    {
                        var instance = Instantiate(ChoicePrefab, transform);
                        active_scene = instance.GetComponent<Scene>();
                        active_scene.loader = this;
                        instance.GetComponent<LoadTour>().Load((current_path as TourPath).scene[sub_scene[0]], sub_scene[1]);
                    }
                    else
                    {
                        var instance = Instantiate(TourPrefab, transform);
                        active_scene = instance.GetComponent<Scene>();
                        active_scene.loader = this;
                        instance.GetComponent<LoadTour>().Load((current_path as TourPath).scene[sub_scene[0]], sub_scene[1]);
                    }
                }
                break;
            case ScenePathBase.Type.Break:
                {

                }
                break;
            default:
                break;
        }
        if (old_scene != active_scene && old_scene != null)
        {
            old_scene.gameObject.SetActive(false);
            Destroy(old_scene.gameObject);
        }
    }

    public void switchScene(float state)
    {
        if (Mathf.Abs(state) < 1)
        {
            return;
        }
        switch (current_path.type)
        {
            case ScenePathBase.Type.SceneOnly:
                {

                }
                break;
            case ScenePathBase.Type.Tinder:
                {
                    var _path = (current_path as TinderPath);
                    if (state >= 1)
                    {
                        current_path = _path.any_accept;
                        sub_scene = new int[] { 0, 0, 0 };

                    }
                    else
                    {
                        sub_scene[0]++;
                        if (sub_scene[0] >= _path.scene.Length)
                        {
                            current_path = _path.last_nope;
                            sub_scene = new int[] { 0, 0, 0 };
                        }
                    }
                    initScene();
                }
                break;
            case ScenePathBase.Type.Chat:
                {

                }
                break;
            case ScenePathBase.Type.Tour:
                {
                    var _path = (current_path as TourPath);
                    var _scene = _path.scene[sub_scene[0]];

                    var has_choice = (_scene.has_end_choice && sub_scene[1] >= _scene.dialog.Length);

                    sub_scene[1]++; // select next dialogue
                    if (sub_scene[1] >= _scene.dialog.Length)
                    {
                        // end of dialoge -> next scene
                        // or CHOICE

                        // is last scene
                        if (sub_scene[0] == _path.scene.Length - 1)
                        {
                            // is choice on end of path
                            if (_scene.has_end_choice)
                            {
                                if (sub_scene[1] >= _scene.dialog.Length + 1)
                                {
                                    if (state >= 1)
                                    {
                                        current_path = _path.go_right;
                                        sub_scene = new int[] { 0, 0, 0 };
                                    }
                                    else
                                    {
                                        current_path = _path.go_left;
                                        sub_scene = new int[] { 0, 0, 0 };
                                    }
                                }
                            }
                            else
                            {
                                current_path = _path.next;
                                sub_scene = new int[] { 0, 0, 0 };
                            }
                        }
                        else
                        {
                            sub_scene[0]++;
                            sub_scene[1] = 0;
                        }
                    }
                    initScene();
                }
                break;
            case ScenePathBase.Type.Break:
                {

                }
                break;
            default:
                break;
        }
    }
    public Scene getActiveScene()
    {
        return active_scene;
    }
}

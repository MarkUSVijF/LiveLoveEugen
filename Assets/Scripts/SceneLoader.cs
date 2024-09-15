using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public ScenePathBase startpoint;
    private ScenePathBase current_path;
    private Scene active = null;

    public GameObject TinderPrefab;

    public void Awake()
    {
        UpdateState(0);
    }

    public void UpdateState(float state)
    {
        switch (startpoint.type)
        {
            case ScenePathBase.Type.SceneOnly:
                {

                }
                break;
            case ScenePathBase.Type.Tinder:
                {
                    var instance = Instantiate(TinderPrefab, transform);
                    active = instance.GetComponent<Scene>();
                    instance.GetComponent<LoadTinder>().Load((startpoint as TinderPath).scene[0]);
                }
                break;
            case ScenePathBase.Type.Chat:
                {

                }
                break;
            case ScenePathBase.Type.Tour:
                {

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
        return active;
    }
}

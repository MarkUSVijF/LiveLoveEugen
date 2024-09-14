using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDevice : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    SceneManager sceneManager;
    Scene active_scene = null;
    bool is_swiping = false;

    public void Awake()
    {
        sceneManager = GetComponent<SceneManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.getActiveScene(eventData).SwipeStart(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.getActiveScene(eventData).OnSwipe(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.getActiveScene(eventData).SwipeEnd(eventData.position);
    }

    private Scene getActiveScene(PointerEventData eventData)
    {
        var _scene = sceneManager.getActiveScene();
        if (active_scene != _scene)
        {
            if (is_swiping)
            {
                active_scene?.SwipeEnd(eventData.position);
                _scene.SwipeStart(eventData.position);
            }
            active_scene = _scene;
        }
        return active_scene;
    }
}

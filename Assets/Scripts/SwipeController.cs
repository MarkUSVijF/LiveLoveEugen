using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[SerializeField]
public enum SwipeOption
{
    Horizontal,
    Upward
} 
public class SwipeController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{


    public Scene scene;
    //public Rigidbody2D scene.getController();
    public SwipeOption direction = SwipeOption.Horizontal;
    public float minDistance = 1;//25;
    public float maxVelocity = 10000;//200;
    public float rot_multiplier = 15;
    public float dead_angle = 2;//5;
    public float select_angle = 5;//15;

    public float state = 0;

    public bool active = false;
    public Vector2 inital_pos;
    public Vector2 swipe_start;
    public Vector2 finger_pos;

    public Vector2 temp;

    public void Awake()
    {
        inital_pos = scene.getController().position;
        finger_pos = Vector2.zero;
        swipe_start = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        active = true;
        swipe_start = eventData.position;
        finger_pos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        finger_pos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        active = false;
        scene.getController().velocity = Vector2.zero;
        scene.getController().angularVelocity = 0f;
        finger_pos = Vector2.zero;
        swipe_start = Vector2.zero;
    }

    public void Update()
    {
        var swipe = (finger_pos - swipe_start);

        if (direction == SwipeOption.Horizontal)
        {
            var cur_rot = scene.getController().rotation;
            var target_rot = swipe.x / 450 * (-rot_multiplier);
            var targetVelocity = target_rot - cur_rot;
            var magnitude = Math.Abs(targetVelocity);
            //var direction = magnitude == 0 ? 0 : targetVelocity / magnitude;

            if (magnitude >= minDistance)
            {
                targetVelocity /= magnitude;
                targetVelocity *= Math.Min(maxVelocity, magnitude * 10);
                scene.getController().angularVelocity = targetVelocity;
            }
            else if (magnitude >= 1)
            {
                scene.getController().angularVelocity = targetVelocity;
            }
            else
            {
                if (active)
                {
                    scene.getController().angularVelocity = 0f;
                }
                else
                {
                    scene.getController().position = inital_pos;
                    scene.getController().velocity = Vector2.zero;
                    scene.getController().rotation = 0;
                    scene.getController().angularVelocity = 0;
                }
            }

            if (Math.Abs(cur_rot) < dead_angle)
            {
                state = 0;
            }
            else if (Math.Abs(cur_rot) > select_angle)
            {
                state = -cur_rot / Math.Abs(cur_rot);
            }
            else
            {
                state = -((Math.Abs(cur_rot) - dead_angle) / (select_angle - dead_angle)) * (cur_rot / Math.Abs(cur_rot));
            }
        }
        else
        {
            var targetVelocity = ((finger_pos - swipe_start) - (scene.getController().position - inital_pos));
            targetVelocity.Scale(new Vector2(0f, 1f));
            var distance = targetVelocity.magnitude;

            if (active)
            {
                if (((finger_pos - swipe_start) + (scene.getController().position - inital_pos)).y <= 0)
                {
                    scene.getController().position = inital_pos;
                    scene.getController().velocity = Vector2.zero;
                    swipe_start = finger_pos;
                    return;
                }
                else if (distance >= minDistance)
                {
                    targetVelocity /= distance;
                    targetVelocity *= Math.Min(maxVelocity, distance * 10);
                    scene.getController().velocity = targetVelocity;
                }
                else
                {
                    scene.getController().velocity = Vector2.zero;
                }
            }
            else
            {
                if (distance >= minDistance)
                {
                    targetVelocity /= distance;
                    targetVelocity *= Math.Min(maxVelocity, distance * 10);
                    scene.getController().velocity = targetVelocity;
                }
                else if (distance >= 1)
                {
                    scene.getController().velocity = targetVelocity;
                }
                else
                {
                    scene.getController().position = inital_pos;
                    scene.getController().velocity = Vector2.zero;
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SwipeAble : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float minDistance = 25;
    public float maxVelocity = 200;
    bool active = false;
    Vector3 inital_pos;
    Vector3 swipe_start;
    Vector3 finger_pos;
    public float rot_multiplier = 75;
    public float dead_angle = 5;
    public float select_angle = 15;

    public float state = 0;

    public void Awake()
    {
        inital_pos = transform.position;
        finger_pos = Vector3.zero;
        swipe_start = Vector3.zero;
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
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        finger_pos = Vector3.zero;
        swipe_start = Vector3.zero;
    }

    public void Update()
    {
        /*var targetVelocity = ((finger_pos - swipe_start) - (transform.position - inital_pos));
        targetVelocity.Scale(new Vector3(1f, 0f, 0f));
        var distance = targetVelocity.magnitude;

        if (active)
        {
            if (distance >= minDistance)
            {
                targetVelocity /= distance;
                targetVelocity *= Math.Min(maxVelocity, distance*10);
                GetComponent<Rigidbody2D>().velocity = targetVelocity;
            } else
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
        else {
            if (distance >= minDistance)
            {
                targetVelocity /= distance;
                targetVelocity *= Math.Min(maxVelocity, distance * 10);
                GetComponent<Rigidbody2D>().velocity = targetVelocity;
            }
            else if (distance >= 1)
            {
                GetComponent<Rigidbody2D>().velocity = targetVelocity;
            }
            else
            {
                GetComponent<Rigidbody2D>().position = inital_pos;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }*/
        var cur_rot = GetComponent<Rigidbody2D>().rotation;
        var target_rot = (finger_pos - swipe_start).x / 450 * (-rot_multiplier);
        var targetVelocity = target_rot - cur_rot;
        var magnitude = Math.Abs(targetVelocity);
        //var direction = magnitude == 0 ? 0 : targetVelocity / magnitude;

        if (magnitude >= minDistance)
        {
            targetVelocity /= magnitude;
            targetVelocity *= Math.Min(maxVelocity, magnitude * 10);
            GetComponent<Rigidbody2D>().angularVelocity = targetVelocity;
        }
        else if (magnitude >= 1)
        {
            GetComponent<Rigidbody2D>().angularVelocity = targetVelocity;
        }
        else
        {
            if (active)
            {
                GetComponent<Rigidbody2D>().angularVelocity = 0f;
            }
            else
            {
                GetComponent<Rigidbody2D>().position = inital_pos;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().rotation = 0;
                GetComponent<Rigidbody2D>().angularVelocity = 0;
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
            state = -((Math.Abs(cur_rot) - dead_angle) / (select_angle - dead_angle))*(cur_rot / Math.Abs(cur_rot));
        }
    }
}

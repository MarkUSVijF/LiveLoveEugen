using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeAble : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float minDistance = 25;
    public float maxVelocity = 200;
    public bool active = false;
    public Vector3 inital_pos;
    public Vector3 swipe_start;
    public Vector3 finger_pos;

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
        finger_pos = Vector3.zero;
        swipe_start = Vector3.zero;
    }

    public void Update()
    {
        var targetVelocity = ((finger_pos - swipe_start) - (transform.position - inital_pos));
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
        }
    }
}

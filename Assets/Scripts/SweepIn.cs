using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepIn : MonoBehaviour, SwipeAble
{
    public SwipeController controller;
    private Vector3 init_position;
    public Vector3 position_offset;
    private bool is_active = false;
    private Vector3 init_velocity;
    [Range(0f, 1f)]
    public float speed = 0.5f;

    void Awake()
    {
        controller.registerSwipeAble(this);
        init_position = transform.position;
    }

    void OnEnable()
    {
        is_active = true;
        transform.position = init_position + position_offset;
    }

    void FixedUpdate()
    {
        if (is_active)
        {
            var start_distance = (position_offset).magnitude;
            var current_distance = (transform.position - init_position).magnitude;
            transform.position = init_position + (current_distance / start_distance) * position_offset * (1- speed);
            if (current_distance < 1)
            {
                transform.position = init_position;
                is_active = false;
            }
        }
    }

    public void SwipeStart(Vector2 position)
    {
        if (is_active)
        {
            transform.position = init_position;
            is_active=false;
        }
    }
    public void OnSwipe(Vector2 position) { }
    public void SwipeEnd(Vector2 position) { }
}

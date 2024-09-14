using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class SwipeController : MonoBehaviour
{
    [SerializeField]
    public enum SwipeOption
    {
        Horizontal,
        Upward,
        Horizontal_Paning
    }

    public Rigidbody2D controllable;
    //public Rigidbody2D controllable;
    public SwipeOption direction = SwipeOption.Horizontal;
    public float min_change = 2;//5;
    public float max_change = 5;//15;

    public float state = 0;

    public bool active = false;
    public Vector2 inital_pos;
    public float inital_rot;
    public Vector2 swipe_start;
    public Vector2 finger_pos;

    public Rigidbody2D getController()
    {
        GetComponent<Scene>();
        return controllable;
    }

    public void Awake()
    {
        inital_pos = controllable.position;
        inital_rot = controllable.rotation;

        finger_pos = Vector2.zero;
        swipe_start = Vector2.zero;
    }

    public void SwipeStart(Vector2 position)
    {
        active = true;
        swipe_start = position;
        finger_pos = position;
    }

    public void OnSwipe(Vector2 position)
    {
        finger_pos = position;
    }

    public void SwipeEnd(Vector2 position)
    {
        active = false;
        controllable.velocity = Vector2.zero;
        controllable.angularVelocity = 0f;
        finger_pos = Vector2.zero;
        swipe_start = Vector2.zero;
    }
    public void Update()
    {
        var swipe = (finger_pos - swipe_start);
        float cur_change = 0f;

        switch (direction)
        {
            case SwipeOption.Horizontal:
                {
                    cur_change = -(controllable.rotation - inital_rot);
                    var target_rot = swipe.x / 450 * (-15);
                    var targetVelocity = target_rot - cur_change;
                    var magnitude = Math.Abs(targetVelocity);
                    //var direction = magnitude == 0 ? 0 : targetVelocity / magnitude;

                    if (magnitude >= 1)
                    {
                        targetVelocity /= magnitude;
                        targetVelocity *= Math.Min(10000, magnitude * 10);
                        controllable.angularVelocity = targetVelocity;
                    }
                    else if (magnitude >= 1)
                    {
                        controllable.angularVelocity = targetVelocity;
                    }
                    else
                    {
                        if (active)
                        {
                            controllable.angularVelocity = 0f;
                        }
                        else
                        {
                            controllable.position = inital_pos;
                            controllable.velocity = Vector2.zero;
                            controllable.rotation = 0;
                            controllable.angularVelocity = 0;
                        }
                    }
                }
                break;
            case SwipeOption.Horizontal_Paning:
                {
                    cur_change = (controllable.position.x - inital_pos.x);
                    var targetVelocity = ((finger_pos - swipe_start) - (controllable.position - inital_pos));
                    targetVelocity.Scale(new Vector2(1f, 0f));
                    var distance = targetVelocity.magnitude;

                    if (active)
                    {
                        if (distance >= 1)
                        {
                            targetVelocity /= distance;
                            targetVelocity *= Math.Min(10000, distance * 10);
                            controllable.velocity = targetVelocity;
                        }
                        else
                        {
                            controllable.velocity = Vector2.zero;
                        }
                    }
                    else
                    {
                        if (distance >= 1)
                        {
                            targetVelocity /= distance;
                            targetVelocity *= Math.Min(10000, distance * 10);
                            controllable.velocity = targetVelocity;
                        }
                        else if (distance >= 1)
                        {
                            controllable.velocity = targetVelocity;
                        }
                        else
                        {
                            controllable.position = inital_pos;
                            controllable.velocity = Vector2.zero;
                        }
                    }
                }
                break;
            case SwipeOption.Upward:
                {
                    cur_change = (controllable.position.y - inital_pos.y);
                    var targetVelocity = ((finger_pos - swipe_start) - (controllable.position - inital_pos));
                    targetVelocity.Scale(new Vector2(0f, 1f));
                    var distance = targetVelocity.magnitude;

                    if (active)
                    {
                        if (((finger_pos - swipe_start) + (controllable.position - inital_pos)).y <= 0)
                        {
                            controllable.position = inital_pos;
                            controllable.velocity = Vector2.zero;
                            swipe_start = finger_pos;
                            return;
                        }
                        else if (distance >= 1)
                        {
                            targetVelocity /= distance;
                            targetVelocity *= Math.Min(10000, distance * 10);
                            controllable.velocity = targetVelocity;
                        }
                        else
                        {
                            controllable.velocity = Vector2.zero;
                        }
                    }
                    else
                    {
                        if (distance >= 1)
                        {
                            targetVelocity /= distance;
                            targetVelocity *= Math.Min(10000, distance * 10);
                            controllable.velocity = targetVelocity;
                        }
                        else if (distance >= 1)
                        {
                            controllable.velocity = targetVelocity;
                        }
                        else
                        {
                            controllable.position = inital_pos;
                            controllable.velocity = Vector2.zero;
                        }
                    }
                }
                break;
            default:
                break;
        }


        if (Math.Abs(cur_change) < min_change)
        {
            state = 0;
        }
        else if (Math.Abs(cur_change) > max_change)
        {
            state = cur_change / Math.Abs(cur_change);
        }
        else
        {
            state = ((Math.Abs(cur_change) - min_change) / (max_change - min_change)) * (cur_change / Math.Abs(cur_change));
        }
    }
}

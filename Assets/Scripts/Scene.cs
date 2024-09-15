using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scene : SwipeController//, MonoBehaviour
{
    [HideInInspector]
    public SceneManager manager = null;
    [HideInInspector]
    public SceneLoader loader = null;

    [SerializeField]
    public Image outline;
    [ConditionalField(nameof(outline))] public Color positive_color = Color.green;
    [ConditionalField(nameof(outline))] public Color negative_color = Color.red;
    private bool uses_outline;

    [SerializeField]
    public Tour_Options tour_otions;
    private bool uses_otions;


    public new void Awake()
    {
        ((SwipeController)this).Awake();
        uses_outline = outline != null;
        if (uses_outline) outline.SetAlpha(0f);
        uses_otions = tour_otions.isValid();
        if (uses_otions) tour_otions.updateState(0);
    }

    public new void Update()
    {
        var old_state = state;
        ((SwipeController)this).Update();

        if (state != old_state)
        {
            if (uses_outline)
            {
                if (state > 0)
                {
                    outline.color = positive_color;
                } else
                {
                    outline.color = negative_color;
                }
                outline.SetAlpha(Math.Abs(state));
            }
            if (uses_otions)
            {
                tour_otions.updateState(state);
            }
        }
    }

    public new void SwipeEnd(Vector2 position)
    {
        ((SwipeController)this).SwipeEnd(position);
        if (Math.Abs(state) >= 1)
        {
            if (manager != null) manager.switchScene(state);
            if (loader != null) loader.switchScene(state);
        }
    }
}
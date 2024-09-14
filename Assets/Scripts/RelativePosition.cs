using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RelativePosition : MonoBehaviour
{
    public Transform parent;
    private Vector3 init_parent_position;
    private Vector3 init_position;
    public float scale = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        init_position = transform.localPosition;
        init_parent_position = parent.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = init_position - (parent.transform.localPosition - init_parent_position) + (parent.transform.localPosition - init_parent_position)* scale;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Freya;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class LineForCard : MonoBehaviour
{

    public Transform b;
    
    public Transform a;

    public LineRenderer lineRenderer;

    public Transform circle;

    [Range(0, 1)]
    public float rate = 0.0f;
    
    void Start()
    {
        Debug.Log("a");
    }

    // Update is called once per frame
    void Update()
    {
        
        lineRenderer.SetPosition(0, a.position);
        lineRenderer.SetPosition(1, b.position);


        circle.transform.position = B(a.transform, b.transform, rate);

    }
    
    Vector3 B(Transform pointA, Transform pointB, float t)
    {
        return (1 - t) * pointA.position + pointB.position * t;
    }
}

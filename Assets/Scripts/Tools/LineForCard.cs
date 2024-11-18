using System;
using System.Collections;
using System.Collections.Generic;
// using Freya;
using Shapes;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

// [ExecuteInEditMode]
public class LineForCard : MonoBehaviour
{

    public Transform b;
    
    public Transform a;

    public Line lineRenderer;

    public Transform circle;

    [Range(0, 1)]
    public float rate = 0.0f;
    
    public float rate1 = 0.0f;
    private bool xk = true;

    void Start()
    {
        lineRenderer.Start = a.position;
        lineRenderer.End = b.position;
        Observable.Timer(TimeSpan.FromSeconds(10))
            .Subscribe(v =>
            {
                xk = false;
            });
    }
    

    // Update is called once per frame
    void Update()
    {
        if (xk) return;
        
        if (rate1 < 1)
        {
            rate1 += Time.deltaTime / 2;
            circle.transform.position = B(a.transform, b.transform, rate1);
        }

    }
    
    Vector3 B(Transform pointA, Transform pointB, float t)
    {
        return (1 - t) * pointA.position + pointB.position * t;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Algorithm;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    [ExecuteInEditMode]
    public class CurveForCard: MonoBehaviour
    {
        [Range(0, 1)]
        public float rate = 0.0f;


        public LineForCard lineForCard1;
        public LineForCard lineForCard2;


        public Transform circle;

        public LineRenderer lineRenderer;

        public List<Transform> allSpots = new();
        private void Update()
        {
            // D(100);
            lineForCard1.rate = rate;
            lineForCard2.rate = rate;

            circle.transform.position = B(lineForCard1.circle.transform, lineForCard2.circle.transform, rate);
        }
        
        Vector3 B(Transform pointA, Transform pointB, float t)
        {
            return (1 - t) * pointA.position + pointB.position * t;
        }

        Vector3 C(Vector3 pointA, Vector3 pointB, float t)
        {
            return (1 - t) * pointA + pointB * t;
        }

        public int offset = 10;

        [Button]
        public List<int> FF(int count)
        {
            var newC = new List<int>();

            // var m = - 1;
            
            for (int i = 0; i < count; i++)
            {
                var ca = -5 * (count - 1) + offset * i;
                // m = (m * -1);
                newC.Add(ca + 50);
            }

            
            
            
            Debug.Log(string.Join(",",newC.Select(v => v.ToString())));

            return newC;
        }

        [Button]
        public void DD()
        {

            var array = new Vector3[101];
            (0, 100)
                .Enumerate(v =>
                {

                    var i = v * 1.0f / 100;

                    var a = B(lineForCard1.a.transform, lineForCard1.b.transform, i);
                    var b = B(lineForCard2.a.transform, lineForCard2.b.transform, i);

                    var spot = C(a, b, i);
                    array[v] = spot;
                });

            lineRenderer.positionCount = 100;
            lineRenderer.SetPositions(array);

            allSpots.ForEach(v => Destroy(v));
            allSpots.Clear();
        }


        [Button]
        public Vector3[] D(int count)
        {

            var array = new Vector3[101];
            (0, 100)
                .Enumerate(v =>
                {

                    var i = v * 1.0f / 100;
                    
                    var a = B(lineForCard1.a.transform, lineForCard1.b.transform, i);
                    var b = B(lineForCard2.a.transform, lineForCard2.b.transform, i);

                    var spot = C(a, b, i);
                    array[v] = spot;
                });

            lineRenderer.positionCount = 100;
            lineRenderer.SetPositions(array);

            allSpots.ForEach(v => Destroy(v));
            allSpots.Clear();

            var newC = FF(count);

            return newC.Select(v => array[v]).ToArray();
        }
    }
}
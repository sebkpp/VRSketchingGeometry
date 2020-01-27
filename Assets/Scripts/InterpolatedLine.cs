﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KochanekBartelsSplines.Interpolation;

public class InterpolatedLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer LineRenderer;

    [SerializeField]
    private Vector3[] ControlPoints;

    private List<Vector3> InterpolatedPoints = new List<Vector3>();
    private int steps = 20;
    // Start is called before the first frame update
    void Start()
    {
        if (ControlPoints.Length < 3) {
            return;        
        }
        for (int i = 0; i < ControlPoints.Length; i++) {

            //first control point
            if (i == 0) {
                Vector3 point1 = ControlPoints[i];
                Vector3 point2 = ControlPoints[i];
                Vector3 point3 = ControlPoints[i + 1];
                Vector3 point4 = ControlPoints[i + 2];

                InterpolatedPoints.AddRange(InterpolatedPointsCalculator.GetInterpolatedPoints(point1, point2, point3, point4, 0, 0, 0, steps));
            }
            //middle control point
            else if (i + 2 < ControlPoints.Length) {
                Vector3 point1 = ControlPoints[i - 1];
                Vector3 point2 = ControlPoints[i];
                Vector3 point3 = ControlPoints[i + 1];
                Vector3 point4 = ControlPoints[i + 2];

                InterpolatedPoints.AddRange(InterpolatedPointsCalculator.GetInterpolatedPoints(point1, point2, point3, point4, 0, 0, 0, steps));
            }
            //last control point
            else if (i + 1 < ControlPoints.Length)
            {
                Vector3 point1 = ControlPoints[i - 1];
                Vector3 point2 = ControlPoints[i];
                Vector3 point3 = ControlPoints[i + 1];
                Vector3 point4 = ControlPoints[i + 1];

                InterpolatedPoints.AddRange(InterpolatedPointsCalculator.GetInterpolatedPoints(point1, point2, point3, point4, 0, 0, 0, steps));
            }
            
        }
        LineRenderer.positionCount = InterpolatedPoints.Count;
        LineRenderer.SetPositions(InterpolatedPoints.ToArray());
    }
}

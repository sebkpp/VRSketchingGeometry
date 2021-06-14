using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.Splines;
using VRSketchingGeometry.Meshing;
using VRSketchingGeometry.Serialization;

namespace VRSketchingGeometry.SketchObjectManagement
{
    /// <summary>
    /// A line sketch object with no smooth interpolation between control point.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class LinearInterpolationStrokeSketchObject : StrokeSketchObject, ISerializableComponent
    {
        protected override SplineMesh MakeSplineMesh(int interpolationSteps, Vector3 lineDiameter)
        {
            return new SplineMesh(new LinearInterpolationSpline(), lineDiameter);
        }

        SerializableComponentData ISerializableComponent.GetData()
        {
            StrokeSketchObjectData data = base.GetData();
            data.Interpolation = StrokeSketchObjectData.InterpolationType.Linear;
            return data;
        }
    }
}
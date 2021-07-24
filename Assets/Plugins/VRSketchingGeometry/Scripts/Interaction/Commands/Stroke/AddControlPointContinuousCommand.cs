using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Stroke {
    /// <summary>
    /// Add control point at the end of spline if it is at least a certain distance away from the last control point.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class AddControlPointContinuousCommand : ICommand
    {
        private StrokeSketchObject StrokeSketchObject;
        private Vector3 NewControlPoint;

        public AddControlPointContinuousCommand(StrokeSketchObject strokeSketchObject, Vector3 controlPoint) {
            this.StrokeSketchObject = strokeSketchObject;
            this.NewControlPoint = controlPoint;
        }

        public bool Execute()
        {
            return StrokeSketchObject.AddControlPointContinuous(NewControlPoint);
        }

        public void Redo()
        {
            if (SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject))
            {
                SketchWorld.ActiveSketchWorld.RestoreObject(this.StrokeSketchObject);
            }
            StrokeSketchObject.AddControlPoint(NewControlPoint);
        }

        public void Undo()
        {
            StrokeSketchObject.DeleteControlPoint();
            if (this.StrokeSketchObject.getNumberOfControlPoints() == 0)
            {
                SketchWorld.ActiveSketchWorld.DeleteObject(this.StrokeSketchObject);
            }
        }
    }
}

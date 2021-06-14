using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Stroke {
    /// <summary>
    /// Add control point at the end of the spline.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class AddControlPointCommand : ICommand
    {
        private StrokeSketchObject StrokeSketchObject;
        private Vector3 NewControlPoint;

        public AddControlPointCommand(StrokeSketchObject strokeSketchObject, Vector3 controlPoint) {
            this.StrokeSketchObject = strokeSketchObject;
            this.NewControlPoint = controlPoint;
        }

        public bool Execute()
        {
            StrokeSketchObject.AddControlPoint(NewControlPoint);
            return true;
        }

        public void Redo()
        {
            if (SketchWorld.ActiveSketchWorld != null && SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject)) {
                SketchWorld.ActiveSketchWorld.RestoreObject(this.StrokeSketchObject);
            }
            this.Execute();
        }

        public void Undo()
        {
            StrokeSketchObject.DeleteControlPoint();
            if (this.StrokeSketchObject.getNumberOfControlPoints() == 0) {
                SketchWorld.ActiveSketchWorld.DeleteObject(this.StrokeSketchObject);
            }
        }
    }
}

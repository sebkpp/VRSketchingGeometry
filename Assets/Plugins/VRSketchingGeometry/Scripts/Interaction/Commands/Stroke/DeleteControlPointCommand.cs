using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Stroke
{
    /// <summary>
    /// Delete control point at the end of spline.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class DeleteControlPointCommand : ICommand
    {
        private StrokeSketchObject StrokeSketchObject;
        private Vector3 OldControlPoint;

        public DeleteControlPointCommand(StrokeSketchObject strokeSketchObject)
        {
            this.StrokeSketchObject = strokeSketchObject;
        }

        public bool Execute()
        {
            this.OldControlPoint = StrokeSketchObject.GetControlPoints()[StrokeSketchObject.getNumberOfControlPoints() - 1];
            StrokeSketchObject.DeleteControlPoint();
            if (this.StrokeSketchObject.getNumberOfControlPoints() == 0)
            {
                SketchWorld.ActiveSketchWorld.DeleteObject(this.StrokeSketchObject);
            }
            return true;
        }

        public void Redo()
        {
            this.Execute();
        }

        public void Undo()
        {
            if (SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject))
            {
                SketchWorld.ActiveSketchWorld.RestoreObject(this.StrokeSketchObject);
            }
            StrokeSketchObject.AddControlPoint(OldControlPoint);
        }
    }
}

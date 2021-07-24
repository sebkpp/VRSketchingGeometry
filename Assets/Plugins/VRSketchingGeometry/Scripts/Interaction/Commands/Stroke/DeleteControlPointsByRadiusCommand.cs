using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Stroke
{
    /// <summary>
    /// Delete control points within a sphere volume defined by a point and radius.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class DeleteControlPointsByRadiusCommand : ICommand
    {
        private StrokeSketchObject OriginalStrokeSketchObject;
        /// <summary>
        /// New sketch objects created during deletion of control points.
        /// </summary>
        private List<StrokeSketchObject> NewLines;
        /// <summary>
        /// Control points of OriginalLineSketchObject before deletion.
        /// </summary>
        private List<Vector3> OriginalControlPoints;
        /// <summary>
        /// Control points of OriginalLineSketchObject after deletion.
        /// </summary>
        private List<Vector3> NewControlPoints;
        /// <summary>
        /// Point around which is deleted.
        /// </summary>
        private Vector3 Point;
        /// <summary>
        /// Radius around Point.
        /// </summary>
        private float Radius;

        /// <summary>
        /// Command for deleting control points within a radius around a point of a line sketch object. 
        /// </summary>
        /// <param name="strokeSketchObject"></param>
        /// <param name="point">Point around which the control points will be deleted.</param>
        /// <param name="radius">Radius around point in which the control points are deleted.</param>
        public DeleteControlPointsByRadiusCommand(StrokeSketchObject strokeSketchObject, Vector3 point, float radius)
        {
            this.OriginalStrokeSketchObject = strokeSketchObject;
            this.Point = point;
            this.Radius = radius;
        }

        public bool Execute()
        {
            this.OriginalControlPoints = OriginalStrokeSketchObject.GetControlPoints();
            bool didDelete = OriginalStrokeSketchObject.DeleteControlPoints(Point, Radius, out NewLines);
            if (OriginalStrokeSketchObject.gameObject.activeInHierarchy)
            {
                NewControlPoints = OriginalStrokeSketchObject.GetControlPoints();
            }
            else {
                NewControlPoints = null;
            }
            return didDelete;
        }

        public void Redo()
        {
            if (NewControlPoints == null)
            {
                SketchWorld.ActiveSketchWorld.DeleteObject(OriginalStrokeSketchObject);
            }
            else {
                OriginalStrokeSketchObject.SetControlPointsLocalSpace(NewControlPoints);
            }

            foreach (StrokeSketchObject line in NewLines)
            {
                SketchWorld.ActiveSketchWorld.RestoreObject(line);
            }
        }

        public void Undo()
        {
            if (!OriginalStrokeSketchObject.gameObject.activeInHierarchy) {
                SketchWorld.ActiveSketchWorld.RestoreObject(OriginalStrokeSketchObject);
            }
            OriginalStrokeSketchObject.SetControlPointsLocalSpace(OriginalControlPoints);

            foreach (StrokeSketchObject line in NewLines) {
                SketchWorld.ActiveSketchWorld.DeleteObject(line);
            }

        }
    }
}

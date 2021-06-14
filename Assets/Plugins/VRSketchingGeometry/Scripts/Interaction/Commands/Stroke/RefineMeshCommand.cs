using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Commands.Stroke
{
    /// <summary>
    /// Refine the mesh of a line sketch object using the Parallel Transport algorithm.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class RefineMeshCommand : ICommand
    {
        StrokeSketchObject StrokeSketchObject;
        List<Vector3> OriginalControlPoints;

        public RefineMeshCommand(StrokeSketchObject strokeSketchObject)
        {
            this.StrokeSketchObject = strokeSketchObject;
            OriginalControlPoints = strokeSketchObject.GetControlPoints();
        }

        public bool Execute()
        {
            StrokeSketchObject.RefineMesh();
            return true;
        }

        public void Redo()
        {
            this.Execute();
        }

        public void Undo()
        {
            this.StrokeSketchObject.SetControlPointsLocalSpace(OriginalControlPoints);
        }
    }
}
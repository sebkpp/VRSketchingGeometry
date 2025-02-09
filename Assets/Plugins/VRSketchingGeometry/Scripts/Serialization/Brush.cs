﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.Serialization;
using VRSketchingGeometry.SketchObjectManagement;

namespace VRSketchingGeometry.Serialization {
    /// <summary>
    /// Represents visual settings for a sketch object.
    /// </summary>
    /// <remarks>Original author: tterpi</remarks>
    public class Brush
    {
        public SketchMaterialData SketchMaterial;
    }

    /// <summary>
    /// Brush for <see cref="StrokeSketchObject"/>.
    /// </summary>
    public class StrokeBrush : Brush {
        public float CrossSectionScale;
        public List<Vector3> CrossSectionVertices;
        public List<Vector3> CrossSectionNormals;
        public int InterpolationSteps;
    }

    /// <summary>
    /// Brush for <see cref="VRSketchingGeometry.SketchObjectManagement.RibbonSketchObject"/>.
    /// </summary>
    public class RibbonBrush : Brush {
        public Vector3 CrossSectionScale;
        public List<Vector3> CrossSectionVertices;
    }
}

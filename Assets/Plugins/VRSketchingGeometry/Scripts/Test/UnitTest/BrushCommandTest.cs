using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Serialization;
using VRSketchingGeometry.Commands.Stroke;
using VRSketchingGeometry.Commands.Ribbon;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class BrushCommandTest
    {
        private RibbonSketchObject Ribbon;
        private StrokeSketchObject Stroke;
        private PatchSketchObject Patch;
        private CommandInvoker Invoker;

        [UnitySetUp]
        public IEnumerator SetUpScene()
        {
            yield return SceneManager.LoadSceneAsync("CommandTestScene", LoadSceneMode.Single);
            this.Ribbon = GameObject.FindObjectOfType<RibbonSketchObject>();
            this.Stroke = GameObject.FindObjectOfType<StrokeSketchObject>();
            this.Patch = GameObject.FindObjectOfType<PatchSketchObject>();
            yield return null;
            Invoker = new CommandInvoker();
        }

        [Test]
        public void SetBrushOnStrokeSketchObject()
        {
            ICommand addCommand = new AddControlPointCommand(this.Stroke, new Vector3(0,0,0));
            Invoker.ExecuteCommand(addCommand);
            addCommand = new AddControlPointCommand(this.Stroke, new Vector3(1, 1, 1));
            Invoker.ExecuteCommand(addCommand);
            addCommand = new AddControlPointCommand(this.Stroke, new Vector3(2, 2, 0));
            Invoker.ExecuteCommand(addCommand);
            Assert.AreEqual((2*20 + 2) * 7, this.Stroke.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

            StrokeBrush brush = this.Stroke.GetBrush() as StrokeBrush;
            brush.SketchMaterial.AlbedoColor = Color.green;
            brush.CrossSectionVertices.Add(Vector3.one);
            brush.CrossSectionNormals.Add(Vector3.one);
            brush.InterpolationSteps = 10;
            ICommand SetBrushCommand = new SetBrushCommand(this.Stroke, brush);
            Invoker.ExecuteCommand(SetBrushCommand);

            Assert.AreEqual(Color.green, this.Stroke.GetComponent<MeshRenderer>().sharedMaterial.color);
            StrokeBrush updatedBrush = this.Stroke.GetBrush() as StrokeBrush;
            Assert.AreEqual(Color.green, updatedBrush.SketchMaterial.AlbedoColor);
            Assert.AreEqual((2 * 10 + 2) * 8, this.Stroke.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void SetBrushOnStrokeSketchObjectUndo()
        {
            ICommand addCommand = new AddControlPointCommand(this.Stroke, new Vector3(0, 0, 0));
            Invoker.ExecuteCommand(addCommand);
            addCommand = new AddControlPointCommand(this.Stroke, new Vector3(1, 1, 1));
            Invoker.ExecuteCommand(addCommand);
            addCommand = new AddControlPointCommand(this.Stroke, new Vector3(2, 2, 0));
            Invoker.ExecuteCommand(addCommand);
            Assert.AreEqual((2 * 20 + 2) * 7, this.Stroke.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

            StrokeBrush brush = this.Stroke.GetBrush() as StrokeBrush;
            Color originalColor = brush.SketchMaterial.AlbedoColor;
            brush.SketchMaterial.AlbedoColor = Color.green;
            brush.CrossSectionVertices.Add(Vector3.one);
            brush.CrossSectionNormals.Add(Vector3.one);
            ICommand SetBrushCommand = new SetBrushCommand(this.Stroke, brush);
            Invoker.ExecuteCommand(SetBrushCommand);
            Invoker.Undo();

            Assert.AreEqual(originalColor, this.Stroke.GetComponent<MeshRenderer>().sharedMaterial.color);
            StrokeBrush updatedBrush = this.Stroke.GetBrush() as StrokeBrush;
            Assert.AreEqual(originalColor, updatedBrush.SketchMaterial.AlbedoColor);
            Assert.AreEqual((2 * 20 + 2) * 7, this.Stroke.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void SetBrushOnStrokeSketchObjectRedo()
        {
            ICommand addCommand = new AddControlPointCommand(this.Stroke, new Vector3(0, 0, 0));
            Invoker.ExecuteCommand(addCommand);
            addCommand = new AddControlPointCommand(this.Stroke, new Vector3(1, 1, 1));
            Invoker.ExecuteCommand(addCommand);
            addCommand = new AddControlPointCommand(this.Stroke, new Vector3(2, 2, 0));
            Invoker.ExecuteCommand(addCommand);
            Assert.AreEqual((2 * 20 + 2) * 7, this.Stroke.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

            StrokeBrush brush = this.Stroke.GetBrush() as StrokeBrush;
            brush.SketchMaterial.AlbedoColor = Color.green;
            brush.CrossSectionVertices.Add(Vector3.one);
            brush.CrossSectionNormals.Add(Vector3.one);
            ICommand SetBrushCommand = new SetBrushCommand(this.Stroke, brush);
            Invoker.ExecuteCommand(SetBrushCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(Color.green, this.Stroke.GetComponent<MeshRenderer>().sharedMaterial.color);
            StrokeBrush updatedBrush = this.Stroke.GetBrush() as StrokeBrush;
            Assert.AreEqual(Color.green, updatedBrush.SketchMaterial.AlbedoColor);
            Assert.AreEqual((2 * 20 + 2) * 8, this.Stroke.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void SetBrushOnRibbonSketchObject() {
            ICommand addCommand = new AddPointAndRotationCommand(this.Ribbon, new Vector3(0,0,0), Quaternion.Euler(0,0,45) );
            Invoker.ExecuteCommand(addCommand);

            addCommand = new AddPointAndRotationCommand(this.Ribbon, new Vector3(1, 1, 1), Quaternion.Euler(0, 0, -45));
            Invoker.ExecuteCommand(addCommand);

            addCommand = new AddPointAndRotationCommand(this.Ribbon, new Vector3(1, 1, 1), Quaternion.Euler(45, 0, 0));
            Invoker.ExecuteCommand(addCommand);

            Assert.AreEqual(3 * 3, this.Ribbon.GetComponent<MeshFilter>().sharedMesh.vertexCount);

            RibbonBrush brush = this.Ribbon.GetBrush() as RibbonBrush;
            brush.SketchMaterial.AlbedoColor = Color.cyan;
            brush.CrossSectionVertices.Add(Vector3.one);
            ICommand SetBrushCommand = new SetBrushCommand(this.Ribbon, brush);
            Invoker.ExecuteCommand(SetBrushCommand);

            Assert.AreEqual(4 * 3, this.Ribbon.GetComponent<MeshFilter>().sharedMesh.vertexCount);
            Assert.AreEqual(Color.cyan, this.Ribbon.GetComponent<MeshRenderer>().sharedMaterial.color);
            RibbonBrush updatedBrush = this.Ribbon.GetBrush() as RibbonBrush;
            Assert.AreEqual(Color.cyan, updatedBrush.SketchMaterial.AlbedoColor);
        }

        [Test]
        public void SetBrushOnPatchObject() {
            Brush brush = this.Patch.GetBrush();
            brush.SketchMaterial.AlbedoColor = Color.magenta;
            Assert.AreNotEqual(Color.magenta, this.Patch.GetComponent<MeshRenderer>().sharedMaterial.color);

            ICommand SetBrushCommand = new SetBrushCommand(this.Patch, brush);
            Invoker.ExecuteCommand(SetBrushCommand);

            Assert.AreEqual(Color.magenta, this.Patch.GetComponent<MeshRenderer>().sharedMaterial.color);
        }
    }
}

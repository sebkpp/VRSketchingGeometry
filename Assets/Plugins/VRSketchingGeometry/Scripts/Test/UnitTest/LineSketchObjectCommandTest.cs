using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Commands.Stroke;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class StrokeSketchObjectCommandsTest
    {
        private StrokeSketchObject StrokeSketchObject;
        private CommandInvoker Invoker;

        [UnitySetUp]
        public IEnumerator SetUpScene()
        {
            yield return SceneManager.LoadSceneAsync("CommandTestScene", LoadSceneMode.Single);
            this.StrokeSketchObject = GameObject.FindObjectOfType<StrokeSketchObject>();
            yield return null;
            Invoker = new CommandInvoker();
        }

        [Test]
        public void AddOneControlPointToStrokeWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);
            Assert.IsTrue(this.StrokeSketchObject.GetControlPoints()[0] == new Vector3(1,2,3));
        }

        [Test]
        public void AddOneControlPointToStrokeWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();
            Invoker.Redo();

            Assert.IsTrue(this.StrokeSketchObject.GetControlPoints()[0] == new Vector3(1, 2, 3));
            Assert.IsFalse(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject));
        }

        [Test]
        public void AddOneControlPointToStrokeWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 0);
            Assert.IsTrue(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject));
        }

        [Test]
        public void DeleteOneControlPointWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            DeleteControlPointCommand deleteCommand = new DeleteControlPointCommand(this.StrokeSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 0);
            Assert.IsTrue(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject));
        }

        [Test]
        public void DeleteOneControlPointWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            DeleteControlPointCommand deleteCommand = new DeleteControlPointCommand(this.StrokeSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 1);
            Assert.IsFalse(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject));
        }

        [Test]
        public void DeleteOneControlPointWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            DeleteControlPointCommand deleteCommand = new DeleteControlPointCommand(this.StrokeSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 0);
            Assert.IsTrue(SketchWorld.ActiveSketchWorld.IsObjectDeleted(this.StrokeSketchObject));
        }

        [Test]
        public void AddMultipleControlPointsWithCommand() {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 3);
            Assert.AreEqual((2 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsAndDeleteOneWithCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointCommand(this.StrokeSketchObject);
            Invoker.ExecuteCommand(deleteCommand);

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 3);
            Assert.AreEqual((2 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsAndDeleteOneWithCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointCommand(this.StrokeSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddMultipleControlPointsAndDeleteOneWithCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointCommand(this.StrokeSketchObject);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 3);
            Assert.AreEqual((2 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void AddControlPointContinuousCommand() {
            ICommand addControlPointCommand = new AddControlPointContinuousCommand(this.StrokeSketchObject, new Vector3(1,2,3));
            Invoker.ExecuteCommand(addControlPointCommand);

            addControlPointCommand = new AddControlPointContinuousCommand(this.StrokeSketchObject, new Vector3(1,3.001f,3));
            Invoker.ExecuteCommand(addControlPointCommand);

            Assert.AreEqual(2, this.StrokeSketchObject.getNumberOfControlPoints());
        }

        [Test]
        public void AddControlPointContinuousCommand2()
        {
            ICommand addControlPointCommand = new AddControlPointContinuousCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(addControlPointCommand);

            addControlPointCommand = new AddControlPointContinuousCommand(this.StrokeSketchObject, new Vector3(1, 2.999f, 3));
            Invoker.ExecuteCommand(addControlPointCommand);

            Assert.AreEqual(1, this.StrokeSketchObject.getNumberOfControlPoints());
        }


        [Test]
        public void DeleteByRadiusCommand()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointsByRadiusCommand(this.StrokeSketchObject, new Vector3(3,3.5f,3), .6f);
            Invoker.ExecuteCommand(deleteCommand);

            Assert.AreEqual(2, this.StrokeSketchObject.getNumberOfControlPoints());
            Assert.AreEqual((2 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void DeleteByRadiusCommandUndo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointsByRadiusCommand(this.StrokeSketchObject, new Vector3(3, 3.5f, 3), .6f);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();

            Assert.AreEqual(4, this.StrokeSketchObject.getNumberOfControlPoints());
            Assert.AreEqual((3*20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }

        [Test]
        public void DeleteByRadiusCommandRedo()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            ICommand deleteCommand = new DeleteControlPointsByRadiusCommand(this.StrokeSketchObject, new Vector3(3, 3.5f, 3), .6f);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();

            Assert.AreEqual(2, this.StrokeSketchObject.getNumberOfControlPoints());
            Assert.AreEqual((2 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

            StrokeSketchObject[] lines = GameObject.FindObjectsOfType<StrokeSketchObject>();
            Assert.AreEqual(2, lines.Length);
            foreach (StrokeSketchObject line in lines) {
                if (line != this.StrokeSketchObject) {
                    Assert.AreEqual(1, line.getNumberOfControlPoints());
                    Assert.AreEqual(new Vector3(4, 3, 2), line.GetControlPoints()[0]);
                }
            }
        }

        [Test]
        public void RefineMeshTest()
        {
            AddControlPointCommand command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(1, 2, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(2, 3, 4));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(3, 3, 3));
            Invoker.ExecuteCommand(command);

            command = new AddControlPointCommand(this.StrokeSketchObject, new Vector3(4, 3, 2));
            Invoker.ExecuteCommand(command);

            this.StrokeSketchObject.RefineMesh();

            Assert.AreEqual(this.StrokeSketchObject.getNumberOfControlPoints(), 4);
            Assert.AreEqual((3 * 20 + 2) * 7, this.StrokeSketchObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);
        }
    }
}

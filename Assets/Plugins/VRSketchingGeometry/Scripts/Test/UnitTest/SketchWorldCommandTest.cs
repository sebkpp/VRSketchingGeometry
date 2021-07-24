using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VRSketchingGeometry;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.SketchObjectManagement;

namespace Tests
{
    public class SketchWorldCommandTest
    {
        private SketchWorld SketchWorld;
        private StrokeSketchObject StrokeSketchObject;
        private CommandInvoker Invoker;

        [UnitySetUp]
        public IEnumerator SetUpScene() {
            yield return SceneManager.LoadSceneAsync("CommandTestScene", LoadSceneMode.Single);
            this.SketchWorld = GameObject.FindObjectOfType<SketchWorld>();
            this.StrokeSketchObject = GameObject.FindObjectOfType<StrokeSketchObject>();
            yield return null;
            Invoker = new CommandInvoker();
        }

        [Test]
        public void AddObjectToSketchWorldTest()
        {
            AddObjectToSketchWorldRootCommand addCommand = new AddObjectToSketchWorldRootCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(addCommand);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "RootSketchObjectGroup");
        }

        [Test]
        public void AddObjectToSketchWorldUndoTest()
        {
            AddObjectToSketchWorldRootCommand addCommand = new AddObjectToSketchWorldRootCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(addCommand);
            Invoker.Undo();
            Assert.IsFalse(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "Deleted Bin");
        }

        [Test]
        public void AddObjectToSketchWorldRedoTest()
        {
            AddObjectToSketchWorldRootCommand addCommand = new AddObjectToSketchWorldRootCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(addCommand);
            Invoker.Undo();
            Assert.IsFalse(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "Deleted Bin");

            Invoker.Redo();
            Assert.IsTrue(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "RootSketchObjectGroup");
        }

        [Test]
        public void DeleteObjectTest()
        {
            AddObjectToSketchWorldRootCommand addCommand = new AddObjectToSketchWorldRootCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(addCommand);
            Assert.IsTrue(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "RootSketchObjectGroup");

            DeleteObjectCommand deleteCommand = new DeleteObjectCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(deleteCommand);
            Assert.IsFalse(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "Deleted Bin");
        }

        [Test]
        public void DeleteObjectUndoTest()
        {
            AddObjectToSketchWorldRootCommand addCommand = new AddObjectToSketchWorldRootCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(addCommand);
            Assert.IsTrue(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "RootSketchObjectGroup");

            DeleteObjectCommand deleteCommand = new DeleteObjectCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Assert.IsTrue(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "RootSketchObjectGroup");
        }

        [Test]
        public void DeleteObjectRedoTest()
        {
            AddObjectToSketchWorldRootCommand addCommand = new AddObjectToSketchWorldRootCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(addCommand);
            Assert.IsTrue(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "RootSketchObjectGroup");

            DeleteObjectCommand deleteCommand = new DeleteObjectCommand(this.StrokeSketchObject, this.SketchWorld);
            Invoker.ExecuteCommand(deleteCommand);
            Invoker.Undo();
            Invoker.Redo();
            Assert.IsFalse(this.StrokeSketchObject.gameObject.activeInHierarchy);
            Assert.IsTrue(this.StrokeSketchObject.transform.IsChildOf(this.SketchWorld.transform));
            Assert.IsTrue(this.StrokeSketchObject.transform.parent.name == "Deleted Bin");
        }
    }
}

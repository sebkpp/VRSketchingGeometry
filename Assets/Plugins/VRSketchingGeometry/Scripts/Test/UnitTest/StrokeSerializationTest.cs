using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools.Utils;

namespace Tests
{
    public class StrokeSerializationTest
    {
        private StrokeSketchObject StrokeSketchObject;
        private ISerializableComponent SerializableSketchObject;

        [UnitySetUp]
        public IEnumerator SetUpScene()
        {
            yield return SceneManager.LoadSceneAsync("CommandTestScene", LoadSceneMode.Single);
            this.StrokeSketchObject = GameObject.FindObjectOfType<StrokeSketchObject>();
            this.SerializableSketchObject = this.StrokeSketchObject;
            yield return null;
        }

        [Test]
        public void GetData_ControlPoints()
        {
            this.StrokeSketchObject.SetControlPointsLocalSpace(new List<Vector3> { new Vector3(1, 2, 3), new Vector3(3, 2, 1), new Vector3(1, 1, 1), new Vector3(2, 2, 2) });
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            Assert.AreEqual(new Vector3(3, 2, 1), data.ControlPoints[1]);
        }

        [Test]
        public void GetData_CrossSection()
        {
            List<Vector3> crossSection = new List<Vector3> { new Vector3(0, 0, 1), new Vector3(.5f, 0, 0), new Vector3(-.5f, 0, 0) };
            this.StrokeSketchObject.SetStrokeCrossSection(crossSection, crossSection, .3f);
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            Assert.AreEqual(new Vector3(.5f, 0, 0), data.CrossSectionVertices[1]);
            Assert.AreEqual(new Vector3(0, 0, 1), data.CrossSectionVertices[0]);
            Assert.AreEqual(.3f, data.CrossSectionScale);
        }

        [Test]
        public void GetData_Position() {
            this.StrokeSketchObject.transform.position = new Vector3(1,2,3);
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            Assert.AreEqual(new Vector3(1, 2, 3), data.Position);
        }

        [Test]
        public void GetData_Rotation()
        {
            this.StrokeSketchObject.transform.rotation = Quaternion.Euler(0,25,0);
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            Assert.That(data.Rotation, Is.EqualTo(Quaternion.Euler(0, 25, 0)).Using(QuaternionEqualityComparer.Instance));
        }

        [Test]
        public void GetData_Scale()
        {
            this.StrokeSketchObject.transform.localScale = new Vector3(3,3,3);
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            Assert.AreEqual(new Vector3(3, 3, 3), data.Scale);
        }

        [Test]
        public void ApplyData_ControlPoints()
        {
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            data.ControlPoints = new List<Vector3> { new Vector3(1, 2, 3), new Vector3(3, 2, 1), new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            this.SerializableSketchObject.ApplyData(data);
            Assert.AreEqual(new Vector3(3, 2, 1), this.StrokeSketchObject.GetControlPoints()[1]);
            Assert.AreEqual((3 * 20 + 2) * 7, this.StrokeSketchObject.gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount);
        }

        [Test]
        public void ApplyData_CrossSection()
        {
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            List<Vector3> crossSection = new List<Vector3> { new Vector3(0, 0, 1), new Vector3(.5f, 0, 0), new Vector3(-.5f, 0, 0) };
            data.ControlPoints = new List<Vector3> { new Vector3(1, 2, 3), new Vector3(3, 2, 1), new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            data.CrossSectionVertices = crossSection;
            data.CrossSectionNormals = crossSection;
            data.CrossSectionScale = 3.0f;
            this.SerializableSketchObject.ApplyData(data);
            Assert.AreEqual(new Vector3(3, 2, 1), this.StrokeSketchObject.GetControlPoints()[1]);
            Assert.AreEqual((3 * 20 + 2) * 3, this.StrokeSketchObject.gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount);
        }

        [Test]
        public void ApplyData_Position() {
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            data.Position = new Vector3(2, 5, 8);
            this.SerializableSketchObject.ApplyData(data);
            Assert.AreEqual(new Vector3(2, 5, 8), this.StrokeSketchObject.gameObject.transform.position);
        }

        [Test]
        public void ApplyData_Rotation()
        {
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            data.Rotation = Quaternion.Euler(10, 20, 30);
            this.SerializableSketchObject.ApplyData(data);
            Assert.That(this.StrokeSketchObject.gameObject.transform.rotation, Is.EqualTo(Quaternion.Euler(10, 20, 30)).Using(QuaternionEqualityComparer.Instance));
        }

        [Test]
        public void ApplyData_Scale()
        {
            StrokeSketchObjectData data = this.SerializableSketchObject.GetData() as StrokeSketchObjectData;
            data.Scale = new Vector3(1,2,3);
            this.SerializableSketchObject.ApplyData(data);
            Assert.AreEqual(new Vector3(1, 2, 3), this.StrokeSketchObject.gameObject.transform.localScale);
        }
    }
}

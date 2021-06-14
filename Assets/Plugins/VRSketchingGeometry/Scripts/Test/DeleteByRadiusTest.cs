using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry.Export;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.Serialization;
using VRSketchingGeometry;
using VRSketchingGeometry.Meshing;
using VRSketchingGeometry.Serialization;

public class DeleteByRadiusTest : MonoBehaviour
{
    public DefaultReferences defaults;
    public GameObject selectionPrefab;
    [FormerlySerializedAs("LineSketchObjectPrefab")] public GameObject StrokeSketchObjectPrefab;
    private StrokeSketchObject strokeSketchObject;
    private StrokeSketchObject strokeSketchObject2;
    public GameObject controlPointParent;
    public GameObject deletePoint;
    public float deleteRadius;

    private bool ranOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        strokeSketchObject = Instantiate(StrokeSketchObjectPrefab).GetComponent<StrokeSketchObject>();
        strokeSketchObject.SetStrokeDiameter(.5f);

        strokeSketchObject2 = Instantiate(defaults.LinearInterpolationStrokeSketchObjectPrefab).GetComponent<StrokeSketchObject>();
        strokeSketchObject2.SetStrokeDiameter(.5f);
    }

    IEnumerator changeDiameter() {
        yield return new WaitForSeconds(5);
        strokeSketchObject.DeleteControlPoints(deletePoint.transform.position, deleteRadius, out List<StrokeSketchObject> newLines);
        OBJExporter exporter = new OBJExporter();
        string exportPath = OBJExporter.GetDefaultExportPath();
        //exporter.ExportGameObject(lineSketchObject.gameObject, exportPath);
        //Debug.Log(JsonUtility.ToJson(lineSketchObject));
        //XMLSerializeTest();
        //XMLSerializeTest2();
        //exporter.ExportGameObject(controlPointParent, exportPath);
        strokeSketchObject.SetInterpolationSteps(4);
        strokeSketchObject.RefineMesh();
        strokeSketchObject2.RefineMesh();

        //Debug.Log(exportPath);
        //lineSketchObject.setLineDiameter(.1f);
        //yield return new WaitForSeconds(2);
        //lineSketchObject.deleteControlPoint();
        //lineSketchObject.deleteControlPoint();
    }

    private void XMLSerializeTest() {
        //LineSketchObject lso = new LineSketchObject();

        string path = System.IO.Path.Combine(Application.dataPath, "test_sketch.xml");
        Debug.Log(path);
        // Serialize the object to a file.
        // First write something so that there is something to read ...  
        var writer = new System.Xml.Serialization.XmlSerializer(typeof(Vector3[]));
        var wfile = new System.IO.StreamWriter(path);
        writer.Serialize(wfile, strokeSketchObject);
        wfile.Close();

        // Now we can read the serialized book ...  
        System.Xml.Serialization.XmlSerializer reader =
            new System.Xml.Serialization.XmlSerializer(typeof(Vector3[]));
        System.IO.StreamReader file = new System.IO.StreamReader(
            path);
        Vector3[] overview = (Vector3[]) reader.Deserialize(file);
        file.Close();
    }

    private void XMLSerializeTest2() {
        string path = Serializer.WriteTestXmlFile<VRSketchingGeometry.Serialization.SerializableComponentData>
            ((strokeSketchObject as ISerializableComponent).GetData());
        Serializer.DeserializeFromXmlFile(out StrokeSketchObjectData readData, System.IO.Path.Combine(Application.dataPath, "TestSerialization.xml"));
        StrokeSketchObject deserStroke = Instantiate(StrokeSketchObjectPrefab).GetComponent<StrokeSketchObject>();
        readData.SketchMaterial.AlbedoColor = Color.red;
        (deserStroke as ISerializableComponent).ApplyData(readData);

        deserStroke.transform.position += new Vector3(0,2,0);

        Debug.Log(readData.ControlPoints.Count);
    }

    IEnumerator deactivateSelection(SketchObjectSelection selection) {
        yield return new WaitForSeconds(3);
        selection.Deactivate();
    }

    private void lineSketchObjectTest() {

        foreach (Transform controlPoint in controlPointParent.transform) {
            strokeSketchObject.AddControlPoint(controlPoint.position);
            strokeSketchObject2.AddControlPoint(controlPoint.position);
        }

        strokeSketchObject.SetStrokeCrossSection(CircularCrossSection.GenerateVertices(16), CircularCrossSection.GenerateVertices(16, 1f), .5f);
        //lineSketchObject.setLineDiameter(.7f);
        StartCoroutine(changeDiameter());

        //StartCoroutine(deactivateSelection(selection));
    }

    private void SetAddComparison() {
        List<Vector3> controlPoints = new List<Vector3>();
        strokeSketchObject.SetControlPointsLocalSpace(new List<Vector3>());
        foreach (Transform controlPoint in controlPointParent.transform)
        {
            strokeSketchObject.AddControlPoint(controlPoint.position);
            controlPoints.Add(controlPoint.position);
        }

        strokeSketchObject2.SetControlPointsLocalSpace(controlPoints);
    }

    // Update is called once per frame
    void Update()
    {
        //SetAddComparison();
        if (!ranOnce)
        {
            lineSketchObjectTest();
            ranOnce = true;
        }
    }
}

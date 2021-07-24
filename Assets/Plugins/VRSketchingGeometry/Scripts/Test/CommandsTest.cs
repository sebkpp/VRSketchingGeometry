using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VRSketchingGeometry.SketchObjectManagement;
using VRSketchingGeometry.Commands;
using VRSketchingGeometry.Commands.Stroke;

public class CommandsTest : MonoBehaviour
{
    public GameObject selectionPrefab;
    [FormerlySerializedAs("LineSketchObjectPrefab")] public GameObject StrokeSketchObjectPrefab;
    private StrokeSketchObject strokeSketchObject;
    private StrokeSketchObject strokeSketchObject2;
    public SketchWorld sketchWorld;
    private CommandInvoker invoker;

    private bool ranOnce = false;

    // Start is called before the first frame update
    void Start()
    {

        invoker = new CommandInvoker();
        strokeSketchObject = Instantiate(StrokeSketchObjectPrefab).GetComponent<StrokeSketchObject>();
        invoker.ExecuteCommand(new AddObjectToSketchWorldRootCommand(strokeSketchObject, sketchWorld));

        strokeSketchObject2 = Instantiate(StrokeSketchObjectPrefab).GetComponent<StrokeSketchObject>();
    }

    IEnumerator changeDiameter()
    {
        yield return new WaitForSeconds(5);
        strokeSketchObject.SetStrokeDiameter(.1f);
    }

    IEnumerator deactivateSelection(SketchObjectSelection selection)
    {
        yield return new WaitForSeconds(3);
        selection.Deactivate();
    }

    private void lineSketchObjectTest()
    {
        strokeSketchObject.AddControlPoint(new Vector3(-2, 1, 0));
        strokeSketchObject.AddControlPoint(Vector3.one);
        strokeSketchObject.AddControlPoint(new Vector3(2, 2, 0));
        strokeSketchObject.AddControlPoint(new Vector3(2, 1, 0));

        strokeSketchObject.SetStrokeDiameter(.7f);

        //StartCoroutine(changeDiameter());

        strokeSketchObject2.AddControlPoint(new Vector3(1, 0, 0));
        strokeSketchObject2.AddControlPoint(new Vector3(2, 1, 1));
        strokeSketchObject2.AddControlPoint(new Vector3(3, 2, 0));
        strokeSketchObject2.AddControlPoint(new Vector3(3, 1, 0));

        //GameObject selectionGO = new GameObject("sketchObjectSelection", typeof(SketchObjectSelection));
        GameObject selectionGO = Instantiate(selectionPrefab);
        GameObject groupGO = new GameObject("sketchObjectGroup", typeof(SketchObjectGroup));
        SketchObjectSelection selection = selectionGO.GetComponent<SketchObjectSelection>();
        selection.AddToSelection(strokeSketchObject);
        selection.AddToSelection(strokeSketchObject2);
        selection.Activate();
        StartCoroutine(deactivateSelection(selection));
    }

    private void commandsTest() {

        //CommandInvoker invoker = new CommandInvoker();
        invoker.ExecuteCommand(new AddControlPointCommand(strokeSketchObject, new Vector3(-2, 1, 0)));
        invoker.ExecuteCommand(new AddControlPointCommand(strokeSketchObject, new Vector3(1, 1, 1)));
        invoker.ExecuteCommand(new AddControlPointCommand(strokeSketchObject, new Vector3(2, 2, 0)));
        invoker.ExecuteCommand(new AddControlPointCommand(strokeSketchObject, new Vector3(2, 3, 0)));
        invoker.ExecuteCommand(new DeleteControlPointCommand(strokeSketchObject));

        invoker.Undo();
        invoker.Undo();
        invoker.Undo();
        invoker.Undo();
        invoker.Undo();
        invoker.Undo();

        invoker.Redo();
        invoker.Redo();
        invoker.Redo();
        invoker.Redo();
        invoker.Redo();
        invoker.Redo();

        invoker.ExecuteCommand(new DeleteObjectCommand(strokeSketchObject, sketchWorld));

        invoker.Undo();
        invoker.Redo();
        //invoker.Undo();


        //invoker.ExecuteCommand(new AddControlPointCommand(lineSketchObject, new Vector3(2, 3, 0)));
        //invoker.Redo();

        //invoker.Undo();
        //invoker.Undo();
        //invoker.Undo();
        //invoker.Redo();
        //invoker.Redo();
        //invoker.Redo();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ranOnce)
        {
            ranOnce = true;
            //lineSketchObjectTest();
            commandsTest();
        }
    }
}

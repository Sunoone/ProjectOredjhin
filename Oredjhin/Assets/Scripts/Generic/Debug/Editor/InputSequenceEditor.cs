using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputSequenceEditor : EditorWindow
{
    public ScriptableObject referenceFile; // Placeholder for eventual data file.
    // The data file should also get a button to open this sequencer. In general, make a library for this.
    float width, height;

    List<Rect> windows = new List<Rect>();

    [MenuItem("FG/Inputs/Sequencer")]
    static void ShowEditor()
    {
        InputSequenceEditor editor = EditorWindow.GetWindow<InputSequenceEditor>();
        editor.titleContent = new GUIContent("Placeholder Title");
        editor.Init();
    }

    public void Init()
    {
        minSize = new Vector2(800, 800);
        windows.Add(new Rect(10, 10, 100, 100));
    }
    public void SetValuesForReference(ScriptableObject refObject)
    {

    }

    void OnGUI()
    {
        if (referenceFile == null)
            Init();


        /*DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows
        DrawNodeCurve(window2, window3); // Here the curve is drawn under the windows

        BeginWindows();
        window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
        window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
        window3 = GUI.Window(3, window3, DrawNodeWindow, "Window 3");
        EndWindows();*/
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
        //GUI.DragWindow()
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}

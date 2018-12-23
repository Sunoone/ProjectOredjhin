using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Freethware.Tools
{

    // Original source: http://gram.gs/gramlog/creating-node-based-editor-unity/
    public class NodeBasedEditor : EditorWindow
    {
        private List<Node> nodes;
        private List<Connection> connections;
        private List<ConnectionPoint> selections;

        private float menuBarHeight = 220f;
        private Rect menuBar;

        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private GUIStyle inPointStyle;
        private GUIStyle outPointStyle;
        private GUIStyle resizerStyle;

        private Vector2 offset;
        private Vector2 drag;

        private int border = 12;
        private Vector2 nodeSize = new Vector2(200, 50);

        
        [MenuItem("Window/Base Node Based Editor")]
        private static void OpenWindow()
        {
            NodeBasedEditor window = GetWindow<NodeBasedEditor>();
            window.titleContent = new GUIContent("Base Node Based Editor Window");
        }

        private void OnEnable()
        {
            CreateNodeStyle();
            CreateSelectedNodeStyle();
            CreateInPointStyle();
            CreateOutPointStyle();
            CreateResizerStyle();
        }

        protected virtual void CreateNodeStyle()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(border, border, border, border);
        }
        protected virtual void CreateSelectedNodeStyle()
        {
            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(border, border, border, border);
        }
        protected virtual void CreateInPointStyle()
        {
            inPointStyle = new GUIStyle();
            inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            inPointStyle.border = new RectOffset(Mathf.RoundToInt(border / 3), Mathf.RoundToInt(border / 3), border, border);
        }
        protected virtual void CreateOutPointStyle()
        {
            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            outPointStyle.border = new RectOffset(Mathf.RoundToInt(border / 3), Mathf.RoundToInt(border / 3), border, border);
        }
        protected virtual void CreateResizerStyle()
        {
            resizerStyle = new GUIStyle();
            //resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_icon dropdown.png") as Texture2D;
            //resizerStyle.active.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;
        }

        private void OnGUI()
        {
            DrawGrid(20, .2f, Color.gray);
            DrawGrid(100, .4f, Color.gray);
            DrawMenuBar();
            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed)
                Repaint();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * .5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            int length = widthDivs;
            for (int  i = 0; i < length; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }
            length = heightDivs;
            for (int i = 0; i < length; i++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * i, 0) + newOffset, new Vector3(position.width, gridSpacing * i, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawMenuBar()
        {
            menuBar = new Rect(0, 0, position.width, menuBarHeight);

            GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton, GUILayout.Width(35));
            GUILayout.Space(5);
            GUILayout.Button(new GUIContent("Load"), EditorStyles.toolbarButton, GUILayout.Width(35));

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                int length = nodes.Count;
                for (int i = 0; i < length; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                int length = connections.Count;
                for (int i = 0; i < length; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        private void DrawConnectionLine(Event e)
        {
            if (selections != null && selections.Count == 1 && selections[0] != null)
            {
                Handles.DrawBezier(
                    selections[0].Rect.center,
                    e.mousePosition,
                    selections[0].Rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 2 && !IsLocked)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;
            if (nodes != null)
            {
                int length = nodes.Count;
                for (int i = 0; i < length; i++)
                {
                    nodes[i].Drag(delta);
                }
            }
            GUI.changed = true;
        }
        private void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();
                int length = connections.Count;
                for (int i = 0; i < length; i++)
                {
                    Debug.Log("Connection In: " + connections[i].InPoint.GetHashCode() + ", Node In: " + node.InPoint.GetHashCode());
                    Debug.Log("Connection Out: " + connections[i].OutPoint.GetHashCode() + ", Node Out: " + node.OutPoint.GetHashCode());
                    if (connections[i].InPoint == node.InPoint || connections[i].OutPoint == node.OutPoint)
                    {
                        connectionsToRemove.Add(connections[i]);
                        Debug.Log("1");
                    }
                }
                length = connectionsToRemove.Count;
                for (int i = 0; i < length; i++)
                {
                    RemoveConnection(connectionsToRemove[i]);
                    Debug.Log("2");
                }
            }
            if (selections == null)
                selections = new List<ConnectionPoint>();
            if (selections.Count >= 1)
            {
                if (node.InPoint == selections[0])
                    selections.RemoveAt(0);
                else if (node.OutPoint == selections[0])
                    selections.RemoveAt(0);
            }
            if (selections.Count >= 2)
            {
                if (node.InPoint == selections[1])
                    selections.RemoveAt(1);
                else if (node.OutPoint == selections[1])
                    selections.RemoveAt(1);
            }
            nodes.Remove(node);
        }
        private bool IsLocked;
        public void OnLockPosition(bool state)
        {
            IsLocked = state;
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e, out IsLocked);
                    if (guiChanged || IsLocked)
                        GUI.changed = true;
                }
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            ProcessContextMenu(mousePosition, null);
        }
        private void ProcessContextMenu(Vector2 mousePosition, GenericMenu genericMenu)
        {
            if (genericMenu == null)
                genericMenu = new GenericMenu();
            if (selections != null && selections.Count > 0)
                genericMenu.AddItem(new GUIContent("Deselect connection point"), false, () => ClearConnectionSelection());
            genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            if (nodes == null)
            {
                nodes = new List<Node>();
            }
            nodes.Add(new Node(mousePosition, nodeSize.x, nodeSize.y, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, resizerStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnLockPosition, ProcessContextMenu));
        }
        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            if (selections == null)
                selections = new List<ConnectionPoint>();

            if (selections.Count == 0)
                selections.Add(inPoint);
            else if (selections[0].Type == ConnectionPointType.Out)
            {
                selections.Add(inPoint);
                SolveClickSecondPoint();
            }
            else
                Debug.LogWarning("Selections are not compatible.");
        }
        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            if (selections == null)
                selections = new List<ConnectionPoint>();

            if (selections.Count == 0)
                selections.Add(outPoint);
            else if (selections[0].Type == ConnectionPointType.In)
            {
                selections.Add(outPoint);
                SolveClickSecondPoint();
            }
            else
                Debug.LogWarning("Selections are not compatible.");
        }
        private void OnClickRemoveConnection(Connection connection) { RemoveConnection(connection); }

        private void SolveClickSecondPoint()
        {
            if (selections[0].Node != selections[1].Node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }

        private void CreateConnection()
        {
            if(connections == null)
            {
                connections = new List<Connection>();         
            }

            if (selections[0].Type == ConnectionPointType.In)
                PointConnecter(selections[0], selections[1]);
            else
                PointConnecter(selections[1], selections[0]);
        }
        private void PointConnecter(ConnectionPoint point1, ConnectionPoint point2)
        {
            if (Connection.Connect(point1, point2))
                connections.Add(new Connection(point1, point2, OnClickRemoveConnection));
            else
            {
                int length = connections.Count;
                for (int i = 0; i < length; i++)
                {
                    if (connections[i].InPoint == point1)
                    {
                        RemoveConnection(connections[i]);
                        Connection.Connect(point1, point2);
                        connections.Add(new Connection(point1, point2, OnClickRemoveConnection));
                    }
                    else if (connections[i].OutPoint == point2)
                    {
                        RemoveConnection(connections[i]);
                        Connection.Connect(point1, point2);
                        connections.Add(new Connection(point1, point2, OnClickRemoveConnection));
                    }
                    // (connections[i].InPoint == )
                }
                Debug.LogError("Connection limit reached for selected connections.");
            }
        }
        public void RemoveConnection(Connection connection)
        {
            connection.Disconnect();
            connections.Remove(connection);
        }
        private void ClearConnectionSelection()
        {
            selections.Clear();
        }

 
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Xml.Serialization;

namespace Freethware.Tools
{
    // Original source: http://gram.gs/gramlog/creating-node-based-editor-unity/
    public enum ChangedState
    {

    }
        // Should be made resizable.
    public class Node {

        [XmlIgnore] public bool IsDragged;
        [XmlIgnore] public bool IsSelected;

        [XmlIgnore] public ConnectionPoint InPoint;
        [XmlIgnore] public ConnectionPoint OutPoint;
        [XmlIgnore] public Resizer Resizer;

        public Rect Rect;
        public Vector2 MinSize = new Vector2(100, 50);
        [XmlIgnore] public float ResizerSize = 10f;
        [XmlIgnore] private float border = 10f;

        [XmlIgnore] public string Title;
        [XmlIgnore] public GUIStyle Style;
        [XmlIgnore] public GUIStyle DefaultNodeStyle;
        [XmlIgnore] public GUIStyle SelectedNodeStyle;
        [XmlIgnore] public GUIStyle resizerStyle;

        [XmlIgnore] public Action<Node> OnRemoveNode;
        [XmlIgnore] public Action<Vector2, GenericMenu> OnRightClick;

        public Node() { }

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedNodeStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, GUIStyle resizerStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> onClickRemoveNode, Action<bool> onResize, Action<Vector2, GenericMenu> processContextMenu)
        {
            Rect = new Rect(position.x, position.y, width, height);
            //Area = new Rect(position.x, position.y, width - border, height - border);
            Style = nodeStyle;
            DefaultNodeStyle = nodeStyle;
            SelectedNodeStyle = selectedNodeStyle;
            InPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
            OutPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
            Resizer = new Resizer(this, resizerStyle, onResize);
            OnRemoveNode = onClickRemoveNode;
            OnRightClick = processContextMenu;
            CalcMinSize();
        }

        private void Save()
        {
            //XMLOp.Serialize
        }

        protected virtual void CalcMinSize()
        {
           // MinSize = GUIStringFit("Wooohoooo", Style);
            //MinSize.x = minimumField.x;
            //MinSize.y *= (variableAmount * 1.2f) + 40;
            MinSize = new Vector2(100, 50);
        }

        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
            // Might want to add some checks.
        }

        int testInt = 5;

        private Vector2 minimumField = new Vector2(10, 10);
        private int variableAmount = 3;
        public void Draw()
        {
            
            InPoint.Draw();
            OutPoint.Draw();

            //GUI.Box(Rect, Title, Style);

            GUI.Box(Rect, Title, Style);
            GUILayout.BeginArea(new Rect(Rect.x + 10, Rect.y + 10, Rect.width - 20, Rect.height - 20));
            GUIStringFit("Wooohoooo", Style);
            EditorGUILayout.Space();
            testInt = EditorGUILayout.IntField("Text", testInt);
            testInt = EditorGUILayout.IntField("Wooohoooo", testInt);
            testInt = EditorGUILayout.IntField("Uhh, ok", testInt);
           
            //GUILayout.TextField("Wth", 30, Style);
            GUILayout.EndArea();

            Resizer.Draw();
        }

        public static Vector2 GUIStringFit(string text, GUIStyle style)
        {
            Vector2 size = style.CalcSize(new GUIContent(text + " "));
            EditorGUIUtility.labelWidth = size.x;
            return size;
        }

        public void Select()
        {
            IsSelected = true;
            Style = SelectedNodeStyle;
        }
        public void Deselect()
        {
            IsSelected = false;
            Style = DefaultNodeStyle;
        }

        public bool ProcessEvents(Event e, out bool isLocked)
        {
            if (/*InPoint.ProcessEvents(e) ||
                OutPoint.ProcessEvents(e) ||*/
                Resizer.ProcessEvents(e))
            {
                isLocked = true;
                return true;
            }
            else
                isLocked = false;


            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (Rect.Contains(e.mousePosition))
                        {
                            IsDragged = true;
                            GUI.changed = true;
                            Select();
                        }
                        else
                        {
                            GUI.changed = true;
                            Deselect();
                        }
                    }

                    if (e.button == 1 /*&& IsSelected*/ && Rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu(e.mousePosition);
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    IsDragged = false;
                    break;
                case EventType.MouseMove:
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0 && IsDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
                    break;
                case EventType.ScrollWheel:
                    break;
                case EventType.Repaint:
                    break;
                case EventType.Layout:
                    break;
                case EventType.DragUpdated:
                    break;
                case EventType.DragPerform:
                    break;
                case EventType.DragExited:
                    break;
                case EventType.Ignore:
                    break;
                case EventType.Used:
                    break;
                case EventType.ValidateCommand:
                    break;
                case EventType.ExecuteCommand:
                    break;
                case EventType.ContextClick:
                    break;
                case EventType.MouseEnterWindow:
                    break;
                case EventType.MouseLeaveWindow:
                    break;
                default:
                    break;
            }
            
            return false;
        }
        
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            OnRightClick.Invoke(mousePosition, genericMenu);
        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
                OnRemoveNode.Invoke(this);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Freethware.Tools
{
    // Original source: http://gram.gs/gramlog/creating-node-based-editor-unity/

    public delegate void Action<T>(T t);
    public delegate void Action<X, Y>(X mousePosition, Y genericMenu);
    public enum ConnectionPointType { In, Out }

    public abstract class ConnectionPoint<X>
    {
        public X Variable;
    }


    public class ConnectionPoint
    {
        private Vector2 ConnectionPointSize = new Vector2(10f, 20f);
        private float widthMultiplier = .8f;

        public Rect Rect;
        public ConnectionPointType Type;

        public Node Node;

        public GUIStyle Style;

        public Action<ConnectionPoint> OnClickConnectionPoint;

        public int AllowedConnections = -1;
        public int CurrentConnections = 0;
        
        public bool RequestConnection() { return AllowedConnections < 0 || CurrentConnections + 1 <= AllowedConnections; }
        public void RegisterConnection() { CurrentConnections++; }
        public void RegisterDisconnection() { CurrentConnections--; }

        public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> onClickConnectionPoint)//, Action<bool> onLocked)
        {
            Node = node;
            Type = type;
            Style = style;
            OnClickConnectionPoint = onClickConnectionPoint;
            Rect = new Rect(0, 0, ConnectionPointSize.x, ConnectionPointSize.y);
        }

        public void Draw()
        {
            Rect.y = Node.Rect.y + (Node.Rect.height * .5f) - Rect.height * .5f;
            switch (Type)
            {
                case ConnectionPointType.In:
                    Rect.x = Node.Rect.x - Rect.width + ConnectionPointSize.x * widthMultiplier;
                    break;
                case ConnectionPointType.Out:
                    Rect.x = Node.Rect.x + Node.Rect.width - ConnectionPointSize.x * widthMultiplier;
                    break;
                default:
                    break;
            }

            GUI.Box(Rect, "", Style);
            if (GUI.Button(Rect, "", Style))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint.Invoke(this);
                    //OnClickConnectionPoint(this);
                }
                   
            }
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && Rect.Contains(e.mousePosition))
                    {
                        if (OnClickConnectionPoint != null)
                        {
                            OnClickConnectionPoint.Invoke(this);
                            //OnClickConnectionPoint(this);
                        }
                        return true;
                    }
                    break;
                case EventType.MouseUp:
                    break;
                case EventType.MouseMove:
                    break;
                case EventType.MouseDrag:
                    return true;
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
                    Debug.Log("Entering");
                    break;
                case EventType.MouseLeaveWindow:
                    Debug.Log("Leaving");
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}

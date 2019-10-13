using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Freethware.Tools
{
    public enum ResizeDirection
    {
        None,
        UpLeft,
        Up,
        UpRight,
        Left,
        Right,
        DownLeft,
        Down,
        DownRight,
    }
    public class Resizer {

        private Node node;
        private Rect rect;

        private Rect upLeft;
        private Rect up;
        private Rect upRight;

        private Rect left;
        private Rect right;

        private Rect downLeft;
        private Rect down;
        private Rect downRight;

        private bool resizeCheck;

        public float size = 10f;
        public GUIStyle ResizerStyle;
        public Action<bool> OnResize;

        public Resizer(Node node, GUIStyle resizerStyle, Action<bool> onResize)
        {
            this.node = node;
            this.ResizerStyle = resizerStyle;
            OnResize = onResize;
            //resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;
        }
        public void Draw()
        {
            //rect = new Rect(node.Rect.x + node.Rect.width, node.Rect.y + node.Rect.height, size, size);
           
            up = new Rect(node.Rect.x + 10, node.Rect.y, node.Rect.width - 20, 10);
            GUI.Box(up, "", ResizerStyle);
            EditorGUIUtility.AddCursorRect(up, MouseCursor.ResizeVertical);

            down = new Rect(node.Rect.x + 10, node.Rect.y + node.Rect.height - 10, node.Rect.width - 20, 10);
            GUI.Box(down, "", ResizerStyle);
            EditorGUIUtility.AddCursorRect(down, MouseCursor.ResizeVertical);
        }

        ResizeDirection resizeDir;
        private Vector2 mouseStart;
        private Vector2 rectStart;
        private Vector2 oldSize;
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        mouseStart = e.mousePosition;
                        rectStart = node.Rect.position;
                        oldSize = node.Rect.size;
                        if (down.Contains(e.mousePosition))         
                            resizeDir = ResizeDirection.Down;
                        else if (up.Contains(e.mousePosition))
                            resizeDir = ResizeDirection.Up;
                    }
                    break;
                case EventType.MouseUp:
                    resizeDir = ResizeDirection.None;
                    break;
                case EventType.MouseMove:
                    break;
                case EventType.MouseDrag:
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
            Resize(resizeDir, e);
            return resizeDir != ResizeDirection.None;
        }

        private void Resize(ResizeDirection dir, Event e)
        {
            float newHeight;
            switch (dir)
            {
                case ResizeDirection.None:
                    break;
                case ResizeDirection.UpLeft:
                    break;
                case ResizeDirection.Up:
                    newHeight = (mouseStart.y - e.mousePosition.y);

                    if (oldSize.y + newHeight >= node.MinSize.y)
                    {
                        node.Rect.y = rectStart.y - newHeight;
                        node.Rect.height = oldSize.y + newHeight;
                    }
                    else
                    {
                        node.Rect.height = node.MinSize.y;
                    }
  
                    break;
                case ResizeDirection.UpRight:
                    break;
                case ResizeDirection.Left:
                    break;
                case ResizeDirection.Right:
                    break;
                case ResizeDirection.DownLeft:
                    break;
                case ResizeDirection.Down:
                    newHeight = oldSize.y + (e.mousePosition.y - mouseStart.y);
                    if (newHeight >= node.MinSize.y)
                    {
                        node.Rect.height = newHeight;
                    }
                    else
                    {
                        
                    }
                    break;
                case ResizeDirection.DownRight:
                    break;
                default:
                    break;
            }

                /*float newWidth = oldSize.x + (e.mousePosition.x - origin.x);
                if (newWidth >= node.MinSize.x)
                    node.Rect.width = newWidth;
                else
                    node.Rect.width = node.MinSize.x;*/
            
        }
    }
}

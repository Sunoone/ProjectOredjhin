using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Freethware.Tools
{
    // Original source: http://gram.gs/gramlog/creating-node-based-editor-unity/

    public class Connection
    {
        public ConnectionPoint InPoint;
        public ConnectionPoint OutPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection() { }

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> onClickRemoveConnection)
        {
            InPoint = inPoint;
            OutPoint = outPoint;
            OnClickRemoveConnection = onClickRemoveConnection;
        }

        public static bool Connect(ConnectionPoint point1, ConnectionPoint point2)
        {
            if (point1.RequestConnection() && point2.RequestConnection())
            {
                point1.RegisterConnection();
                point2.RegisterConnection();
                return true;
            }
            return false;
        }
        public void Disconnect()
        {
            InPoint.RegisterDisconnection();
            OutPoint.RegisterDisconnection();
        }

        public void Draw()
        {
            Handles.DrawBezier(
                InPoint.Rect.center,
                OutPoint.Rect.center,
                InPoint.Rect.center + Vector2.left * 50f,
                OutPoint.Rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f);

            if (Handles.Button((InPoint.Rect.center + OutPoint.Rect.center) * .5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection.Invoke(this);
                    //OnClickRemoveConnection(this);
                }
            }
        }
    }
}

using UnityEditor;

namespace EasyButtons
{
    /// <summary>
    /// Custom inspector for Object including derived classes.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class EasyButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawEasyButtons();

            // Draw the rest of the inspector as usual
            DrawDefaultInspector();
        }
    }
}
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "InputUnit", menuName = "Quests/Component/InputUnit", order = 1)]
public class InputUnit : ScriptableObject
{
    public string Description;

    public virtual float GetFloatValue() { return -1f; }
}



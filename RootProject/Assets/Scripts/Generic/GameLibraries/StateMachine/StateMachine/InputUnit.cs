using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "InputUnit", menuName = "Quests/Component/InputUnit", order = 1)]
    public class InputUnit : ScriptableObject
    {
        public string Description;
    }
}

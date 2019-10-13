using UnityEngine;

namespace Freethware.Inputs
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DirectionUnit", menuName = "Controls/DirectionUnit", order = 1)]
    public class Controls_DirectionUnit : Controls_InputUnit
    {
        public Direction Direction;
    }
}

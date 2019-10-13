using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Performer
{
    public abstract class Performer : MonoBehaviour, IPerformer
    {
        public virtual void LookTowards(Entity pos) { }
        public virtual void LookTowards(Vector3 target) { }

        public virtual void TurnTowards(Entity pos) { }
        public virtual void TurnTowards(Vector3 target) { }

        public virtual void Interact(Entity pos) { }     
        public virtual void Interact() { }
    }

    public interface IPerformer 
    {
        void LookTowards(Entity user);
        void TurnTowards(Entity user);
        void Interact(Entity user);
    }

    public enum Performance
    {
        LookTowards,
        TurnAround,
        Interact
    }
}

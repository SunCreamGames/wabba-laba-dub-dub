using UnityEngine;

public abstract class State
{
    public virtual Vector3 CalculateMovementVector(Transform[] others, Transform catchPlayer, Transform me)
    {
        return Vector3.zero;
    }
}
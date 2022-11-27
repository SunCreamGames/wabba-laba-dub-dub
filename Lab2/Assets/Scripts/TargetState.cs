using UnityEngine;

public class TargetState : State
{
    public override Vector3 CalculateMovementVector(Transform[] others, Transform catchPlayer, Transform me)
    {
        return (me.position - catchPlayer.position).normalized;
    }
}
using UnityEngine;

public class WanderState : State
{
    public override Vector3 CalculateMovementVector(Transform[] others, Transform catchPlayer, Transform me)
    {
        var x = Random.Range(-1f, 1f);
        var z = Random.Range(-1f, 1f);

        return new Vector3(x, 0, z).normalized;
    }
}
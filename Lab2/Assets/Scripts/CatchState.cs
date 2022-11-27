using UnityEngine;

public class CatchState : State
{
    public override Vector3 CalculateMovementVector(Transform[] others, Transform catchPlayer, Transform me)
    {
        var dist = float.MaxValue;
        Transform target = me;
        foreach (var other in others)
        {
            if (other.GetComponent<Bot>() != null && other.GetComponent<Bot>().isProtected
                || other.GetComponent<Player>() != null && other.GetComponent<Player>().isProtected)
            {
                continue;
            }

            var newDist = Vector3.Distance(other.position, me.position);
            if (newDist < dist)
            {
                dist = newDist;
                target = other;
            }
        }

        return (target.position - me.position).normalized;
    }
}
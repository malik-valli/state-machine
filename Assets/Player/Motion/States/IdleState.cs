using UnityEngine;

public class IdleState : GroundState
{
    public override void Enter() =>
        Speed = 0;

    public override void Tick() { }

    public override void FixedTick() =>
        base.PinDown();

    public override void Exit() =>
        InertiaVector = Vector3.zero;
}

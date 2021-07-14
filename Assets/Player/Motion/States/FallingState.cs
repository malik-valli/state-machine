using System;
using UnityEngine;

[Serializable]
public class FallingState : AirState
{
    [SerializeField, Range(0, 1)] private float effectOfInertia = 0.5f;

    public override void Enter() { }

    public override void Tick() { }

    public override void FixedTick()
    {
        VerticalSpeed -= Mathf.Abs(Physics.gravity.y) * Time.fixedDeltaTime;

        PlayerMotion.Instance.AddPhysicalMotion(Vector3.up * VerticalSpeed + GroundState.InertiaVector * effectOfInertia);
    }

    public override void Exit() =>
        VerticalSpeed = 0;
}

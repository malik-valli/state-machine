using System;
using UnityEngine;

[Serializable]
public class JumpingState : AirState
{
    [SerializeField] private float jumpHeight = 1;
    [SerializeField, Range(0, 1)] private float effectOfInertia = 1f;

    public override void Enter() =>
        VerticalSpeed = Mathf.Sqrt(jumpHeight * 2 * Mathf.Abs(Physics.gravity.y));

    public override void Tick() { }

    public override void FixedTick()
    {
        PlayerMotion.Instance.AddPhysicalMotion(Vector3.up * VerticalSpeed + GroundState.InertiaVector * effectOfInertia);
        
        GravityAttraction();
    }

    private void GravityAttraction() =>
        VerticalSpeed -= Mathf.Abs(Physics.gravity.y) * Time.fixedDeltaTime;

    public override void Exit() { }
}

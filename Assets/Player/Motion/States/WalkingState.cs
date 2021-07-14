using System;
using UnityEngine;

[Serializable]
public class WalkingState : GroundState
{
    private CharacterController characterController;

    [SerializeField]
    private float walkingSpeed = 2;

    public override void Enter()
    {
        characterController = PlayerMotion.Instance.CharacterController;
        Speed = walkingSpeed;
    }

    public override void Tick()
    {
        PlayerMotion.Instance.AddMotion((characterController.transform.right * PlayerControl.Instance.HorizontalAxis +
            characterController.transform.forward * PlayerControl.Instance.VerticalAxis) * Speed);
    }

    public override void FixedTick() =>
        base.PinDown();

    public override void Exit()
    {
        InertiaVector = (characterController.transform.right * PlayerControl.Instance.HorizontalAxis +
            characterController.transform.forward * PlayerControl.Instance.VerticalAxis) * Speed;
    }
}

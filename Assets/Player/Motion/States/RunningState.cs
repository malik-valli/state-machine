using System;
using UnityEngine;

[Serializable]
public class RunningState : GroundState
{
    private CharacterController characterController;

    [SerializeField]
    private float runningSpeed = 8;

    public override void Enter()
    {
        characterController = PlayerMotion.Instance.CharacterController;
        Speed = runningSpeed;
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

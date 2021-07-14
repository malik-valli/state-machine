using UnityEngine;

public abstract class GroundState : State
{
    public static float Speed { get; protected set; }
    public static Vector3 InertiaVector { get; protected set; }

    public override abstract void Enter();

    public override abstract void Tick();

    /// <summary> Pins the player to the ground using gravity. 
    /// DON'T put it in regular Tick(). Put it in FixedTick() </summary>
    protected void PinDown() =>
        PlayerMotion.Instance.AddPhysicalMotion(Vector3.down * Mathf.Abs(Physics.gravity.y));

    public override abstract void Exit();
}
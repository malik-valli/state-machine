public abstract class AirState : State
{
    public static float VerticalSpeed { get; protected set; }

    public override abstract void Enter();

    public override abstract void Tick();

    public override abstract void Exit();
}
public abstract class State
{
    public abstract void Enter();

    public abstract void Tick();
    public virtual void FixedTick() { }

    public abstract void Exit();
}

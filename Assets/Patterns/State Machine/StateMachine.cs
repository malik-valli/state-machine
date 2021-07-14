using System;
using System.Collections.Generic;

public class StateMachine
{
    public State CurrentState { get; private set; }
    public State PreviousState { get; private set; }

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();

    private readonly List<Transition> EmptyTransitions = new List<Transition>(0);

    private void AnyTick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);
    }

    /// <summary> Put it in Update(). </summary>
    public void Tick()
    {
        AnyTick();
        CurrentState?.Tick();
    }

    /// <summary> Put it in FixedUpdate(). DON'T put it in Update()! </summary>
    public void FixedTick()
    {
        AnyTick();
        CurrentState?.FixedTick();
    }

    public void SetState(State newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState?.Exit();
        PreviousState = CurrentState;
        
        CurrentState = newState;

        transitions.TryGetValue(CurrentState.GetType(), out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = EmptyTransitions;

        CurrentState.Enter();
    }

    public void AddTransition(State from, State to, Func<bool> predicate)
    {
        if (this.transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            this.transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
        if (from == CurrentState) currentTransitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(State state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }

    private class Transition
    {
        public Func<bool> Condition { get; }
        public State To { get; }

        public Transition(State to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}

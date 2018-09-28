using System.Collections.Generic;

/// <summary>
/// Manager class for states
/// </summary>
public class StateManager
{
    /// <summary>List with all current states in the order they were started</summary>
    protected Stack<State> runningStates;

    /// <summary>
    /// Gets the currently running state.
    /// </summary>
    public State RunningState
    {
        get { return runningStates.Peek(); }
    }

    /// <summary>
    /// Creates a new instance of the manager
    /// </summary>
    public StateManager()
    {
        // create the stack
        runningStates = new Stack<State>();
    }

    /// <summary>
    /// Switches from one state to another.
    /// </summary>
    /// <param name="state">The state that shall exchange the current active state</param>
    public virtual void ChangeState(State state, bool pushNewState, bool killAllStates)
    {
        if (state == null)
            return;

        // exit current state
        if (runningStates.Count > 0)
        {
            runningStates.Peek().Exit();
            runningStates.Pop();

            // kill all other states if desired
            if (killAllStates)
            {
                while (runningStates.Count > 0)
                {
                    runningStates.Peek().Exit();
                    runningStates.Pop();
                }
            }
        }

        // enter the new state
        state.Enter();

        // push it onto the stack if desired
        if (pushNewState)
            runningStates.Push(state);
    }

    public virtual void ChangeState(State state)
    {
        ChangeState(state, true, false);
    }

    /// <summary>
    /// Pauses the currently active state and starts a new one.
    /// </summary>
    /// <param name="state">The state that shall interrupt the current active state</param>
    public virtual void InterruptCurrentState(State state)
    {
        if (state == null)
            return;

        // pause current state
        if (runningStates.Count > 0)
            runningStates.Peek().Pause();

        // enter the new state and push it onto the stack
        state.Enter();
        runningStates.Push(state);
    }

    /// <summary>
    /// Continues a state that was formerly interrupted and paused.
    /// </summary>
    public void ContinueInterruptedState()
    {
        // continue interrupted state
        if (runningStates.Count > 0)
            runningStates.Peek().Continue();
    }

    /// <summary>
    /// Exits the current state.
    /// </summary>
    public void ExitCurrentState()
    {
        // exit current state and remove it
        if (runningStates.Count > 0)
        {
            runningStates.Peek().Exit();
            runningStates.Pop();
        }
    }

    /// <summary>
    /// Updates the current state and paused states.
    /// </summary>
    public void Update()
    {
        if (runningStates.Count > 0)
        {
            // update current state
            runningStates.Peek().Update();

            // update paused states
            foreach (var state in runningStates)
                if (state.IsPaused)
                    state.UpdatePaused();
        }
    }
}
using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract base class for all states
/// </summary>
public abstract class State
{
    /// <summary>Reference to the parent state manager</summary>
    protected StateManager stateManager;
    /// <summary>Name of this state</summary>
    protected string name;
    /// <summary>Whether or not the state is currently paused</summary>
    protected bool isPaused = false;

    /// <summary>
    /// Gets the parent state manager.
    /// </summary>
    public StateManager StateManager
    {
        get { return stateManager; }
    }

    /// <summary>
    /// Gets or sets the name of this state.
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    /// <summary>
    /// Gets whether or not the state is currently paused.
    /// </summary>
    public bool IsPaused
    {
        get { return isPaused; }
    }

    /// <summary>
    /// Creates a new instance of the state.
    /// </summary>
    /// <param name="stateManager">The parent state manager</param>
    /// <param name="name">Name of this state</param>
    public State(StateManager stateManager, string name)
    {
        this.stateManager = stateManager;
        this.name = name;
    }

    /// <summary>
    /// Creates a new instance of the state.
    /// </summary>
    /// <param name="stateManager">The parent state manager</param>
    public State(StateManager stateManager)
        : this(stateManager, "")
    { }

    /// <summary>
    /// Updates the state.
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    /// Updates the state, when it is paused.
    /// </summary>
    public virtual void UpdatePaused()
    {
    }

    /// <summary>
    /// Performs actions when the state is started the first time.
    /// </summary>
    public virtual void Enter()
    {
    }

    /// <summary>
    /// Performs actions when the state is exited.
    /// </summary>
    public virtual void Exit()
    {
    }

    /// <summary>
    /// Performs actions to pause the state.
    /// </summary>
    public virtual void Pause()
    {
        isPaused = true;
    }

    /// <summary>
    /// Performs actions when the state is continued.
    /// </summary>
    public virtual void Continue()
    {
        isPaused = false;
    }
}
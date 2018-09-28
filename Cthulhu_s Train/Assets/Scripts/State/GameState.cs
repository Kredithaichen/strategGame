using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Abstract base class for all game states. Handles input without parameters or result.
/// </summary>
public abstract class GameState : State
{
    /// <summary>Name of the level that is loaded on enter</summary>
    protected string levelName;

    protected bool needsLoading;
    private bool isLoading;
    private bool finishedLoading;

    /// <summary>
    /// Gets or sets the name of the level that is loaded on entry.
    /// </summary>
    public string LevelName
    {
        get { return levelName; }
        set { levelName = value; }
    }

    /// <summary>
    /// Gets the parent game state manager.
    /// </summary>
    public new GameStateManager StateManager
    {
        get { return (GameStateManager)stateManager; }
    }

    public bool FinishedLoading
    {
        get { return finishedLoading; }
        set { finishedLoading = value; }
    }

    /// <summary>
    /// Creates a new instance of the game state.
    /// </summary>
    /// <param name="stateManager">Parent game state manager</param>
    /// <param name="name">Name of this state</param>
    public GameState(GameStateManager stateManager, string name, bool needsLoading = false)
        : base(stateManager, name)
    {
        this.needsLoading = needsLoading;
    }

    /// <summary>
    /// Creates a new instance of the game state with a level name.
    /// </summary>
    /// <param name="stateManager">Parent game state manager</param>
    /// <param name="name">Name of this state</param>
    /// <param name="levelName">Name of the level to load on enter</param>
    public GameState(GameStateManager stateManager, string name, string levelName, bool needsLoading = false)
        : this(stateManager, name)
    {
        this.levelName = levelName;
        this.needsLoading = needsLoading;
    }

    /// <summary>
    /// Handles the input that occurs while this state is running.
    /// </summary>
    public virtual void HandleInput()
    {
    }

    /// <summary>
    /// Performs actions when the state is started the first time and loads a level if needed.
    /// </summary>
    public override void Enter()
    {
        // load a level if needed
        if (!string.IsNullOrEmpty(levelName) && SceneManager.GetActiveScene().name != levelName)
        {
            SceneManager.LoadScene("Scenes/" + levelName);
            SceneManager.sceneLoaded += OnSceneWasLoaded;

            if (needsLoading)
                StateManager.LoadState(this);
        }
        else
            // as the state does not need loading, it can be instantly pushed
            StateManager.PushStateOntoStack(this);
    }

    /// <summary>
    /// Is called when the scene is finished with loading to perform dynamic object initialization.
    /// </summary>
    /// <param name="scene">The scene taht was loaded</param>
    /// <param name="mode">The mode the scene was loaded in</param>
    /// <remarks>This method has to be called to remove the delegate!</remarks>
    public virtual void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        // remove the previously added delegate
        SceneManager.sceneLoaded -= OnSceneWasLoaded;

        // as the state is done with loading by now, the state can be put onto the statet stack
        StateManager.PushStateOntoStack(this);

        //finishedLoading = true;
    }

    /// <summary>
    /// Performs actions when the state is continued and loads a level if needed.
    /// </summary>
    public override void Continue()
    {
        base.Continue();

        // load a level if needed and currently a different level is active
        if (!string.IsNullOrEmpty(levelName) && SceneManager.GetActiveScene().name != levelName)
            SceneManager.LoadScene(levelName);
    }

    public virtual void OnLoading()
    {
    }
}

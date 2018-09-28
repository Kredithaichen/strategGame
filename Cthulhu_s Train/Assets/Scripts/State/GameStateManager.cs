
/// <summary>
/// Special manager class for game states.
/// </summary>
public class GameStateManager : StateManager
{
    private GameState loadingState;

    public new GameState RunningState
    {
        get { return runningStates.Peek() as GameState; }
    }

    /// <summary>
    /// Handles the input every frame and sends it to the active state.
    /// </summary>
    public void HandleInput()
    {
        if (runningStates.Count > 0)
            ((GameState)runningStates.Peek()).HandleInput();
    }

    public override void ChangeState(State state)
    {
        base.ChangeState(state, false, false);
    }

    public override void InterruptCurrentState(State state)
    {
        if (state == null)
            return;

        // pause current state
        if (runningStates.Count > 0)
            runningStates.Peek().Pause();

        // enter the new state but do not push it yet
        state.Enter();
    }

    internal void PushStateOntoStack(GameState state)
    {
        if (state == null)
            return;

        runningStates.Push(state);
    }

    public void LoadState(GameState loadState)
    {
        loadingState = loadState;
    }

    public void UpdateLoadingScreen()
    {
        if (loadingState != null)
        {
            loadingState.OnLoading();

            if (loadingState.FinishedLoading)
                loadingState = null;
        }
    }
}

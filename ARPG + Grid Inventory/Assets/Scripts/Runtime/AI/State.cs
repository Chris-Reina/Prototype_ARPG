namespace DoaT.AI
{
    public abstract class State
    {
        protected StateManager _stateManager;
        
        public State(StateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public abstract void Awake();
        public abstract void Execute();
        public abstract void Sleep();
    }
}
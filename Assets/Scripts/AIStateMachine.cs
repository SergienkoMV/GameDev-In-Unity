public class AIStateMachine
{
    public AIState[] States;
    public AIAgent Agent;
    public AIStateID CurrentState;

    public AIStateMachine (AIAgent agent)
    {
        this.Agent = agent;
        int numState = System.Enum.GetNames(typeof(AIStateID)).Length;
        States = new AIState[numState];
    }
    
    public void RegisterState(AIState state)
    {
        int index = (int)state.GetID();
        States[index] = state;
    }

    public AIState GetState(AIStateID stateID)
    {
        int index = (int)stateID;
        return States[index];
    }

    public void Update()
    {
        GetState(CurrentState)?.Update(Agent);
    }

    public void ChangeState(AIStateID newState)
    {
        GetState(CurrentState)?.Exit(Agent);
        CurrentState = newState;
        GetState(CurrentState)?.Enter(Agent);
    }
}

using System.Collections.Generic;
public class BatStates : States<BatCore, BatStates>
{
    Dictionary<State, BaseState<BatCore, BatStates>> _states = new Dictionary<State, BaseState<BatCore, BatStates>>();
    
    enum State
    {
        Chase, Freeze, Idle, 
    }
    public BatStates(BatCore contextCore) : base (contextCore)
    {
        _states[State.Chase] = new BatChaseState(Core, this);
        _states[State.Freeze] = new BatFreezeState(Core, this);
        _states[State.Idle] = new BatIdleState(Core, this);

    }

    public BaseState<BatCore, BatStates> Chase() => _states[State.Chase];
    public BaseState<BatCore, BatStates> Freeze() => _states[State.Freeze];
    public BaseState<BatCore, BatStates> Idle() => _states[State.Idle];

    
}

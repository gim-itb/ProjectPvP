using System.Collections.Generic;
public class BullStates : States<BullCore, BullStates>
{
    Dictionary<State, BaseState<BullCore, BullStates>> _states = new Dictionary<State, BaseState<BullCore, BullStates>>();
    
    enum State
    {
        Chase, Freeze, Idle, 
    }
    public BullStates(BullCore contextCore) : base (contextCore)
    {
        _states[State.Chase] = new BullChaseState(Core, this);
        _states[State.Freeze] = new BullFreezeState(Core, this);
        _states[State.Idle] = new BullIdleState(Core, this);

    }

    public BaseState<BullCore, BullStates> Chase() => _states[State.Chase];
    public BaseState<BullCore, BullStates> Freeze() => _states[State.Freeze];
    public BaseState<BullCore, BullStates> Idle() => _states[State.Idle];

    
}

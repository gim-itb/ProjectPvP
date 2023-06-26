using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BatFreezeState : BaseState<BatCore, BatStates>
{
    public BatFreezeState(BatCore contextCore, BatStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        
    }
    public override void StateFixedUpdate()
    {
        Core.StunTimer -= Time.fixedDeltaTime;
        if(Core.StunTimer <= 0)
        {
            Core.StunTimer = 1;
            Core.UnFreezeSelf();
            SwitchState(States.Idle());
        }
    }

    public override void StateExit()
    {
        
    }
}

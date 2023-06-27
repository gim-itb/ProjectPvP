using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BullIdleState : BaseState<BullCore, BullStates>
{
    public BullIdleState(BullCore contextCore, BullStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        Core.Stop();
    }

    public override void StateUpdate()
    {
        
    }
    public override void StateFixedUpdate()
    {
        Core.Rotation();
        Core.Attack();
        Collider2D col = Physics2D.OverlapCircle(Core.transform.position, Core.DetectRadius, Core.PlayerLayerMask);
        if(col != null && col.CompareTag(Core.PlayerTag))
        {
            Core.ChaseTarget = col.transform;
            SwitchState(States.Chase());
        }
    }

    public override void StateExit()
    {
        
    }
}

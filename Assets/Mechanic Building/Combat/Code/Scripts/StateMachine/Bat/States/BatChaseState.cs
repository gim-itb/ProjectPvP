using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BatChaseState : BaseState<BatCore, BatStates>
{
    public BatChaseState(BatCore contextCore, BatStates States) : base (contextCore, States)
    {
    }

    float _chaseTimer = 1;
    float _chaseDuration = 1;
    public override void StateEnter()
    {
        _chaseTimer = _chaseDuration;
    }

    public override void StateUpdate()
    {
        
    }
    public override void StateFixedUpdate()
    {
        Core.Rotation();
        _chaseTimer -= Time.fixedDeltaTime;
        if(_chaseTimer <= 0)
        {
            Collider2D col = Physics2D.OverlapCircle(Core.transform.position, Core.DetectRadius, Core.PlayerLayerMask);
            if(col != null && col.CompareTag(Core.PlayerTag))
            {
                _chaseTimer = _chaseDuration;
                return;
            }

            Core.ChaseTarget = null;
            SwitchState(States.Idle());
            return;
        }
        if(Core.ChaseTarget == null)
        {
            SwitchState(States.Idle());
            return;
        }
        Core.Move();
    }

    public override void StateExit()
    {
        
    }
}

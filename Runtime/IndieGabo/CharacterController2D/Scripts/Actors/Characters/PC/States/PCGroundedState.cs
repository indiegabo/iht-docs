using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieGabo.FSM;

namespace IndieGabo.CharacterController2D.Actors
{
    public abstract class PCGroundedState : PCState
    {
        protected Func<bool> Still() => () => Actor.grounded && Actor.movementDirection.x == 0;
        protected Func<bool> Running() => () => Actor.grounded && Actor.movementDirection.x != 0;
        protected Func<bool> Dashing() => () => Actor.dash.dashing;
        protected Func<bool> Ascending() => () => Actor.jump.jumping;
        protected Func<bool> Falling() => () => !Actor.grounded && Actor.rb.velocity.y != 0;

        // Called to set the state able to be used by the Machine.
        // A good place to define Transitions, the state name and etc.
        public void OnLoad()
        {
            AddTransition(Ascending(), GetComponent<PCAscendingState>(), 1);
            AddTransition(Falling(), GetComponent<PCFallingState>(), 1);
            AddTransition(Dashing(), GetComponent<PCDashingState>(), 1);
            AddTransition(Still(), GetComponent<PCIdleState>());
            AddTransition(Running(), GetComponent<PCRunningState>());
        }

        // What to do when Actor's enters this state
        public void OnEnter()
        {
            Actor.spriteRenderer.color = Color.blue;
        }
    }
}

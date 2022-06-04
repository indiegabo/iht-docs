using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieGabo.FSM;

namespace IndieGabo.CharacterController2D.Actors
{
    public abstract class PCOnAirState : PCState
    {
        Func<bool> Ascending() => () => Actor.jump.jumping;
        Func<bool> Dashing() => () => Actor.dash.dashing;
        Func<bool> Falling() => () => !Actor.grounded && Actor.rb.velocity.y != 0 && !Actor.jump.jumping;
        Func<bool> Grounded() => () => Actor.grounded;

        // Called to set the state able to be used by the Machine.
        // A good place to define Transitions, the state name and etc.
        public void OnLoad()
        {
            AddTransition(Ascending(), GetComponent<PCAscendingState>());
            AddTransition(Dashing(), GetComponent<PCDashingState>());
            AddTransition(Falling(), GetComponent<PCFallingState>());
            AddTransition(Grounded(), GetComponent<PCIdleState>());
        }

        // What to do when Actor's enters this state
        public void OnEnter()
        {
            Actor.spriteRenderer.color = Color.red;
        }
    }
}

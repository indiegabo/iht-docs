using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieGabo.FSM;

namespace IndieGabo.CharacterController2D.Actors
{

    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/States/PCEvaluationState")]
    public class PCEvaluationState : PCState
    {

        Func<bool> Grounded() => () => Actor.grounded;
        Func<bool> Falling() => () => !Actor.grounded && Actor.rb.velocity.y < 0;

        // Called to set the state able to be used by the Machine.
        // A good place to define Transitions, the state name and etc.
        public void OnLoad()
        {
            AddTransition(Grounded(), GetComponent<PCIdleState>());
            AddTransition(Falling(), GetComponent<PCFallingState>());
        }
    }
}

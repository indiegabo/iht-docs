using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieGabo.CharacterController2D.Data;

namespace IndieGabo.CharacterController2D.Actors
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/States/PCDashingState")]
    public class PCDashingState : PCState
    {

        // What to do when Actor's enters this state
        public void OnEnter()
        {
            Actor.spriteRenderer.color = Color.green;
            Actor.dash.SetUpDash(Actor.flip.currentDirection);
        }

        /// Called upon every machine's FixedTick()
        public void FixedTick()
        {
            if (!Actor.dash.dashing) { machine.ChangeState(machine.DefaultState); }
            SlopeData slopeData = Actor.slopeData;
            if (!slopeData.onSlope)
            {
                Actor.dash.ApplyDash();
            }
            else
            {
                Actor.dash.ApplyDash(slopeData);
            }
        }
    }
}

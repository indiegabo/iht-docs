using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieGabo.FSM;

namespace IndieGabo.CharacterController2D.Actors
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/States/PCFallingState")]
    public class PCFallingState : PCOnAirState
    {

        /// Called upon every machine's FixedTick()
        public void FixedTick()
        {
            Actor.flip.EvaluateAndFlip(Actor.movementDirection.x);
            Actor.movement.MoveHorizontally(Actor.movementDirection.x, Actor.slopeData);
        }
    }
}

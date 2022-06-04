using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndieGabo.FSM;

namespace IndieGabo.CharacterController2D.Actors
{
    [RequireComponent(typeof(PCFallingState))]
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/States/PCAscendingState")]
    public class PCAscendingState : PCOnAirState
    {
        #region Editor

        #endregion

        /// Called upon every machine's FixedTick()
        public void FixedTick()
        {
            Actor.flip.EvaluateAndFlip(Actor.movementDirection.x);
            bool ignoreSlopes = true;
            Actor.movement.MoveHorizontally(Actor.movementDirection.x, Actor.slopeData, ignoreSlopes);
            Actor.jump.Ascend();
        }
    }
}

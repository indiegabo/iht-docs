using System;
using UnityEngine;
using static IndieGabo.CharacterController2D.Checkers2D.SlopeChecker2D;

namespace IndieGabo.CharacterController2D.Actors
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/States/PCRunningState")]
    [RequireComponent(typeof(PCIdleState))]
    public class PCRunningState : PCGroundedState
    {
        /// Called upon every machine's FixedTick()
        public void FixedTick()
        {
            Actor.flip.EvaluateAndFlip(Actor.movementDirection.x);
            Actor.movement.MoveHorizontally(Actor.flip.currentDirection, Actor.slopeData);
        }
    }
}

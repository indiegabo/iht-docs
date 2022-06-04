using System;
using UnityEngine;
using static IndieGabo.CharacterController2D.Checkers2D.SlopeChecker2D;

namespace IndieGabo.CharacterController2D.Actors
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/States/PCIdleState")]
    public class PCIdleState : PCGroundedState
    {
        /// Called upon every machine's FixedTick()
        public void FixedTick()
        {
            Actor.movement.MoveHorizontally(0, Actor.flip.currentDirection, Actor.slopeData);
        }
    }
}

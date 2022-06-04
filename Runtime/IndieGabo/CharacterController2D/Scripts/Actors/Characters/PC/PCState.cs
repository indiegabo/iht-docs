using System.Collections;
using System.Collections.Generic;
using IndieGabo.FSM;
using UnityEngine;

namespace IndieGabo.CharacterController2D.Actors
{
    public abstract class PCState : State
    {
        protected PCActor Actor => actor as PCActor;
    }
}

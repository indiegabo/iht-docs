using System.Collections;
using System.Collections.Generic;

using NaughtyAttributes;
using UnityEngine;

namespace IndieGabo.FSM
{
    /// <summary>
    /// This class represents an actor, being an actor anything wich perform different actions
    /// and wants to benefit from the FSM
    /// </summary>
    [DefaultExecutionOrder(0)]
    public abstract class Actor : MonoBehaviour
    {

        #region Fields

        /// <summary>
        /// A reference to the actor's state machine
        /// </summary>
        protected StateMachine stateMachine;

        #endregion

        #region  Properties

        public StateMachine Machine => stateMachine;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            SetMachine();// Finds the machine among objects components.
        }

        protected virtual void Update()
        {
            stateMachine?.Tick();
        }

        protected virtual void LateUpdate()
        {
            stateMachine?.LateTick();
        }

        protected virtual void FixedUpdate()
        {
            stateMachine?.FixedTick();
        }

        #endregion

        /// <summary>
        /// Sets the state machine for this Actor. 
        /// </summary>
        protected virtual void SetMachine()
        {
            stateMachine = GetComponentInChildren<StateMachine>();

            if (stateMachine != null) { stateMachine.SetUp(this); return; }

            Debug.LogError($"State Machine not found for {GetType()}");
        }
    }
}

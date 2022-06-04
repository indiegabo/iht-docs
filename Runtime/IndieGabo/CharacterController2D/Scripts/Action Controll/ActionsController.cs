using System.Collections;
using System.Collections.Generic;
using IndieGabo.FSM;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IndieGabo.CharacterController2D.ActionControll
{
    /// <summary>
    /// Represents any entity wich can emit actions for whoever wants to listen.
    /// </summary>
    public abstract class ActionsController : MonoBehaviour
    {
        #region Properties

        protected bool canEmitActions = true;

        #endregion

        #region Getters 

        /// <summary>
        /// A lambda check wich determines if actions can be
        /// emitted.
        /// </summary>
        /// <value> true if can emit actions </value>
        public abstract bool CanEmit { get; }

        #endregion

        #region Callbacks 

        public void EnableEmission()
        {
            canEmitActions = true;
        }

        public void DisableEmission()
        {
            canEmitActions = false;
        }

        #endregion

        #region Action Emission

        /// <summary>
        /// Emits an event
        /// </summary>
        /// <param name="unityEvent"> The event </param>
        public virtual void EmitAction(UnityEvent unityEvent)
        {
            if (!CanEmit) return;

            unityEvent.Invoke();
        }

        /// <summary>
        /// Emits a event passing a Vector2 value.
        /// </summary>
        /// <param name="unityEvent"> The Event </param>
        /// <param name="vector">The Vector2 value</param>
        public virtual void EmitAction(UnityEvent<Vector2> unityEvent, Vector2 vector)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(vector);
        }

        /// <summary>
        /// Emits a event passing a float value.
        /// </summary>
        /// <param name="unityEvent"> The Event </param>
        /// <param name="number">The float value</param>
        public virtual void EmitAction(UnityEvent<float> unityEvent, float number)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(number);
        }

        /// <summary>
        /// Emits a event passing a int value.
        /// </summary>
        /// <param name="unityEvent"> The Event </param>
        /// <param name="number">The int value</param>
        public virtual void EmitAction(UnityEvent<int> unityEvent, int number)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(number);
        }

        /// <summary>
        /// Emits a event passing a string value.
        /// </summary>
        /// <param name="unityEvent"> The Event </param>
        /// <param name="text">The string value</param>
        public virtual void EmitAction(UnityEvent<string> unityEvent, string text)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(text);
        }

        /// <summary>
        /// Emits a event passing an IndieGabo.FSM.Actor.
        /// </summary>
        /// <param name="unityEvent"> The event </param>
        /// <param name="actor"> The Actor </param>
        public virtual void EmitAction(UnityEvent<Actor> unityEvent, Actor actor)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(actor);
        }

        /// <summary>
        /// Emits a event passing an IndieGabo.FSM.Actor and a Vector2 value.
        /// </summary>
        /// <param name="unityEvent"> The event </param>
        /// <param name="actor"> The Actor </param>
        /// <param name="vector"> The Vector2 </param>
        public virtual void EmitAction(UnityEvent<Actor, Vector2> unityEvent, Actor actor, Vector2 vector)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(actor, vector);
        }

        /// <summary>
        /// Emits a event passing an IndieGabo.FSM.Actor and a float value.
        /// </summary>
        /// <param name="unityEvent"> The event </param>
        /// <param name="actor"> The Actor </param>
        /// <param name="number"> The float </param>
        public virtual void EmitAction(UnityEvent<Actor, float> unityEvent, Actor actor, float number)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(actor, number);
        }

        /// <summary>
        /// Emits a event passing an IndieGabo.FSM.Actor and a int value.
        /// </summary>
        /// <param name="unityEvent"> The event </param>
        /// <param name="actor"> The Actor </param>
        /// <param name="number"> The int </param>
        public virtual void EmitAction(UnityEvent<Actor, int> unityEvent, Actor actor, int number)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(actor, number);
        }

        /// <summary>
        /// Emits a event passing an IndieGabo.FSM.Actor and a string value.
        /// </summary>
        /// <param name="unityEvent"> The event </param>
        /// <param name="actor"> The Actor </param>
        /// <param name="text"> The string </param>
        public virtual void EmitAction(UnityEvent<Actor, string> unityEvent, Actor actor, string text)
        {
            if (!CanEmit) return;

            unityEvent.Invoke(actor, text);
        }

        #endregion
    }
}

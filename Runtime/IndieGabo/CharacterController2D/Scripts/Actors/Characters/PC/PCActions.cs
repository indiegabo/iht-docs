using System.Collections;
using System.Collections.Generic;
using IndieGabo.CharacterController2D.ActionControll;
using IndieGabo.CharacterController2D.Interfaces;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace IndieGabo.CharacterController2D.Actors
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/PCActions")]
    public class PCActions : InputActionsController, IMovementDirectionUpdater, IJumpHandler, IDashHandler
    {
        #region Properties
        protected bool canEmitJumpActions = true;
        protected bool canEmitMovementDirectionActions = true;
        protected bool canEmitDashActions = true;

        #endregion

        /// <summary>
        /// If actions can be emitted
        /// </summary>
        public override bool CanEmit => canEmitActions;

        #region Events

        /// <summary>
        /// Fired to update movement direction
        /// </summary>
        /// <typeparam name="Vector2"></typeparam>
        /// <returns></returns>
        [Foldout("Available Events")]
        public UnityEvent<Vector2> MovementDirectionUpdateEvent;

        /// <summary>
        /// Fired upon jump input started
        /// </summary>
        /// <returns></returns>
        [Foldout("Available Events")]
        public UnityEvent JumpRequestedEvent;

        /// <summary>
        /// Fired upon jump input canceled
        /// </summary>
        /// <returns></returns>
        [Foldout("Available Events")]
        public UnityEvent JumpCanceledEvent;

        /// <summary>
        /// Fired upon dash input
        /// </summary>
        /// <returns></returns>
        [Foldout("Available Events")]
        public UnityEvent DashRequestedEvent;

        #endregion

        #region Getters

        public UnityEvent<Vector2> MovementDirectionUpdate => MovementDirectionUpdateEvent;
        public UnityEvent JumpRequested => JumpRequestedEvent;
        public UnityEvent JumpCanceled => JumpCanceledEvent;
        public UnityEvent DashRequested => DashRequestedEvent;

        #endregion

        #region Input Callbacks

        /// <summary>
        /// Receives a InputAction.CallbackContext every time Player inputs jump.
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void OnJumpInput(InputAction.CallbackContext ctx)
        {
            if (!CanEmit || !canEmitJumpActions) return;

            if (ctx.performed) { EmitAction(JumpRequestedEvent); }
            if (ctx.canceled) { EmitAction(JumpCanceledEvent); }
        }

        /// <summary>
        /// Receives a InputAction.CallbackContext every time Player inputs movement.
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void OnMovementDirectionInput(InputAction.CallbackContext ctx)
        {
            if (!CanEmit || !canEmitMovementDirectionActions) return;

            Vector2 movementInput = ctx.ReadValue<Vector2>();
            EmitAction(MovementDirectionUpdateEvent, movementInput);
        }

        /// <summary>
        /// Receives a InputAction.CallbackContext every time Player inputs the dash action.
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void OnDashInput(InputAction.CallbackContext ctx)
        {
            if (!CanEmit || !canEmitDashActions) return;

            if (ctx.performed) { EmitAction(DashRequestedEvent); }
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Enables emiting direction actions
        /// </summary>
        public virtual void EnableDirectionActions()
        {
            canEmitMovementDirectionActions = true;
        }

        /// <summary>
        /// Disables emiting direction actions
        /// </summary>
        public virtual void DisableDirectionActions()
        {
            canEmitMovementDirectionActions = false;
        }

        /// <summary>
        /// Enables emiting jump actions
        /// </summary>
        public virtual void EnableJumpActions()
        {
            canEmitJumpActions = true;
        }

        /// <summary>
        /// Disables emiting jump actions
        /// </summary>
        public virtual void DisableJumpActions()
        {
            canEmitJumpActions = false;
            EmitAction(JumpCanceled);
        }

        /// <summary>
        /// Enables emiting jump actions
        /// </summary>
        public virtual void EnableDashActions()
        {
            canEmitDashActions = true;
        }

        /// <summary>
        /// Disables emiting jump actions
        /// </summary>
        public virtual void DisableDashActions()
        {
            canEmitDashActions = false;
        }

        #endregion

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        public string componentName = "IndieGabo's  Playable Character's Actions";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        public string info = "This component centralizes a playable character actions. If something (input system included) wants to fire actions, it should ask this component.";

        [field: SerializeField, ReadOnly, Label("Feed Requirements"), TextArea(1, 30), Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        public string requirements = "Detailed list on Docs (use the OpenDocs button) as it would be too big for this tiny space.";

        public string DocsUrl => "https://docs.com";

        [Button, Tooltip("Opens this component's documentation webpage")]
        public virtual void OpenDocs()
        {
            Application.OpenURL(DocsUrl);
        }

#pragma warning restore 0414
        #endregion
    }
}

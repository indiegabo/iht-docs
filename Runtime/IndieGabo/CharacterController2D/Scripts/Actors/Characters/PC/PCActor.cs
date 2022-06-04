using System;
using System.Collections;
using System.Collections.Generic;
using IndieGabo.CharacterController2D.Abilities2D;
using IndieGabo.CharacterController2D.Interfaces;
using IndieGabo.CharacterController2D.Data;
using IndieGabo.FSM;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace IndieGabo.CharacterController2D.Actors
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(DynamicMovement2D))]
    [RequireComponent(typeof(Jump2D))]
    [RequireComponent(typeof(Dash2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PCActions))]
    [RequireComponent(typeof(DirectionalFlip2D))]
    [AddComponentMenu("IndieGabo/Character Controller 2D/Actors/PC (Playable Characters)/PCActor")]
    public class PCActor : Actor
    {
        #region Editor

        [InfoBox("Mark these if you guarantee this GameObject have components that will feed this component's with required info", EInfoBoxType.Warning)]

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IMovementDirectionUpdater you can mark this and it will subscribe to its events. SlopeChecker2D implements it.")]
        [SerializeField] protected bool seekMovementDirectionUpdater = false;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IGroundedUpdater you can mark this and it will subscribe to its events. GroundedChecker2D implements it.")]
        [SerializeField] protected bool seekGroundedUpdater = false;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an ISlopeUpdater you can mark this and it will subscribe to its events. SlopeChecker2D implements it.")]
        [SerializeField] protected bool seekSlopeUpdater = false;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IWallHitUpdater you can mark this and it will subscribe to its events. WallHitChecker2D implements it.")]
        [SerializeField] protected bool seekWallHitUpdater = false;

        #endregion

        #region Components

        // Natural components
        public SpriteRenderer spriteRenderer { get; protected set; }
        public Rigidbody2D rb { get; protected set; }
        public DynamicMovement2D movement { get; protected set; }
        public Jump2D jump { get; protected set; }
        public Dash2D dash { get; protected set; }
        public PCActions actions { get; protected set; }
        public DirectionalFlip2D flip { get; protected set; }

        // Interfaces

        protected IMovementDirectionUpdater movementDirectionUpdater;
        protected IGroundedUpdater groundedUpdater;
        protected ISlopeUpdater slopeUpdater;
        protected IWallHitUpdater wallHitUpdater;

        #endregion

        #region Properties

        public Vector2 movementDirection { get; protected set; } = Vector2.zero;
        public bool grounded { get; protected set; } = false;
        public SlopeData slopeData { get; protected set; }
        public WallHitData wallHitData { get; protected set; }

        #endregion

        #region  Mono
        protected override void Awake()
        {
            base.Awake();

            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<DynamicMovement2D>();
            jump = GetComponent<Jump2D>();
            dash = GetComponent<Dash2D>();
            actions = GetComponent<PCActions>();
            flip = GetComponent<DirectionalFlip2D>();

            stateMachine.SetDefaultState<PCEvaluationState>();
        }

        protected virtual void Start()
        {
            SubscribeToUpdates();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected virtual void OnEnable()
        {
            SubscribeToUpdates();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromUpdates();
        }

        #endregion

        #region Callbacks and updates

        public virtual void UpdateMovementDirection(Vector2 newMovementDirection)
        {
            movementDirection = newMovementDirection;
        }

        public virtual void UpdateGrounding(bool newGrounding)
        {
            grounded = newGrounding;
        }

        public virtual void UpdateSlopeData(SlopeData newSlopeData)
        {
            slopeData = newSlopeData;
        }

        public virtual void UpdateWallHitData(WallHitData newWallHitData)
        {
            wallHitData = newWallHitData;
        }

        #endregion

        #region Update Seeking

        /// <summary>
        /// Subscribes to events based on components wich implements
        /// the correct interfaces
        /// </summary>
        protected virtual void SubscribeToUpdates()
        {
            UnsubscribeFromUpdates();

            if (seekMovementDirectionUpdater)
            {
                movementDirectionUpdater = GetComponent<IMovementDirectionUpdater>();
                if (movementDirectionUpdater == null)
                    CC2DLog.Warning("Component PCActor might not work properly. It is marked to seek for an IMovementDirectionUpdater but it could not find any.");
                movementDirectionUpdater?.MovementDirectionUpdate.AddListener(UpdateMovementDirection);
            }

            if (seekGroundedUpdater)
            {
                groundedUpdater = GetComponent<IGroundedUpdater>();
                if (groundedUpdater == null)
                    CC2DLog.Warning("Component PCActor might not work properly. It is marked to seek for an IGroundedUpdater but it could not find any.");
                groundedUpdater?.GroundedUpdate.AddListener(UpdateGrounding);
            }

            if (seekSlopeUpdater)
            {
                slopeUpdater = GetComponent<ISlopeUpdater>();
                if (slopeUpdater == null)
                    CC2DLog.Warning("Component PCActor might not work properly. It is marked to seek for an ISlopeUpdater but it could not find any.");
                slopeUpdater?.SlopeDataUpdate.AddListener(UpdateSlopeData);
            }

            if (seekWallHitUpdater)
            {
                wallHitUpdater = GetComponent<IWallHitUpdater>();
                if (wallHitUpdater == null)
                    CC2DLog.Warning("Component PCActor might not work properly. It is marked to seek for an IWallHitUpdater but it could not find any.");
                wallHitUpdater?.WallHitUpdate.AddListener(UpdateWallHitData);
            }
        }

        /// <summary>
        /// Unsubscribes from events
        /// </summary>
        protected virtual void UnsubscribeFromUpdates()
        {
            movementDirectionUpdater?.MovementDirectionUpdate.RemoveListener(UpdateMovementDirection);
            groundedUpdater?.GroundedUpdate.RemoveListener(UpdateGrounding);
            slopeUpdater?.SlopeDataUpdate.RemoveListener(UpdateSlopeData);
            wallHitUpdater?.WallHitUpdate.RemoveListener(UpdateWallHitData);
        }

        #endregion

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        private string componentName = "IndieGabo's Playable Character's Actor";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        private string info = $"This component represents a Playable Character. Find more by reading the docs. You can use the OpenDocs button as a shortcut.";

        [ReadOnly, Label("Feed Requirements"), TextArea(1, 30), SerializeField, Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        private string requirements = "On Update: \n"
                                        + "UpdateMovementDirection(Vector2 newMovementDirection) \n\n"
                                        + "On FixedUpdate: \n"
                                        + "UpdateGronding(bool newGrounding) \n"
                                        + "UpdateSlopeData(SlopeData newSlopeData) \n"
                                        + "UpdateWallHitData(WallHitData newWallHitData) \n";

        private string DocsUrl => "https://docs.com";

        [Button, Tooltip("Opens this component's documentation webpage")]
        public virtual void OpenDocs()
        {
            Application.OpenURL(DocsUrl);
        }

#pragma warning restore 0414
        #endregion

    }
}

using System.Collections;
using System.Collections.Generic;
using IndieGabo.CharacterController2D.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using IndieGabo.CharacterController2D.Data;

namespace IndieGabo.CharacterController2D.Abilities2D
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Abilities 2D/Dash2D")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Dash2D : DynamicMovementHandler2D
    {

        #region Editor

        [Header("Configuration"), Space]
        [Tooltip("The speed wich will be applyed to X axis during dash.")]
        [SerializeField] protected float xSpeed = 20f;

        [Tooltip("The speed wich will be applyed to Y axis during dash.")]
        [SerializeField] protected float ySpeed = 0f;

        [Tooltip("Time in seconds of the dash duration.")]
        [SerializeField] protected float duration = 1f;

        [Tooltip("Minimun time in seconds between dashes.")]
        [SerializeField] protected float delay = 1f;

        [Tooltip("The gravity scale to be apllyed to RigidBody2D during dash.")]
        [SerializeField] protected float gravityScale = 0f;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IDashHandler you can mark this and it will subscribe to its events. PCActions, for example, implements it.")]
        [SerializeField] protected bool seekDashHandler = false;

        #endregion

        #region Components
        protected IGroundedUpdater groundedUpdater;
        protected IDashHandler dashHandler;

        #endregion

        #region Properties

        public bool dashing { get; protected set; } = false;
        protected float dashStartedAt;
        protected bool dashLocked = false;
        protected float currentDashTimer = 0;
        protected float currentDirectionSign = 0;

        #endregion

        #region Getters

        protected bool CanStartDashing => !dashLocked && Time.fixedTime > dashStartedAt + duration + delay;

        #endregion

        #region Mono

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            SubscribeToUpdates();
        }

        protected virtual void FixedUpdate()
        {

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

        #region  Logic

        /// <summary>
        /// Starts the jump process so Ascend can be called each physics frame
        /// </summary>
        protected void StartDash()
        {
            dashing = true;
        }

        /// <summary>
        /// Starts the jump process so Ascend can be called each physics frame
        /// </summary>
        public void SetUpDash(float directionSign)
        {
            currentDirectionSign = directionSign;
            currentDashTimer = 0;
            dashStartedAt = Time.fixedTime;
            rb.velocity = Vector2.zero;
        }

        /// <summary>
        /// Should be called on Fixed (Physics) Update.
        /// </summary>
        public void ApplyDash()
        {
            if (currentDashTimer > duration) { StopDash(); return; }
            MoveHorizontallyApplyingGravity(xSpeed, currentDirectionSign, gravityScale);
            MoveVertically(ySpeed);
            currentDashTimer += Time.fixedDeltaTime;
        }

        public void ApplyDash(SlopeData slopeData)
        {
            if (currentDashTimer > duration) { StopDash(); return; }
            MoveHorizontally(xSpeed, currentDirectionSign, slopeData);
            currentDashTimer += Time.fixedDeltaTime;
        }

        /// <summary>
        /// Stops jump in progress if any.
        /// </summary>
        public void StopDash()
        {
            dashing = false;
            ApplyGravityScale(defaultGravityScale);
        }


        #endregion

        #region Callbacks

        /// <summary>
        /// Call this to request a Jump
        /// </summary>
        public void DashRequested()
        {
            if (!CanStartDashing) return;
            StartDash();
        }

        /// <summary>
        /// Call this in order to Lock jump and
        /// prevent new jumps to occur based on
        /// shouldLock boolean.
        /// </summary>
        /// <param name="shouldLock"></param>
        public void LockDash(bool shouldLock)
        {
            dashLocked = shouldLock;
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

            if (seekDashHandler)
            {
                dashHandler = GetComponent<IDashHandler>();
                if (dashHandler == null)
                    Debug.LogWarning("Component Jump 2D might not work properly. It is marked to seek for an IDashHandler but it could not find any.");
                dashHandler?.DashRequested.AddListener(DashRequested);
            }
        }

        /// <summary>
        /// Unsubscribes from events
        /// </summary>
        protected virtual void UnsubscribeFromUpdates()
        {
            dashHandler?.DashRequested.RemoveListener(DashRequested);
        }

        #endregion

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        public string componentName = "IndieGabo's  Dash 2D";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        public string info = "This component gives a GameObject the ability to dash as long as anything with its reference requests it.";

        [field: SerializeField, ReadOnly, Label("Feed Requirements"), TextArea(1, 30), Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        public string requirements = "On FixedUpdate: \n"
                                        + "UpdateGronding(bool newGrounding) \n\n"
                                        + "On Demand:"
                                        + "DashRequested() \n"
                                        + "StopDash()";

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

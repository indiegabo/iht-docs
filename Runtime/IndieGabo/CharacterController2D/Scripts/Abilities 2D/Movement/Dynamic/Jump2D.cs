using System.Collections;
using System.Collections.Generic;
using IndieGabo.CharacterController2D.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using IndieGabo.CharacterController2D.Data;

namespace IndieGabo.CharacterController2D.Abilities2D
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Abilities 2D/Jump2D")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jump2D : DynamicMovementHandler2D
    {
        #region Editor

        [Header("Debug"), Space]

        [Tooltip("Turn this on and get some visual feedback. Do not forget to turn your Gizmos On")]
        [SerializeField] protected bool debugOn = false;

        [Header("Jump Configuration"), Space]
        [Tooltip("The speed wich will be proportionaly applyed to Y axis.")]
        [SerializeField] protected float force = 100f;
        [Tooltip("Not an actual duration in seconds. Tweek this in order to adjust your jump.")]
        [SerializeField] protected float duration = 1f;
        [Tooltip("The amount of jumps the character can acumulate when grounded")]
        [SerializeField] protected int totalOfJumps = 2;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IGroundedUpdater you can mark this and it will subscribe to its events. GroundedChecker2D implements it.")]
        [SerializeField] protected bool seekGroundedUpdater = false;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an ISlopeUpdater you can mark this and it will subscribe to its events. SlopeChecker2D, for example, implements it.")]
        [SerializeField] protected bool seekSlopeUpdater = false;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IJumpHandler you can mark this and it will subscribe to its events. PCActions, for example, implements it.")]
        [SerializeField] protected bool seekJumpHandler = false;

        #endregion

        #region Components
        protected IGroundedUpdater groundedUpdater;
        protected ISlopeUpdater slopeUpdater;
        protected IJumpHandler jumpHandler;

        #endregion

        #region Properties

        public bool jumping { get; protected set; } = false;
        public bool grounded { get; protected set; } = false;
        protected float jumpStartedAt;
        [ShowIf("debugOn"), SerializeField, ReadOnly]
        protected int jumpsLeft;
        protected float groundedGhostingTime = 0.15f;
        protected bool keepAscending = false;
        protected bool jumpLocked = false;
        protected float currentJumpTimer = 0;

        #endregion

        #region Getters

        protected bool CanStartJumping => jumpsLeft > 0 && !jumping && !jumpLocked;

        #endregion

        #region Mono

        protected override void Awake()
        {
            base.Awake();
            jumpsLeft = totalOfJumps;
            ResetJumpCount();
        }

        protected virtual void Start()
        {
            SubscribeToUpdates();
        }

        protected virtual void FixedUpdate()
        {
            ResetJumpCount();
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
        protected void StartJump()
        {
            jumping = true;
            currentJumpTimer = 0;
            jumpStartedAt = Time.fixedTime;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            jumpsLeft--;
        }

        /// <summary>
        /// Should be called on Fixed (Physics) Update.
        /// </summary>
        public void Ascend()
        {
            if (!keepAscending || currentJumpTimer > duration) { jumping = false; return; }

            float proportionCompleted = currentJumpTimer / duration;
            float thisFrameForce = Mathf.Lerp(force, 0f, proportionCompleted);
            ApplyVerticalForce(thisFrameForce);
            currentJumpTimer += Time.fixedDeltaTime;
        }

        /// <summary>
        /// Resets available jumps count upon grounding.
        /// </summary>
        protected void ResetJumpCount()
        {
            if (!grounded || Time.fixedTime < jumpStartedAt + groundedGhostingTime) return;

            jumpsLeft = totalOfJumps;
        }

        /// <summary>
        /// Stops jump in progress if any.
        /// </summary>
        public void StopJump()
        {
            keepAscending = false;
        }


        #endregion

        #region Callbacks

        /// <summary>
        /// Call this to update grounding.
        /// </summary>
        /// <param name="newGrounding"></param>
        public void UpdateGrounding(bool newGrounding)
        {
            grounded = newGrounding;
        }

        /// <summary>
        /// Call this to request a Jump
        /// </summary>
        public void JumpRequested()
        {
            if (!CanStartJumping) return;
            keepAscending = true;
            StartJump();
        }

        /// <summary>
        /// Call this in order to Lock jump and
        /// prevent new jumps to occur based on
        /// shouldLock boolean.
        /// </summary>
        /// <param name="shouldLock"></param>
        public void LockJump(bool shouldLock)
        {
            jumpLocked = shouldLock;
        }

        public void OnSlopeEvaluation(SlopeData slopeData)
        {
            LockJump(slopeData.higherThanMax);
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

            if (seekGroundedUpdater)
            {
                groundedUpdater = GetComponent<IGroundedUpdater>();
                if (groundedUpdater == null)
                    Debug.LogWarning("Component Jump 2D might not work properly. It is marked to seek for an IGroundedUpdater but it could not find any.");
                groundedUpdater?.GroundedUpdate.AddListener(UpdateGrounding);
            }

            if (seekSlopeUpdater)
            {
                slopeUpdater = GetComponent<ISlopeUpdater>();
                if (slopeUpdater == null)
                    Debug.LogWarning("Component Jump 2D might not work properly. It is marked to seek for an ISlopeUpdater but it could not find any.");
                slopeUpdater?.SlopeDataUpdate.AddListener(OnSlopeEvaluation);
            }

            if (seekJumpHandler)
            {
                jumpHandler = GetComponent<IJumpHandler>();
                if (jumpHandler == null)
                    Debug.LogWarning("Component Jump 2D might not work properly. It is marked to seek for an IJumpHandler but it could not find any.");
                jumpHandler?.JumpRequested.AddListener(JumpRequested);
                jumpHandler?.JumpCanceled.AddListener(StopJump);
            }
        }

        /// <summary>
        /// Unsubscribes from events
        /// </summary>
        protected virtual void UnsubscribeFromUpdates()
        {
            groundedUpdater?.GroundedUpdate.RemoveListener(UpdateGrounding);
            slopeUpdater?.SlopeDataUpdate.RemoveListener(OnSlopeEvaluation);
            jumpHandler?.JumpRequested.RemoveListener(JumpRequested);
            jumpHandler?.JumpCanceled.RemoveListener(StopJump);
        }

        #endregion

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        public string componentName = "IndieGabo's  Jump 2D";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        public string info = "This component gives a GameObject the ability to jump as long as anything with its reference requests it.";

        [field: SerializeField, ReadOnly, Label("Feed Requirements"), TextArea(1, 30), Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        public string requirements = "On FixedUpdate: \n"
                                        + "UpdateGronding(bool newGrounding) \n\n"
                                        + "On Demand:\n"
                                        + "JumpRequested() \n"
                                        + "StopJump()";

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

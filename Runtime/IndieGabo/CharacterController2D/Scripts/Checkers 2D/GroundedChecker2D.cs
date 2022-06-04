using System.Collections;
using System.Collections.Generic;

using IndieGabo.CharacterController2D.Interfaces;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace IndieGabo.CharacterController2D.Checkers2D
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Checkers 2D/GroundedChecker2D")]
    [RequireComponent(typeof(Collider2D))]
    public class GroundedChecker2D : Checker2D, IGroundedUpdater
    {
        #region Inspector

        [Header("Debug")]
        [Tooltip("Turn this on and get some visual feedback. Do not forget to turn your Gizmos On")]
        [SerializeField] protected bool debugOn = false;

        [Tooltip("This is only informative. Shoul not be touched")]
        [ShowIf("debugOn"), SerializeField, ReadOnly] protected bool grounded = false;


        [Header("Ground check Collider")]
        [Tooltip("This is optional. You can either specify the collider or leave to this component to find a CapsuleCollider2D. Usefull if you have multiple colliders")]
        [SerializeField] new protected Collider2D collider;

        // Right stuff
        [Header("Right Detection")]

        [Tooltip("If right check should be enabled")]
        [SerializeField] protected bool checkRight = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float rightDetectionLength = 2f;

        [Tooltip("An offset position for where right detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float rightPositionXOffset = 0f;

        [Tooltip("An offset position for where rightdetection should start on Y axis")]
        [SerializeField, Range(-100f, 100f)] protected float rightPositionYOffset = 0f;

        // Left Stuff
        [Header("Left Detection")]
        [Tooltip("If left check should be enabled")]
        [SerializeField] protected bool checkLeft = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float leftDetectionLength = 2f;

        [Tooltip("An offset position for where left detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float leftPositionXOffset = 0f;

        [Tooltip("An offset position for where left detection should start on Y axis")]
        [SerializeField, Range(-100f, 100f)] protected float leftPositionYOffset = 0f;

        // Center Stuff
        [Header("Center Detection")]
        [Tooltip("If center check should be enabled")]
        [SerializeField] protected bool checkCenter = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float centerDetectionLength = 2f;

        [Tooltip("An offset position for where center detection should start on X axis")]
        [SerializeField, Range(-100f, 1000f)] protected float centerPositionYOffset = 0f;

        [Header("Layers")]
        [InfoBox("Without this the component won't work", EInfoBoxType.Warning), Tooltip("Inform what layers should be considered ground")]
        [SerializeField, Space] protected LayerMask whatIsGround;

        [Header("Available Events:"), Space, InfoBox("You can use these to directly set listeners about this GameObject's grounding")]
        public UnityEvent<bool> GroundedUpdateEvent;

        #endregion

        #region Getters
        protected float lengthConvertionRate = 100f;
        protected float positionOffsetConvertionRate = 100f;

        // All this convertions are made to make life easier on inspector
        protected float RightLengthConverted => rightDetectionLength / lengthConvertionRate;
        protected float LeftLengthConverted => leftDetectionLength / lengthConvertionRate;
        protected float CenterLengthConverted => centerDetectionLength / lengthConvertionRate;

        // Positioning offset convertions
        protected float RightPositionXOffset => rightPositionXOffset / positionOffsetConvertionRate;
        protected float RightPositionYOffset => rightPositionYOffset / positionOffsetConvertionRate;
        protected float CenterPositionYOffset => centerPositionYOffset / positionOffsetConvertionRate;
        protected float LeftPositionXOffset => leftPositionXOffset / positionOffsetConvertionRate;
        protected float LeftPositionYOffset => leftPositionYOffset / positionOffsetConvertionRate;

        /// <summary>
        /// The whole purpose of this component. Behold: The ground check.
        /// </summary>
        /// <returns> true if... grounded! </returns>
        public bool Grounded => grounded;

        public UnityEvent<bool> GroundedUpdate => GroundedUpdateEvent;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            if (collider == null) collider = GetComponent<Collider2D>();
        }

        protected virtual void FixedUpdate()
        {
            CheckGrounding();
        }

        #endregion

        /// <summary>
        /// Casts rays to determine if character is grounded.
        /// </summary>
        /// <returns> true if grounded </returns>
        public void CheckGrounding()
        {
            CastPositions positions = CalculatePositions(collider.bounds.center, collider.bounds.extents);

            RaycastHit2D rightHit = Physics2D.Raycast(positions.right, Vector2.down, RightLengthConverted, whatIsGround);
            RaycastHit2D leftHit = Physics2D.Raycast(positions.left, Vector2.down, LeftLengthConverted, whatIsGround);
            RaycastHit2D centerHit = Physics2D.Raycast(positions.center, Vector2.down, CenterLengthConverted, whatIsGround);

            bool check = (checkRight && rightHit.collider != null) || (checkCenter && centerHit.collider != null) || (checkLeft && leftHit.collider != null);

            UpdateGroundedStatus(check);
            DebugGroundCheck(positions, rightHit, leftHit, centerHit);
        }

        /// <summary>
        /// Updates grounded status based on groundedUpdate parameter.
        /// This will send an UnityEvent<bool> case grounding status 
        /// has changed.
        /// </summary>
        /// <param name="groundedUpdate"></param>
        public void UpdateGroundedStatus(bool groundedUpdate)
        {
            if (grounded == groundedUpdate) return;
            grounded = groundedUpdate;
            GroundedUpdateEvent.Invoke(grounded);
        }

        /// <summary>
        /// Calculates positions where to cast from based on collider properties.
        /// </summary>
        /// <param name="center"> Stands for the collider center as a Vector2 </param>
        /// <param name="extents"> Stands for the collider extents as a Vector 2 </param>
        /// <returns></returns>
        protected CastPositions CalculatePositions(Vector2 center, Vector2 extents)
        {
            Vector2 rightPos = center + new Vector2(extents.x + RightPositionXOffset, -extents.y + RightPositionYOffset);
            Vector2 leftPos = center + new Vector2(-extents.x - LeftPositionXOffset, -extents.y + LeftPositionYOffset);
            Vector2 centerPos = center + new Vector2(0, -extents.y + CenterPositionYOffset);

            return new CastPositions(rightPos, centerPos, leftPos);
        }


        /// <summary>
        /// Debugs the ground check.
        /// </summary>
        protected void DebugGroundCheck(CastPositions positions, RaycastHit2D rightHit, RaycastHit2D leftHit, RaycastHit2D centerHit)
        {
            if (!debugOn) return;

            if (checkRight)
                Debug.DrawRay(positions.right, Vector2.down * RightLengthConverted, rightHit.collider ? Color.red : Color.green);

            if (checkLeft)
                Debug.DrawRay(positions.left, Vector2.down * LeftLengthConverted, leftHit.collider ? Color.red : Color.green);

            if (checkCenter)
                Debug.DrawRay(positions.center, Vector2.down * CenterLengthConverted, centerHit.collider ? Color.red : Color.green);
        }

        /// <summary>
        /// Represents positions where to RayCast from
        /// </summary>
        protected struct CastPositions
        {
            public Vector2 right;
            public Vector2 center;
            public Vector2 left;

            public CastPositions(Vector2 rightPos, Vector2 centerPos, Vector2 leftPos)
            {
                right = rightPos;
                center = centerPos;
                left = leftPos;
            }
        }

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        public string componentName = "IndieGabo's Grounded Checker 2D";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        public string info = "This component checks for grounding based on the Collider2D of this GameObject";

        [field: SerializeField, ReadOnly, Label("Feed Requirements"), TextArea(1, 30), Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        public string requirements = "None. Good to go.";

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

using System.Collections;
using System.Collections.Generic;

using IndieGabo.CharacterController2D.Data;
using IndieGabo.CharacterController2D.Interfaces;
using IndieGabo.CharacterController2D.Enums;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace IndieGabo.CharacterController2D.Checkers2D
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Checkers 2D/SlopeChecker2D")]
    [RequireComponent(typeof(Collider2D))]
    [DefaultExecutionOrder(-400)]
    public class SlopeChecker2D : Checker2D, ISlopeUpdater
    {
        #region Inspector

        [Header("Debug")]
        [Tooltip("Turn this on and get some visual feedback. Do not forget to turn your Gizmos On")]
        [SerializeField] protected bool debugOn = false;

        [Tooltip("This is only informative. Shoul not be touched")]
        [ShowIf("debugOn"), SerializeField, ReadOnly] protected bool onSlope = false;

        [ShowIf("debugOn"), SerializeField, ReadOnly] protected SlopeData currentSlopeData;

        [Header("Detection")]
        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField, Range(1f, 20f)] protected float detectionLength = 2f;

        [Tooltip("Inform what layers should be considered ground"), InfoBox("Without this the component won't work", EInfoBoxType.Warning)]
        [SerializeField, Space] protected LayerMask whatIsGround;

        [Header("Angles")]
        [SerializeField][Range(1, 45)] protected int maxAngle = 45;

        [Header("Detection Positioning")]

        [Tooltip("An offset position for where detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float positionXOffset = 0f;

        [Tooltip("An offset position for where detection should start on Y axis")]
        [SerializeField, Range(-10f, 100f)] protected float positionYOffset = 0f;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IFacingDirectionUpdater you can mark this and it will subscribe to its events. PCActions implements it.")]
        [SerializeField] protected bool seekFacingDirectionUpdater = false;

        [Foldout("Update Seekers")]
        [Tooltip("If you guarantee your GameObject has a component wich implements an IGroundedUpdater you can mark this and it will subscribe to its events. GroundedChecker2D implements it.")]
        [SerializeField] protected bool seekGroundedUpdater = false;

        [Header("Available Events:"), Space]
        [Tooltip("Associate this event with callbacks that seek for SlopeData"), InfoBox("You can use these to directly set listeners about this GameObject being on a slope")]
        public UnityEvent<SlopeData> SlopeDataUpdateEvent;

        #endregion

        #region Components

        protected new Collider2D collider2D;
        protected IGroundedUpdater groundedUpdater; // Receives info about GameObject's being grounded.
        protected IFacingDirectionUpdater facingDirectionUpdater; // Receives info about GameObject's movement direction.

        #endregion

        #region Properties

        protected bool grounded = false;
        protected float RealMaxAngle => maxAngle - 0.1f;
        protected HorizontalDirections facingDirection = HorizontalDirections.Right;
        protected int lastAbsoluteDirection = 1;

        #endregion

        #region Getters

        // Direct checking
        public bool OnSlope => onSlope;
        protected int FacingDirectionSign => facingDirection == HorizontalDirections.Right ? 1 : -1;

        // All this convertions are made to make life easier on inspector
        protected float ConvertedDetectionLength => detectionLength / 10;
        protected float PositionXOffset => positionXOffset / 10;
        protected float PositionYOffset => positionYOffset / 10;

        // Event getters
        public UnityEvent<SlopeData> SlopeDataUpdate => SlopeDataUpdateEvent;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            collider2D = GetComponent<Collider2D>();
        }

        protected virtual void Start()
        {
            SubscribeToUpdates();
        }

        protected virtual void FixedUpdate()
        {
            SlopeCheck();
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

        /// <summary>
        /// Checks if is hitting a wall
        /// </summary>
        /// <returns> true if hitting a wall </returns>
        protected virtual void SlopeCheck()
        {
            SlopeData data = new SlopeData();

            float length = ConvertedDetectionLength; // Just "length" is easier to read.

            CastPositions positions = CalculatePositions(collider2D.bounds.center, collider2D.bounds.extents, FacingDirectionSign);

            RaycastHit2D centerHit = Physics2D.Raycast(positions.center, Vector2.down, length, whatIsGround);
            RaycastHit2D frontHit = Physics2D.Raycast(positions.front, Vector2.down, length * 2, whatIsGround);
            RaycastHit2D backHit = Physics2D.Raycast(positions.back, Vector2.down, length * 2, whatIsGround);
            data.normalPerpendicular = Vector2.Perpendicular(centerHit.normal).normalized;

            DebugSlopeData(positions, length, centerHit, data);


            if (!grounded) // can't be on a slope if not grounded
            {
                data.onSlope = false;
                UpdateSlopeData(data);
                return;
            }

            if (!centerHit.collider && !frontHit.collider && !backHit.collider) // No hit means not on slope
            {
                data.onSlope = false;
                UpdateSlopeData(data);
                return;
            }

            // Angles must be round up so comparisons won't be broken up.
            float frontAngle = Mathf.Round(Vector2.Angle(frontHit.normal, Vector2.up));
            float centerAngle = Mathf.Round(Vector2.Angle(centerHit.normal, Vector2.up));
            float backAngle = Mathf.Round(Vector2.Angle(backHit.normal, Vector2.up));

            if (Mathf.Abs(centerAngle) == 0 && Mathf.Abs(frontAngle) == 0 && Mathf.Abs(backAngle) == 0) // On Flat ground
            {
                data.onSlope = false;
                UpdateSlopeData(data);
                return;
            }

            if (centerAngle > maxAngle || frontAngle > maxAngle || backAngle > maxAngle) // On slope but too high
            {
                data.onSlope = true;
                data.higherThanMax = true;
                UpdateSlopeData(data);
                return;
            }

            data.onSlope = true;
            data.ascending = frontHit.point.y > backHit.point.y;
            data.descending = centerHit.point.y < backHit.point.y;
            data.exitingFromAbove = Mathf.Abs(frontAngle) == 0 && data.ascending;
            data.exitingFromBelow = Mathf.Abs(centerAngle) == 0 && data.descending;

            UpdateSlopeData(data);
        }

        /// <summary>
        /// Calculates positions where to cast from based on collider properties and facing direction.
        /// </summary>
        /// <param name="colliderCenter"></param>
        /// <param name="colliderExtents"></param>
        /// <param name="dirX"></param>
        /// <returns></returns>
        protected virtual CastPositions CalculatePositions(Vector2 colliderCenter, Vector2 colliderExtents, int dirX)
        {
            Vector2 centerPos;
            Vector2 frontPos;
            Vector2 backPos;

            centerPos = colliderCenter + new Vector2(0, -colliderExtents.y + PositionYOffset);

            if (dirX > 0)
            {
                frontPos = colliderCenter + new Vector2(colliderExtents.x + PositionXOffset, -colliderExtents.y + PositionYOffset);
                backPos = colliderCenter + new Vector2(-colliderExtents.x - PositionXOffset, -colliderExtents.y + PositionYOffset);
            }
            else
            {
                frontPos = colliderCenter + new Vector2(-colliderExtents.x - PositionXOffset, -colliderExtents.y + PositionYOffset);
                backPos = colliderCenter + new Vector2(colliderExtents.x + PositionXOffset, -colliderExtents.y + PositionYOffset);
            }

            return new CastPositions(frontPos, centerPos, backPos);
        }

        protected virtual void UpdateSlopeData(SlopeData newData)
        {
            currentSlopeData = newData;
            onSlope = newData.onSlope;
            SlopeDataUpdate.Invoke(newData);
        }

        #region Callbacks

        public virtual void UpdateFacingDirection(HorizontalDirections newFacingDirection)
        {
            facingDirection = newFacingDirection;
        }

        public virtual void UpdateGrounding(bool newGrounding)
        {
            grounded = newGrounding;
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
            if (seekFacingDirectionUpdater)
            {
                facingDirectionUpdater = GetComponent<IFacingDirectionUpdater>();
                if (facingDirectionUpdater == null)
                    Debug.LogWarning("Component SlopeChecker2D might not work properly. It is marked to seek for an IMovementDirectionUpdater but it could not find any.");
                facingDirectionUpdater?.DirectionFacingUpdate.AddListener(UpdateFacingDirection);
            }

            if (seekGroundedUpdater)
            {
                groundedUpdater = GetComponent<IGroundedUpdater>();
                if (groundedUpdater == null)
                    Debug.LogWarning("Component SlopeChecker2D might not work properly. It is marked to seek for an IGroundedUpdater but it could not find any.");
                groundedUpdater?.GroundedUpdate.AddListener(UpdateGrounding);
            }
        }

        /// <summary>
        /// Unsubscribes from events
        /// </summary>
        protected virtual void UnsubscribeFromUpdates()
        {
            groundedUpdater?.GroundedUpdate.RemoveListener(UpdateGrounding);
            facingDirectionUpdater?.DirectionFacingUpdate.RemoveListener(UpdateFacingDirection);
        }

        #endregion

        /// <summary>
        /// Represents positions where to RayCast from
        /// </summary>
        protected struct CastPositions
        {
            public Vector2 front;
            public Vector2 center;
            public Vector2 back;

            public CastPositions(Vector2 frontPos, Vector2 centerPos, Vector2 backPos)
            {
                front = frontPos;
                center = centerPos;
                back = backPos;
            }
        }

        protected virtual void DebugSlopeData(CastPositions positions, float length, RaycastHit2D centerHit, SlopeData data)
        {
            if (!debugOn) return;

            Debug.DrawRay(positions.center, Vector2.down * length, centerHit ? Color.red : Color.green);
            Debug.DrawRay(positions.front, Vector2.down * length * 2, Color.yellow);
            Debug.DrawRay(positions.back, Vector2.down * length * 2, Color.cyan);
            Debug.DrawRay(centerHit.point, data.normalPerpendicular, Color.blue);
            Debug.DrawRay(centerHit.point, centerHit.normal, Color.black);
        }

        // Debug:
        // Debug.DrawRay(objCollider.bounds.center, Vector2.down* (objCollider.bounds.extents.y + length), centerHit? Color.red : Color.green);
        // Debug.DrawRay(centerHit.point, data.normalPerpendicular, Color.blue);
        // Debug.DrawRay(centerHit.point, centerHit.normal, Color.black);
        // Debug.DrawRay(positions.front, Vector2.down* length * 2, Color.yellow);
        // Debug.DrawRay(positions.back, Vector2.down* length * 2, Color.cyan);

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        private string componentName = "IndieGabo's  Slope Checker 2D";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        private string info = "This component checks if this GameObject is on a slope based on its Collider2D.";

        [ReadOnly, Label("Feed Requirements"), TextArea(1, 30), SerializeField, Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        private string requirements = "On FixedUpdate: \n"
                                        + "UpdateGronding(bool newGrounding)";

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

using System.Collections;
using System.Collections.Generic;
using IndieGabo.CharacterController2D.Data;
using IndieGabo.CharacterController2D.Interfaces;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace IndieGabo.CharacterController2D.Checkers2D
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Checkers 2D/WallHitChecker2D")]
    [RequireComponent(typeof(Collider2D))]
    public class WallHitChecker2D : Checker2D, IWallHitUpdater
    {
        #region Inspector

        [Header("Debug")]
        [Tooltip("Turn this on and get some visual feedback. Do not forget to turn your Gizmos On")]
        [SerializeField] protected bool debugOn = false;

        [Tooltip("This is only informative. Shoul not be touched")]
        [ShowIf("debugOn"), SerializeField, ReadOnly] protected bool hittingWall = false;

        [Header("Ground check Collider")]
        [Tooltip("This is optional. You can either specify the collider or leave to this component to find a CapsuleCollider2D. Usefull if you have multiple colliders")]
        [SerializeField] new protected Collider2D collider;

        // Right stuff
        [Header("Upper Right Detection"), Space]
        [Tooltip("If upper right check should be enabled")]
        [SerializeField] protected bool checkUpperRight = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float upperRightDetectionLength = 2f;

        [Tooltip("An offset position for where upper right detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float upperRightPositionXOffset = 0f;

        [Tooltip("An offset position for where upper right detection should start on Y axis")]
        [SerializeField, Range(-100f, 100f)] protected float upperRightPositionYOffset = 0f;

        [Header("Lower Right Detection"), Space]
        [Tooltip("If lower right check should be enabled")]
        [SerializeField] protected bool checkLowerRight = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float lowerRightDetectionLength = 2f;

        [Tooltip("An offset position for where lower right detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float lowerRightPositionXOffset = 0f;

        [Tooltip("An offset position for where lower right detection should start on Y axis")]
        [SerializeField, Range(-100f, 100f)] protected float lowerRightPositionYOffset = 0f;

        // Left Stuff
        [Header("Upper Left Detection"), Space]
        [Tooltip("If left upper check should be enabled")]
        [SerializeField] protected bool checkUpperLeft = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float upperLeftDetectionLength = 2f;

        [Tooltip("An offset position for where upper left detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float upperLeftPositionXOffset = 0f;

        [Tooltip("An offset position for where upper left detection should start on Y axis")]
        [SerializeField, Range(-100f, 100f)] protected float upperLeftPositionYOffset = 0f;

        [Header("Lower Left Detection"), Space]
        [Tooltip("If left lower check should be enabled")]
        [SerializeField] protected bool checkLowerLeft = true;

        [Tooltip("Detection's length. Tweek this to suit your needs")]
        [SerializeField][Range(1f, 100f)] protected float lowerLeftDetectionLength = 2f;

        [Tooltip("An offset position for where lower left detection should start on X axis")]
        [SerializeField, Range(-100f, 100f)] protected float lowerLeftPositionXOffset = 0f;

        [Tooltip("An offset position for where lower left detection should start on Y axis")]
        [SerializeField, Range(-100f, 100f)] protected float lowerLeftPositionYOffset = 0f;

        [Header("Layers"), Space]
        [InfoBox("Without this the component won't work", EInfoBoxType.Warning), Tooltip("Inform what layers should be considered wall")]
        [SerializeField, Space] protected LayerMask whatIsWall;

        [Header("Available Events:"), Space]
        [Tooltip("Associate this event with callbacks that seek for WallHitData"), InfoBox("You can use these to directly set listeners about this GameObject colliding with walls")]
        public UnityEvent<WallHitData> WallHitUpdateEvent;

        #endregion

        #region Getters

        protected float lengthConvertionRate = 100f;
        protected float positionOffsetConvertionRate = 100f;

        // All this convertions are made to make life easier on inspector
        protected float UpperRightLengthConverted => upperRightDetectionLength / lengthConvertionRate;
        protected float LowerRightLengthConverted => lowerRightDetectionLength / lengthConvertionRate;
        protected float UpperLeftLengthConverted => upperLeftDetectionLength / lengthConvertionRate;
        protected float LowerLeftLengthConverted => lowerLeftDetectionLength / lengthConvertionRate;

        // Positioning offset convertions for code legibility
        protected float UpperRightPositionXOffset => upperRightPositionXOffset / positionOffsetConvertionRate;
        protected float UpperRightPositionYOffset => upperRightPositionYOffset / positionOffsetConvertionRate;
        protected float LowerRightPositionXOffset => lowerRightPositionXOffset / positionOffsetConvertionRate;
        protected float LowerRightPositionYOffset => lowerRightPositionYOffset / positionOffsetConvertionRate;
        protected float UpperLeftPositionXOffset => upperLeftPositionXOffset / positionOffsetConvertionRate;
        protected float UpperLeftPositionYOffset => upperLeftPositionYOffset / positionOffsetConvertionRate;
        protected float LowerLeftPositionXOffset => lowerLeftPositionXOffset / positionOffsetConvertionRate;
        protected float LowerLeftPositionYOffset => lowerLeftPositionYOffset / positionOffsetConvertionRate;

        /// <summary>
        /// Duh... the wall hit.
        /// </summary>
        /// <returns> true if... hitting a wall! </returns>
        public bool HittingWall => hittingWall;
        public UnityEvent<WallHitData> WallHitUpdate => WallHitUpdateEvent;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            if (collider == null) collider = GetComponent<Collider2D>();
        }

        protected virtual void FixedUpdate()
        {
            CheckWallHitting();
        }

        #endregion

        /// <summary>
        /// Casts rays to determine if character is grounded.
        /// </summary>
        /// <returns> true if grounded </returns>
        public void CheckWallHitting()
        {
            WallHitData data = new WallHitData();

            CastPositions positions = CalculatePositions(collider.bounds.center, collider.bounds.extents);

            if (checkUpperRight)
            {
                RaycastHit2D hit = Physics2D.Raycast(positions.upperRight, Vector2.right, UpperRightLengthConverted, whatIsWall);
                DebugCast(positions.upperRight, Vector2.right * UpperRightLengthConverted, hit);

                if (hit && hit.collider)
                {
                    data.upperRight = true;
                    data.upperRightHitAngle = Vector2.Angle(positions.upperRight, hit.point);
                }
            }

            if (checkLowerRight)
            {
                RaycastHit2D hit = Physics2D.Raycast(positions.lowerRight, Vector2.right, LowerRightLengthConverted, whatIsWall);
                DebugCast(positions.lowerRight, Vector2.right * LowerRightLengthConverted, hit);

                if (hit && hit.collider)
                {
                    data.lowerRight = true;
                    data.lowerRightHitAngle = Vector2.Angle(positions.lowerRight, hit.point);
                }
            }

            if (checkUpperLeft)
            {
                RaycastHit2D hit = Physics2D.Raycast(positions.upperLeft, Vector2.left, UpperLeftLengthConverted, whatIsWall);
                DebugCast(positions.upperLeft, Vector2.left * UpperLeftLengthConverted, hit);

                if (hit && hit.collider)
                {
                    data.upperLeft = true;
                    data.upperLeftHitAngle = Vector2.Angle(positions.upperLeft, hit.point);
                }
            }

            if (checkLowerLeft)
            {
                RaycastHit2D hit = Physics2D.Raycast(positions.lowerLeft, Vector2.left, LowerLeftLengthConverted, whatIsWall);
                DebugCast(positions.lowerLeft, Vector2.left * LowerLeftLengthConverted, hit);

                if (hit && hit.collider)
                {
                    data.lowerLeft = true;
                    data.lowerLeftHitAngle = Vector2.Angle(positions.lowerLeft, hit.point);
                }
            }

            UpdateWallHittingStatus(data);
        }

        /// <summary>
        /// Updates wall hitting status based on data parameter.
        /// This will send an UnityEvent<WallHitData>
        /// </summary>
        /// <param name="wallHitStatusUpdate"></param>
        public void UpdateWallHittingStatus(WallHitData data)
        {
            hittingWall = data.lowerRight || data.lowerLeft || data.upperLeft || data.upperRight;
            WallHitUpdateEvent.Invoke(data);
        }

        /// <summary>
        /// Calculates positions where to cast from based on collider properties.
        /// </summary>
        /// <param name="center"> Stands for the collider center as a Vector2 </param>
        /// <param name="extents"> Stands for the collider extents as a Vector 2 </param>
        /// <returns></returns>
        protected CastPositions CalculatePositions(Vector2 center, Vector2 extents)
        {
            Vector2 upperRightPos = center + new Vector2(extents.x + UpperRightPositionXOffset, extents.y + UpperRightPositionYOffset);
            Vector2 lowerRightPos = center + new Vector2(extents.x + LowerRightPositionXOffset, -extents.y + LowerRightPositionYOffset);
            Vector2 upperLeftPos = center + new Vector2(-extents.x - UpperLeftPositionXOffset, extents.y + UpperLeftPositionYOffset);
            Vector2 lowerLeftPos = center + new Vector2(-extents.x - LowerLeftPositionXOffset, -extents.y + LowerLeftPositionYOffset);
            return new CastPositions(upperRightPos, lowerRightPos, upperLeftPos, lowerLeftPos);
        }


        /// <summary>
        /// Debugs the ground check.
        /// </summary>
        protected void DebugCast(Vector2 start, Vector2 dir, RaycastHit2D hit)
        {
            if (!debugOn) return;
            Debug.DrawRay(start, dir, hit.collider ? Color.red : Color.green);
        }

        /// <summary>
        /// Represents positions where to RayCast from
        /// </summary>
        protected struct CastPositions
        {
            public Vector2 upperRight;
            public Vector2 lowerRight;
            public Vector2 upperLeft;
            public Vector2 lowerLeft;

            public CastPositions(Vector2 upperRightPos, Vector2 lowerRightPos, Vector2 upperLeftPos, Vector2 lowerLeftPos)
            {
                upperRight = upperRightPos;
                lowerRight = lowerRightPos;
                upperLeft = upperLeftPos;
                lowerLeft = lowerLeftPos;
            }
        }

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        private string componentName = "IndieGabo's Wall Hit Checker 2D";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        private string info = "This component checks if this GameObject is hitting a wall based on its Collider2D.";

        [ReadOnly, Label("Feed Requirements"), TextArea(1, 30), SerializeField, Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        private string requirements = "None. You are good to go.";

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

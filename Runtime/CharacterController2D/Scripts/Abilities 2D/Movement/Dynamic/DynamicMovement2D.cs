using System.Collections;
using System.Collections.Generic;
using IndieGabo.CharacterController2D.Interfaces;
using IndieGabo.CharacterController2D.Data;
using UnityEngine;
using NaughtyAttributes;

namespace IndieGabo.CharacterController2D.Abilities2D
{
    [AddComponentMenu("IndieGabo/Character Controller 2D/Abilities 2D/DynamicMovement2D")]
    public class DynamicMovement2D : DynamicMovementHandler2D
    {
        #region Editor

        [Header("Speed"), Space]
        [Tooltip("The natural speed applyed on X axis if no speed given when calling the MoveHorizontally() method.")]
        [SerializeField, Range(1f, 200f)] protected float naturalXSpeed = 2f;
        [Tooltip("The natural speed applyed on Y axis if no speed given when calling the MoveVertically() method.")]
        [SerializeField, Range(1f, 200f)] protected float naturalYSpeed = 2f;
        [Tooltip("The natural force applyed X axis if no force given when calling the ApplyForce() method.")]
        [SerializeField, Range(1f, 200f)] protected float naturalXForce = 2f;
        [Tooltip("The natural force applyed Y axis if no force given when calling the ApplyForce() method.")]
        [SerializeField, Range(1f, 200f)] protected float naturalYForce = 2f;

        [Header("Materials"), Space]
        [Tooltip("A PhysicsMaterial2D with friction = 0")]
        [SerializeField] protected PhysicsMaterial2D zeroFriction;

        [Tooltip("A PhysicsMaterial2D with friction = 100000 (maximum friction).")]
        [SerializeField] protected PhysicsMaterial2D fullFriction;

        #endregion

        #region  Properties

        protected MovementMaterials materials;

        #endregion

        #region  Getters    



        #endregion

        #region Mono

        protected override void Awake()
        {
            base.Awake();
            materials = new MovementMaterials(fullFriction, zeroFriction);
        }

        #endregion

        #region Logic

        public virtual void MoveHorizontally(float directionSign)
        {
            MoveHorizontally(naturalXSpeed, directionSign);
        }

        public virtual void MoveHorizontally(float directionSign, SlopeData slopeData)
        {
            MoveHorizontally(naturalXSpeed, directionSign, slopeData, this.materials);
        }

        public virtual void MoveHorizontally(float directionSign, SlopeData slopeData, bool ignoreSlopes)
        {
            MoveHorizontally(naturalXSpeed, directionSign, slopeData, this.materials, ignoreSlopes);
        }

        public override void MoveHorizontally(float speed, float directionSign, SlopeData slopeData, bool ignoreSlopes = false)
        {
            MoveHorizontally(speed, directionSign, slopeData, this.materials, ignoreSlopes);
        }

        #endregion

        #region IGComponent
#pragma warning disable 0414

        [Header("About this component"), Foldout("About this component")]
        [ReadOnly, Label("Name"), SerializeField, Space]
        public string componentName = "IndieGabo's  Dynamic Movement 2D";

        [ReadOnly, Label("Info"), TextArea(1, 30), SerializeField, Space, Foldout("About this component")]
        public string info = "This component gives a GameObject to move using it's RigidBody2D set to Dynamic.";

        [field: SerializeField, ReadOnly, Label("Feed Requirements"), TextArea(1, 30), Space, InfoBox("You MUST feed these functions for this component to work.", EInfoBoxType.Warning), Foldout("About this component")]
        public string requirements = "None. You are good to go.";

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

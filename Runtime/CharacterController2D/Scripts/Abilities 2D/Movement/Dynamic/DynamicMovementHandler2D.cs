using System.Collections;
using System.Collections.Generic;

using IndieGabo.CharacterController2D.Data;
using UnityEngine;

namespace IndieGabo.CharacterController2D.Abilities2D
{
    public abstract class DynamicMovementHandler2D : Ability2D
    {
        #region Components
        protected Rigidbody2D rb;

        #endregion

        #region Properties

        protected float defaultGravityScale;
        protected PhysicsMaterial2D defaultMaterial;

        #endregion

        #region Mono
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            defaultGravityScale = rb.gravityScale;
            defaultMaterial = rb.sharedMaterial;
        }

        #endregion

        #region Horizontal Movement

        /// <summary>
        /// Set a Rigidbody.velocity.x based on a given speed
        /// </summary>
        /// <param name="speed"> The desired speed </param>
        public virtual void MoveHorizontally(float speed, float directionSign)
        {
            rb.sharedMaterial = defaultMaterial;
            ApplyGravityScale(defaultGravityScale);
            rb.velocity = new Vector2(speed * directionSign, rb.velocity.y);
        }

        /// <summary>
        /// Set a Rigidbody.velocity.x based on a given speed
        /// </summary>
        /// <param name="speed"> The desired speed </param>
        public virtual void MoveHorizontallyApplyingGravity(float speed, float directionSign, float gravityScale)
        {
            rb.sharedMaterial = defaultMaterial;
            ApplyGravityScale(gravityScale);
            rb.velocity = new Vector2(speed * directionSign, rb.velocity.y);
        }

        /// <summary>
        /// Set a Rigidbody.velocity.x based on a given speed
        /// </summary>
        /// <param name="speed"> The desired speed </param>
        public virtual void MoveHorizontally(float speed, float directionSign, PhysicsMaterial2D material)
        {
            rb.sharedMaterial = material;
            rb.velocity = new Vector2(speed * directionSign, rb.velocity.y);
        }

        /// <summary>
        /// Set a Rigidbody.velocity.x based on a given X axis speed
        /// and a balancing Y axis speed.
        /// </summary>
        /// <param name="speed"> The desired speed x axis speed</param>
        /// <param name="slopeData"> The slope data to be analyzed</param>
        /// <param name="ignoreSlopes"> If slopes should be ignored </param>
        public virtual void MoveHorizontally(float speed, float directionSign, SlopeData slopeData, bool ignoreSlopes = false)
        {
            if (!slopeData.onSlope || ignoreSlopes) { MoveHorizontally(speed, directionSign); return; }

            rb.sharedMaterial = defaultMaterial;

            // At this point it is considered that the GameObject is on a slope

            // This is the default way to handle velocity on slopes. Attention to the normal perpendicular.
            Vector2 newVelocity = new Vector2(-directionSign * speed * slopeData.normalPerpendicular.x, -directionSign * speed * slopeData.normalPerpendicular.y);

            // Case we are on a forbidden angle.
            if (slopeData.higherThanMax)
            {
                newVelocity.Set(0, rb.velocity.y);
                rb.velocity = newVelocity;

                ApplyGravityScale(Mathf.Clamp(Mathf.Pow(defaultGravityScale, 3), 25, 40)); // We wanna make sure it slips;
                return;
            }

            // Here we prevent that weird jump when exinting a slope from above. Just lock velocity on Y axis as zero.
            if (slopeData.exitingFromAbove)
            {
                newVelocity.Set(-directionSign * speed * slopeData.normalPerpendicular.x, 0f);
                rb.velocity = newVelocity;
                return;
            }

            // Prevent that weird jump when exiting a slope from below. Gravity scale it up.
            if (slopeData.exitingFromBelow)
            {
                ApplyGravityScale(defaultGravityScale * 3);
                newVelocity.Set(-directionSign * speed * slopeData.normalPerpendicular.x, 0f);
                rb.velocity = newVelocity;
                return;
            }

            ApplyGravityScale(defaultGravityScale);
            rb.velocity = newVelocity; // Phew... apply velocity. 
        }

        /// <summary>
        /// Set a Rigidbody.velocity.x based on a given X axis speed
        /// and a balancing Y axis speed.
        /// </summary>
        /// <param name="speed"> The desired speed x axis speed</param>
        /// <param name="slopeData"> The slope data to be analyzed</param>
        /// <param name="ignoreSlopes"> If we slopes should be ignored </param>
        public virtual void MoveHorizontally(float speed, float directionSign, SlopeData slopeData, MovementMaterials materials, bool ignoreSlopes = false)
        {
            if (!slopeData.onSlope || ignoreSlopes) { MoveHorizontally(speed, directionSign); return; }

            // At this point it is considered that the GameObject is on a slope

            // If on slope and has no speed on X axis locks GameObject on the ground.
            if (speed == 0) { rb.sharedMaterial = materials.fullFriction; }

            // If moving on X axis GameObject should move normally.
            if (speed != 0) { rb.sharedMaterial = materials.zeroFriction; }

            // This is the default way to handle velocity on slopes. Attention to the normal perpendicular.
            Vector2 newVelocity = new Vector2(-directionSign * speed * slopeData.normalPerpendicular.x, -directionSign * speed * slopeData.normalPerpendicular.y);

            // Case we are on a forbidden angle.
            if (slopeData.higherThanMax)
            {
                newVelocity.Set(0, rb.velocity.y);
                rb.velocity = newVelocity;
                rb.sharedMaterial = materials.zeroFriction; // Should slip down.
                ApplyGravityScale(Mathf.Clamp(Mathf.Pow(defaultGravityScale, 3), 25, 40)); // We wanna make sure it slips;
                return;
            }

            // Here we prevent that weird jump when exinting a slope from above. Just lock velocity on Y axis as zero.
            if (slopeData.exitingFromAbove)
            {
                newVelocity.Set(-directionSign * speed * slopeData.normalPerpendicular.x, 0f);
                rb.velocity = newVelocity;
                return;
            }

            // Prevent that weird jump when exiting a slope from below. Gravity scale it up.
            if (slopeData.exitingFromBelow)
            {
                ApplyGravityScale(defaultGravityScale * 3);
                newVelocity.Set(-directionSign * speed * slopeData.normalPerpendicular.x, 0f);
                rb.velocity = newVelocity;
                return;
            }

            ApplyGravityScale(defaultGravityScale);
            rb.velocity = newVelocity; // Phew... apply velocity. 
        }

        public virtual void ApplyHorizontalForce(float force, float directionSign)
        {
            rb.gravityScale = defaultGravityScale;
            rb.AddForce(new Vector2(force * directionSign, rb.velocity.y));
        }

        #endregion

        #region Vertical Movement


        /// <summary>
        /// Set a Rigidbody.velocity.x based on a given speed
        /// </summary>
        /// <param name="speed"> The desired speed </param>
        public virtual void MoveVertically(float speed)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }

        public virtual void ApplyVerticalForce(float force)
        {
            ApplyVerticalForce(force, defaultGravityScale);
        }

        public virtual void ApplyVerticalForce(float force, float gravityScale)
        {
            rb.gravityScale = gravityScale;
            rb.AddForce(new Vector2(rb.velocity.x, force));
        }

        #endregion

        #region Data Structure

        public struct MovementMaterials
        {
            public PhysicsMaterial2D fullFriction;
            public PhysicsMaterial2D zeroFriction;

            public MovementMaterials(PhysicsMaterial2D fullFriction, PhysicsMaterial2D zeroFriction)
            {
                this.fullFriction = fullFriction;
                this.zeroFriction = zeroFriction;
            }
        }

        #endregion

        #region Gravity

        public virtual void ApplyGravityScale(float scale)
        {
            rb.gravityScale = scale;
        }

        public virtual void ResetGravityScale()
        {
            ApplyGravityScale(defaultGravityScale);
        }

        #endregion
    }
}

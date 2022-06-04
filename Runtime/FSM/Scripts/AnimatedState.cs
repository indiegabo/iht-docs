using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace IndieGabo.FSM
{
    /// <summary>
    /// This class is crafted for the case you want to use an Animator and select a boolean parameter to be turned on and off. 
    /// respectively by the OnEnter and OnExit methods.
    /// </summary>
    public abstract class AnimatedState : State
    {

        #region Inspector

        [SerializeField, Space, Tooltip("The Actor's Animator")]
        protected Animator animator;

        [ShowIf("AnimatorSet"), AnimatorParam("animator"), Tooltip("The Animator's parameter to set while in this state")]
        public string animatorParamName = "";

        #endregion

        #region Properties

        /// <summary>
        /// Shorthand for knowing if animator is set
        /// </summary>
        protected bool AnimatorSet => animator != null;

        #endregion

        /// <summary>
        /// Called every time the StateMachine activates this state
        /// </summary>
        public virtual void OnEnter()
        {
            if (AnimatorSet && animatorParamName != "")
                animator.SetBool(animatorParamName, true);
        }

        /// <summary>
        /// Called every time the StateMachine deactivates this state
        /// </summary>
        public virtual void OnExit()
        {
            if (AnimatorSet && animatorParamName != "")
                animator.SetBool(animatorParamName, false);
        }
    }
}
using System;
using UnityEngine;

namespace IndieGabo.FSM
{
    public class StateTransition
    {
        /// <summary>
        /// The condition wich evaluates if transition should be made
        /// </summary>
        public Func<bool> Condition { get; protected set; }

        public State state;

        /// <summary>
        /// The transition priority level
        /// </summary>
        public int priority { get; protected set; }

        public StateTransition(Func<bool> Condition, State target, int priority = 0)
        {
            this.Condition = Condition;
            this.state = target;
            this.priority = priority;
        }

    }
}
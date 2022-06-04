using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IndieGabo.CharacterController2D.Interfaces
{
    /// <summary>
    /// Any GameOject that wants to controll moving direction can use an
    /// event through implementing this Interface.
    /// </summary>
    public interface IMovementDirectionUpdater
    {

        /// <summary>
        /// An event wich should be fired to update movements direction
        /// </summary>
        /// <value> A Vector2 representing the direction. This MUST be normalized </value>
        UnityEvent<Vector2> MovementDirectionUpdate { get; }
    }
}

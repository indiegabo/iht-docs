using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IndieGabo.CharacterController2D.Enums;

namespace IndieGabo.CharacterController2D.Interfaces
{
    /// <summary>
    /// Any GameOject that wants to controll facing direction can use an
    /// event through implementing this Interface.
    /// </summary>
    public interface IFacingDirectionUpdater
    {

        /// <summary>
        /// An event wich should be fired to update facing direction
        /// </summary>
        /// <value> A HorizontalDirections representing the direction. </value>
        UnityEvent<HorizontalDirections> DirectionFacingUpdate { get; }
    }
}

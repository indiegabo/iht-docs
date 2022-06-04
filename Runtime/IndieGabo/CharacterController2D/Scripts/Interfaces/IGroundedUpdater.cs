using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IndieGabo.CharacterController2D.Interfaces
{
    /// <summary>
    /// Any GameOject that wants to give information about being or not grounded
    /// through an event must implement this Interface.
    /// </summary>
    public interface IGroundedUpdater
    {
        UnityEvent<bool> GroundedUpdate { get; }
    }
}

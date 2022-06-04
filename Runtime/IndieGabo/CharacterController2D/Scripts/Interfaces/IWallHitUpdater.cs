using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IndieGabo.CharacterController2D.Data;

namespace IndieGabo.CharacterController2D.Interfaces
{
    /// <summary>
    /// Any GameOject that wants to give information about being or not grounded
    /// through an event must implement this Interface.
    /// </summary>
    public interface IWallHitUpdater
    {
        UnityEvent<WallHitData> WallHitUpdate { get; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IndieGabo.CharacterController2D.Interfaces
{
    /// <summary>
    /// Any GameOject that wants to give information about jumping starting or being canceled
    /// through an event must implement this Interface.
    /// </summary>
    public interface IJumpHandler
    {

        UnityEvent JumpRequested { get; }
        UnityEvent JumpCanceled { get; }
    }
}

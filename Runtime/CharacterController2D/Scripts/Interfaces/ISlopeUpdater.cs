using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IndieGabo.CharacterController2D.Data;

namespace IndieGabo.CharacterController2D.Interfaces
{
    /// <summary>
    /// Any GameOject that wants to give information about being or not on a Slope
    /// through an event must implement this Interface.
    /// </summary>
    public interface ISlopeUpdater
    {
        UnityEvent<SlopeData> SlopeDataUpdate { get; }
    }
}

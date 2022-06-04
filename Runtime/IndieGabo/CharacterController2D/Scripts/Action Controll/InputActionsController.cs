using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NaughtyAttributes;


namespace IndieGabo.CharacterController2D.ActionControll
{
    /// <summary>
    /// Represents any entity wich is an ActionsController and
    /// receives player Input.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public abstract class InputActionsController : ActionsController
    {

        #region Required Components
        protected PlayerInput playerInput;

        #endregion

        #region Properties

        [SerializeField, ReadOnly] protected string currentMapName;

        #endregion

        #region Getters

        public string CurrentMapName => currentMapName;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        protected virtual void Start()
        {
            currentMapName = playerInput.currentActionMap.name;
        }

        #endregion

        #region Logic

        public virtual void ChangeMap(string mapName)
        {
            currentMapName = mapName;
            playerInput.SwitchCurrentActionMap(mapName);
        }

        public virtual void ChangeMap(InputActionMap map)
        {
            currentMapName = map.name;
            playerInput.SwitchCurrentActionMap(map.name);
        }

        #endregion
    }
}

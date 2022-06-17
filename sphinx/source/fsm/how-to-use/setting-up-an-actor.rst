.. _setting-up-an-actor-page:

Setting up an Actor
===================

What is an Actor?
-----------------

Anything that performs several actions and therefore will benefit from an :ref:`StateMachine <state-machine-class>` handling its states of action, fits the concept of an actor for us. 

That being said, the :ref:`Actor <actor-class>` class exists to be the gateway between the :ref:`StateMachine <state-machine-class>` and any 
other components wich handle anything for the Actor. Even the actor's own stuff.

e.g. you can have a component that handles the movement of the actor, and another that handles the animation of the actor. Both of them will 
have references within the actor's script, and therefore any of the actor's states can access them. 

.. code-block:: csharp
    
    // State class stuff

    protected void FixedTick()
    {
        actor.movement.Move(); // accessing a component inside a state 
    }

    //  More State class stuff    

You can have as many actors representations as you wish in your game. Being the class :ref:`Actor <actor-class>` an abstraction, just implement 
classes wich inherit from the :ref:`Actor <actor-class>` class.

How it Works
------------

At its ``MonoBehaviour.Awake()`` the :ref:`Actor <actor-class>` will try finding a :ref:`StateMachine <state-machine-class>` component among either its own GameObject
or its children GameObjects. 

From there on the Machine will auto regulate the actor's states evaluating if it needs transitioning into another state on each of the actor's 
``MonoBehaviour.Update()`` ``MonoBehaviour.FixedUpdate()`` and ``MonoBehaviour.LateUpdate()`` methods execution.


MonoBehaviour Tickers
---------------------   

At each of the Actor's MonoBehaviour "frame handlers" it will call the Machine's corresponding ticker. The machine
will then call its current state corresponding Ticker method. 

This means that all states will be synced with the actor's MonoBehaviour methods. Therefore we can say all action's
execution are isolated inside the state respecting the actor's pacing.

The Code
--------

Below is a good start for creating an :ref:`Actor <actor-class>` script of your own. Feel free to copy:

.. important::

    Be aware that you need calling the base class methods in case of overrides of the core ``MonoBehaviour`` 
    methods (wich will probably be needed). 

.. code-block:: csharp

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using IndieGabo.FSM;

    public class MyActor : Actor
    {
        #region MonoBehaviour Methods

        // Be aware that you need calling the base class methods in case of overrides
        // (wich will probably be needed). Here are the important ones already coded:

        protected override void Awake() // Attention to override
        {
            base.Awake(); // Case you did override this MUST be here.
            // Code from here on...
        }
        
        protected override void Update() // Attention to override
        {
            base.Update(); // Case you did override this MUST be here.
            // Code from here on...
        }
        
        protected override void LateUpdate() // Attention to override
        {
            base.LateUpdate(); // Case you did override this MUST be here.
            // Code from here on...
        }

        protected override void FixedUpdate() // Attention to override
        {
            base.FixedUpdate(); // Case you did override this MUST be here.
            // Code from here on...
        }

        #endregion
    }

You can also right click on a folder of your Unity Editor's project window and follow the path *Create* ``->`` *IndieGabo* ``->`` *FSM* ``->`` *New Actor*. 
Selecting this will create a new ``Actor`` script under the right clicked folder for you (Make sure you have the package installed).

.. image:: ../../_static/images/actor-script-from-menu.gif
   :alt: Creating actor script from menu
   :width: 600

Hurray! You've just created your first Actor! Congratulations! Time to understand how to create states for your Actor by :ref:`clicking here <creating-states-for-an-actor-page>`.

|

.. raw:: html

     <div style="clear: both;" ></div>

.. image:: ../../_static/images/gabinho.png
   :alt: Gabinho Waving
   :align: right
   :width: 80

|
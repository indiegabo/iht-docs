.. _welcome-fsm-page:

FSM
===

Much welcome to the IndieGabo's FSM!

This is a code driven FSM (Finate State Machine) solution for your Unity project.

What is a Finite State Machine?
-------------------------------

A Finate State Machine is a design pattern focused on dealing with entities that have multiple possible states. 

Why this FSM?
-------------

Although we know Unity's Animator is a StateMachine it can get messy to use it as a functionality handler. This FSM
helps you to organizing your code by holding the actors actions logic inside its respective state (we will talk about actors in a bit).

Key pratical benefits of using this FSM: 

* Helps you keep things organized
* Once you understand how it works it'll save you hours of debugging since you will know exactly where each functionality is being executed
* Avoids overload of unecessary if statements since each state will only check for the conditions based on their registered transitions
* You can get it running in like 5 minutes

How it Works
------------

Take the concept of an ``actor`` being anything on your game wich performs several actions. The ``state machine`` then is setup
to rule a given ``actor`` states of actions. The ``actor`` states must be registered into the ``state machine`` being that a 
``state`` knows when to transition into another ``state`` under specific conditions.

Also, for the usage of this FSM, we consider that a ``state`` isn't just some kind of status or definition. Our states 
**actually handles** what its ``actor`` performs while on a given ``state`` of actions.
 
How to use
----------

.. toctree::
   :maxdepth: 1

   how-to-use/setting-up-an-actor
   how-to-use/creating-states-for-an-actor
   how-to-use/using-the-state-machine
   how-to-use/parent-states

API
---

.. toctree::
   :maxdepth: 1

   api/actor
   api/state
   api/state-machine
   api/state-transition

.. raw:: html

     <div style="clear: both;" ></div>

.. image:: ../_static/images/gabinho.png
   :alt: Gabinho Waving
   :align: right
   :width: 80

|
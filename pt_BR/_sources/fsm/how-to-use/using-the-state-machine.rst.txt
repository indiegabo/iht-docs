.. _using-the-state-machine-class:

Using the state machine
=======================

Behold: THE STATE MACHINE!!!
----------------------------

Now that we know how to :ref:`setup an a actor <setting-up-an-actor-class>` and how to :ref:`create its states <creating-states-for-an-actor-page>`
we can finally use the state machine.

The :ref:`StateMachine <state-machine-class>` component, different from the actors and states, is a ready to use component. So you do not need to worry about creating any 
script for it. Just attach to a GameObject, attach states to the same GameObject, set a default state and you are good to go.

But, if you want to understand a little about it// Some other code Let's do it!

.. _lifecycle:

Life Cycle
----------

Let's take a look at the machine's Life Cycle:

.. image:: ../../_static/images/life-cycle.jpg
   :alt: Creating actor script from menu
   :width: 600

.. note:: 

   The ``StateMachine`` at its current version has 5 statuses: ``Off``, ``Loading``, ``Ready``, ``On`` and ``Paused``. They are declared as the enum
   ``IndieGabo.FSM.MachineStatus``.

I'd like to focus some attention on these steps:

1. The machine first ``StateMachine.status`` is ``MachineStatus.Off``.

Note that although our machine belongs to MonoBehaviour, it is the :ref:`Actor <actor-class>` wich sets up the machine. That
is because the machine can only start operating when the actor is already awake. Once Awake, the actor calls ``StateMachine.SetUp()``
letting it know who its actor is. 

2. At this point the machine will change ``StateMachine.status`` from ``MachineStatus.Off`` to ``MachineStatus.Loading``.

Here the machine will load up all the states attached into its GameObject and call subsequently all of the attached states ``State.OnLoad()``
followed by their ``State.SortTransitions()``. This last method makes sure transitions priorities get in order. This is the importancy of registering any
:ref:`StateTranstion <state-transition-class>` of your state inside its ``State.OnLoad()``. 

3. Now the machine will change ``StateMachine.status`` from ``MachineStatus.Loading`` to ``MachineStatus.Ready``.

Once ready, the machine will start by using ``StateMachine.ChangeState()`` to set the chosen default state (At the inspector. By you. Do not forget
to do it).

4. The machine will finally change ``StateMachine.status`` from ``MachineStatus.Ready`` to ``MachineStatus.On`` and it is running!!! Hurray!

Tickers Methods and Machine Statuses
------------------------------------

Once the machine has started, meaning its ``StateMachine.status`` is ``MachineStatus.On``, it will proceed passing forward the tickers methods (called inside the 
actor's "frame handlers") to the current state. As long as the ``StateMachine.status`` is ``MachineStatus.On`` it will do so.

If at any time you need to pause this behaviour, you can call ``StateMachine.Pause()`` and it will set ``StateMachine.status`` to ``MachineStatus.Paused``. 
Consequently, none of the tickers will be passed forward to the current state, therefore we can considered it as paused.

.. code-block:: csharp

   // Some other code from inside a state
   
    public void Tick()
    {
      machine.Pause(); // Pausing the machine
    }

    // Some other code

To unpause, simply call ``StateMachine.Resume()`` and the ``StateMachine.status`` will be ``MachineStatus.On`` again.

.. code-block:: csharp
   
   // Some other code from inside a state

    public void Tick()
    {
      machine.Resume(); // Resuming the machine.
    }

   // Some other code

Setting up the machine
----------------------

You can simply create a single GameObject and attach the :ref:`StateMachine <state-machine-class>` component into it. Same 
goes for the :ref:`Actor <actor-class>`. Take a look:

.. raw:: html
   
   <video width="800" height="450" loop autoplay muted controls>
      <source src="../../_static/videos/setting-state-machine-single-object.mp4" type="video/mp4">
   </video>  

|

But, as the actor's GameObject tends to have multiple components attached into it, for the sake of organization, i find more 
convenient to proceed creating an GameObject for the actor and use the machine as its child. Check it out:

.. raw:: html
   
   <video width="800" height="450" loop autoplay muted controls>
      <source src="../../_static/videos/setting-state-machine-as-child.mp4" type="video/mp4">
   </video>  

|

Both strategies will work transparently for you. Just choose yours.

As you could observe, i've prepared a menu item for you to put a new state machine into your hierarchy. But if you wish to attach the :ref:`StateMachine <state-machine-class>`
component into any GameObject, feel free to search for it using the "Add Component" inspector button.

All set!
--------

Checkout the API documentation if you want to know more about these classes. For now, i'd say you have all the tools needed to start benefiting from 
the state machine! 

HURRAY!! Go do something awesome and let me know about it!

.. raw:: html

     <div style="clear: both;" ></div>

.. image:: ../../_static/images/gabinho.png
   :alt: Gabinho Waving
   :align: right
   :width: 80

|
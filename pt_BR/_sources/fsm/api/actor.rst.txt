.. _actor-class:

Actor
=====

Namespace
---------

.. code-block:: csharp

   namespace IndieGabo.FSM {}

Declaration
-----------

.. code-block:: csharp

   public abstract class Actor : MonoBehaviour {}


Fields
----------

.. table:: 
   :width: 100% 
   :widths: 25 15 60

   +-----------------------------------------------+----------------------------+-----------------------------------------------------------------------------------+
   | ``protected StateMachine``                    | ``stateMachine``           |  An internal reference for the actor's :ref:`StateMachine <state-machine-class>`  |
   +-----------------------------------------------+----------------------------+-----------------------------------------------------------------------------------+

Properties
----------

.. table:: 
   :width: 100% 
   :widths: 25 15 60

   +-----------------------------------------------+----------------------------+---------------------------------------------------------------------------------+
   | ``public StateMachine { get; }``              | ``Machine``                | A public access for the actor's :ref:`StateMachine <state-machine-class>`       |
   +-----------------------------------------------+----------------------------+---------------------------------------------------------------------------------+

Methods
----------

.. table:: 
   :width: 100% 
   :widths: 25 15 60

   +-----------------------------------------------+----------------------------+-------------------------------------------------------------------------------------------------------------------------+
   | ``protected virtual void``                    | ``SetMachine()``           | Must be called internally at ``MonoBehaviour.Awake()`` in order to setup the :ref:`StateMachine <state-machine-class>`  |
   +-----------------------------------------------+----------------------------+-------------------------------------------------------------------------------------------------------------------------+

|

.. raw:: html

     <div style="clear: both;" ></div>

.. image:: ../../_static/images/gabinho.png
   :alt: Gabinho Waving
   :align: right
   :width: 80

|
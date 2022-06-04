.. _state-transition-class:

State Transition
================

Namespace
---------

.. code-block:: csharp

   namespace IndieGabo.FSM {}

Declaration
-----------

.. code-block:: csharp

   public class StateTransition {}


Properties
----------

.. table:: 
   :width: 100% 
   :widths: 30 15 45

   +-----------------------------------------------+----------------------------+---------------------------------------------------------------------------------+
   | ``public Func<bool> {get; protected set;}``   | ``Condition``              | A delegate function representing the transition's condition                     |
   +-----------------------------------------------+----------------------------+---------------------------------------------------------------------------------+
   | ``public State {get; protected set;}``        | ``state``                  | The state wich should be transitioned into based on ``Condition``               |
   +-----------------------------------------------+----------------------------+---------------------------------------------------------------------------------+
   | ``public int {get; protected set;}``          | ``priority``               | The execution priority. Higher numbers first. Default 0.                        |
   +-----------------------------------------------+----------------------------+---------------------------------------------------------------------------------+



|

.. raw:: html

     <div style="clear: both;" ></div>

.. image:: ../../_static/images/gabinho.png
   :alt: Gabinho Waving
   :align: right
   :width: 80

|
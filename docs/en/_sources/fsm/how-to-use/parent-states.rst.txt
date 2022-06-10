.. _parent-states-page:

Parent States
=============

Wait a minute... parents?
-------------------------

Yes! Parents!

You can create a state wich holds the same group of properties and functionalities for several other child states.

Say you understand that some actor of yours has several states wich can transition into the same other states. Or share a single or even a group of properties.
You can create a parent state and then create child states that inherit from that. Like the following example:

First, create a parent state:

.. code-block:: csharp

      public class GroundedState : State
      {
         Func<bool> Idle => actor.grounded && actor.rigidBody2D.velocity.x == 0;
         Func<bool> Moving => actor.grounded && actor.rigidBody2D.velocity.x != 0;
         Func<bool> Ascending => actor.rigidBody.velocity.y > 0;
         Func<bool> Descending => actor.rigidBody.velocity.y < 0;

         public virtual void OnLoad() // In case child needs to override this we set as virtual
         {
            AddTransition(Idle, GetComponent<IdleState>());
            AddTransition(Moving, GetComponent<MovingState>());
            
            // Priority 1 because we want this to be evaluated before Idle and Moving. 
            // Meaning that if we detect velocity on Y axis, we do not care about grounding.
            // The transition should occur.
            // Just an example btw.
            AddTransition(Ascending, GetComponent<AscendingState>(), 1); 
            AddTransition(Descending, GetComponent<DescendingState>(), 1);
            
         }
      }

Now we can create a GroundedState wich will inherit all those transitions:

.. code-block:: csharp

      public class IdleState : GroundedState
      {
         public void FixedTick()
         {
            // We can now care only about the Idle logic. No need to register more transitions.
         }
      }

But say you need to register new transitions for your child state:

.. code-block:: csharp

      public class IdleState : GroundedState
      {
         Func<bool> NewCondition => actor.something > actor.anotherThing;

         public override void OnLoad() // Overriding since the parent already implements this
         {
            base.OnLoad(); // This is important so parent transtions get loaded
            
            AddTransition(NewCondition, GetComponent<OtherState>());            
         }
      }

For me personally i find this quite handy. Tons of code can be avoided and you keep your project organized.

.. raw:: html

     <div style="clear: both;" ></div>

.. image:: ../../_static/images/gabinho.png
   :alt: Gabinho Waving
   :align: right
   :width: 80

|

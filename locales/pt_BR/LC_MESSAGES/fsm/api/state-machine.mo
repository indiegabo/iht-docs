��                                !   ;     ]     }     �  (   �  B   �  �     �   �  �   �  �   q  �        �     �     �     �  _   �  ;     d   R  8   �     �  
   �  ,     V   0  +   �     �     �  Q   �     <	  S   X	     �	  �  �	     M  #   m  "   �     �     �  (   �  B     �   Z  �     �   �  �   �  �   7     �     �     �     �  f   �  8   ^  l   �  <        A     J  1   d  V   �  4   �     "     ?  D   Z     �  U   �        A getter for ``defaultState`` A getter for the machine's status A visual feedback for inspector Arguments: ``Actor actor`` Arguments: ``State state`` Arguments: ``T`` where ``T`` : ``State`` Called by the :ref:`Actor <actor-class>` to initialize the machine Called every  :ref:`Actor <actor-class>` ``MonoBehaviour.FixedUpdate()``. Forces machine to |br| evaluate if should change the ``currentState`` and calls |br| the ``currentState`` ``State.FixedTick()`` Called every  :ref:`Actor <actor-class>` ``MonoBehaviour.LateUpdate()``. Forces machine to |br| evaluate if should change the ``currentState`` and calls |br| the ``currentState`` ``State.LateTick()`` Called every  :ref:`Actor <actor-class>` ``MonoBehaviour.Update()``. Forces machine to |br| evaluate if should change the ``currentState`` and calls |br| the ``currentState`` ``State.Tick()`` Case any condition of the ``currentState`` :ref:`transitions <state-transition-class>` is met, |br| the machine will transition into the target state Checks the ``StateTransition.Condition()`` of all ``State.transitions`` of the ``currentState``. Returns null if no condition is met. Declaration Events Fields Gabinho Waving If the machine is ready, sets the ``currentState`` to de default state and ``status`` to ``On`` Invoked every time the machine changes the ``currentState`` List of states attached to the machine's GameObject. |br| Will be loaded at ``StateMachine.SetUp()`` Loads all states attached into the machine's GameObject. Methods Properties Sets the ``currentState`` as the given state The actor related to this state machine |br| just for visual feedback at the inspector The default state to be set using inspector The list of attached states The machine current status The machine's actor. Either attached to |br| the machine's GameObject or a parent The machine's current state The state machine's actor instance. |br| This is set in the Machine's SetUp method. Visual feedback Project-Id-Version: IndieGabo's Handy Tools 
Report-Msgid-Bugs-To: 
POT-Creation-Date: 2022-06-04 14:28-0300
PO-Revision-Date: YEAR-MO-DA HO:MI+ZONE
Last-Translator: FULL NAME <EMAIL@ADDRESS>
Language: pt_BR
Language-Team: pt_BR <LL@li.org>
Plural-Forms: nplurals=2; plural=(n > 1);
MIME-Version: 1.0
Content-Type: text/plain; charset=utf-8
Content-Transfer-Encoding: 8bit
Generated-By: Babel 2.10.1
 Um getter para ``defaultState`` Um getter para o status da máquina Um feedback visual para o inspetor Argumentos: ``Actor actor``" Argumentos: ``State state``" Argumentos: ``T`` onde ``T`` : ``State`` Chamado pelo :ref:`Ator <actor-class>` para inicializar a máquina Chamado a cada :ref:`Actor <actor-class>` ``MonoBehaviour.FixedUpdate()``. Força a máquina a |br| avaliar se deve mudar o ``currentState`` e chama |br| o ``currentState`` ``State.FixedTick()`` Chamado a cada :ref:`Actor <actor-class>` ``MonoBehaviour.Update()``. Força a máquina a |br| avaliar se deve mudar o ``currentState`` e chama |br| o ``currentState`` ``State.Tick()`` Chamado a cada :ref:`Actor <actor-class>` ``MonoBehaviour.Update()``. Força a máquina a |br| avaliar se deve mudar o ``currentState`` e chama |br| o ``currentState`` ``State.Tick()`` Caso alguma condição das :ref:`transições transição do<state-transition-class>` ``currentState`` seja atendida, |br| a máquina irá transitar para o estado alvo Verifica a ``StateTransition.Condition()`` de todas as transições do ``currentState``. Retorna null se nenhuma condição for atendida. Declaração Eventos Campos (Fields) Gabinho Acenando Se a máquina está pronta, define o ``currentState`` como o estado padrão e o ``status`` como ``On`` Invocado toda vez que a máquina muda o ``currentState`` Lista de estados anexados ao GameObject da máquina. |br| Será carregado durante o ``StateMachine.SetUp()`` Carrega todos os estados anexados ao GameObject da máquina. Métodos Propriedades (Properties) Define o ``currentState`` como o estado fornecido O ator relacionado a este estado máquina |br| apenas para feedback visual no inspetor O estado padrão para ser definido usando o inspetor Um lista de estados anexados O status atual da máquina O ator da máquina. Anexado ao |br| GameObject da máquina ou um pai O estado atual da máquina A instância do ator da máquina. |br| Este é definido no método SetUp da máquina. Feedback visual 
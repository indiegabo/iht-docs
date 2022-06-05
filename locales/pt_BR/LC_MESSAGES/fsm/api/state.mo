��          �                 �     J   �  )   �  Z     n   y     �  -   �     "     )  #   8     \  4   d  T   �  R   �  �  A  �   �  K   �  *   �  a   �  y   Z     �  Q   �     3     C  '   T     |  ;   �  [   �  ]   	   A name you can give the state (you can use the inspector) |br| so you can have a visual feeeback at the :ref:`StateMachine <state-machine-class>` inspector Arguments: ``Func<bool> Condition``, ``State state``, ``int priority = 0`` Arguments: ``StateTransition transition`` Called by the :ref:`StateMachine <state-machine-class>` to load important functionalities. Called to register a :ref:`StateTransition <state-transition-class>`. Must be called inside ``State.OnLoad()`` Declaration Either the set ``name`` or the state ``Type`` Fields Gabinho Waving List of transitions for this state. Methods Reorders ``transitions`` considering priority levels The machine's actor. Set inside InternalLoad() method wich is called by the machine. The state machine. Set inside InternalLoad() method wich is called by the machine. Project-Id-Version: IndieGabo's Handy Tools 
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
 Um nome que você pode dar ao estado (você pode usar o inspector) |br| para você ter uma visualização de feedback no inspetor da :ref:`StateMachine <state-machine-class>` Argumentos: ``Func<bool> Condition``, ``State state``, ``int priority = 0`` Argumentos: ``StateTransition transition`` Chamado pela :ref:`StateMachine <state-machine-class>` para carregar importantes funcionalidades. Chamado para registrar uma :ref:`StateTransition <state-transition-class>`. Deve ser chamado dentro de ``State.OnLoad()`` Declaração Retorna o ``name`` ou o ``Type`` do estado caso um nome não tenha sido definido. Campos (Fields) Gabinho Acenando Lista de transições para este estado. Métodos Reordena ``transitions`` considerando níveis de prioridade O ator da máquina. Definido dentro do método InternalLoad() que é chamado pela máquina. A máquina de estado. Definido dentro do método InternalLoad() que é chamado pela máquina. 
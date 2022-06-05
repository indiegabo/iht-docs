��          �               \  �   ]  �     �   �  �   �  g        �  �   �     �     �  �   �     r     �  �   �     z  �   �     >     P  �   q	  �   A
  �  2  �   �  �   �  �   9  �     u   �  *   /  �   Z     N     `  �   n          ,  �   A  	      2  *     ]  5  o  �   �  �   u   Anything that performs several actions and therefore will benefit from an :ref:`StateMachine <state-machine-class>` handling its states of action, fits the concept of an actor for us. At each of the Actor's MonoBehaviour "frame handlers" it will call the Machine's corresponding ticker. The machine will then call its current state corresponding Ticker method. At its ``MonoBehaviour.Awake()`` the :ref:`Actor <actor-class>` will try finding a :ref:`StateMachine <state-machine-class>` component among either its own GameObject or its children GameObjects. Be aware that you need calling the base class methods in case of overrides of the core ``MonoBehaviour`` methods (wich will probably be needed). Below is a good start for creating an :ref:`Actor <actor-class>` script of your own. Feel free to copy: Creating actor script from menu From there on the Machine will auto regulate the actor's states evaluating if it needs transitioning into another state on each of the actor's ``MonoBehaviour.Update()`` ``MonoBehaviour.FixedUpdate()`` and ``MonoBehaviour.LateUpdate()`` methods execution. Gabinho Waving How it Works Hurray! You've just created your first Actor! Congratulations! Time to understand how to create states for your Actor by :ref:`clicking here <creating-states-for-an-actor-page>`. MonoBehaviour Tickers Setting up an Actor That being said, the :ref:`Actor <actor-class>` class exists to be the gateway between the :ref:`StateMachine <state-machine-class>` and any other components wich handle anything for the Actor. Even the actor's own stuff. The Code This means that all states will be synced with the actor's MonoBehaviour methods. Therefore we can say all action's execution are isolated inside the state respecting the actor's pacing. What is an Actor? You can also right click on a folder of your Unity Editor's project window and follow the path *Create* ``->`` *IndieGabo* ``->`` *FSM* ``->`` *New Actor*. Selecting this will create a new ``Actor`` script under the right clicked folder for you (Make sure you have the package installed). You can have as many actors representations as you wish in your game. Being the class :ref:`Actor <actor-class>` an abstraction, just implement classes wich inherit from the :ref:`Actor <actor-class>` class. e.g. you can have a component that handles the movement of the actor, and another that handles the animation of the actor. Both of them will have references within the actor's script, and therefore any of the actor's states can access them. Project-Id-Version: IndieGabo's Handy Tools 
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
 Qualquer coisa que executa várias ações e portanto se beneficia de um :ref:`StateMachine <state-machine-class>` lidando com os seus estados de ação, atende ao conceito de ator para nós. Em cada execução dos "Frame Handlers" do MonoBehaviour do Ator, ele chamará o ticker correspondente da máquina. A máquina por sua vez chamará o método ticker do estado atual. No ``MonoBehaviour.Awake()`` do :ref:`Actor <actor-class>`, ele tentará encontrar um componente :ref:`StateMachine <state-machine-class>` componente em seu próprio GameObject ou nos GameObjects filhos do seu GameObject. Tenha em mente que você precisa chamar os métodos da classe base em caso de sobrescrita dos métodos ``MonoBehaviour`` (que provavelmente serão necessárias). Abaixo está um bom início para criar seu próprio script de :ref:`Ator <actor-class>`. Fique a vontade para copiar. Criando um script de ator a partir do menu A partir daí a máquina irá regular os estados do ator avaliando se ele precisa transitar para outro estado em cada execução dos métodos ``MonoBehaviour.Update()``, ``MonoBehaviour.FixedUpdate()`` e ``MonoBehaviour.LateUpdate()`` do ator. Gabinhgo Acenando Como funciona Boa! Você acabou de criar seu primeiro ator. Parabéns! Hora de aprender como criar estados para seu Ator :ref:`clicando aqui <creating-states-for-an-actor-page>`. Tickers do MonoBehaviour Configurando um ator Sendo assim, a classe :ref:`Actor <actor-class>` existe para ser a conexão entre a :ref:`StateMachine <state-machine-class>` e qualquer outro componente que manipule algo para o ator. Incluindo as coisas do próprio ator. O Código Isso significa que todos os estados serão sincronizados com os métodos ``MonoBehaviour.Update()``, ``MonoBehaviour.FixedUpdate()`` e ``MonoBehaviour.LateUpdate()`` MonoBehaviour do ator. Portanto, podemos dizer que todas as ações executadas estão isoladas dentro do estado respeitando o ritmo do ator. O que é um ator? Você também pode clicar com o botão direito no painel de projeto do Editor da Unity e seguir o caminho *Create* ``->`` *IndieGabo* ``->`` *FSM* ``->`` *New Actor*. A Unity criará um novo script de ``ator`` no diretório clicado (Certifique-se de que você tenha o pacote de templates de script instalado). Você pode ter quantas representações de atores quiser no seu joguinho. Sendo a classe :ref:`Actor <actor-class>` uma abstração, apenas implemente classes que herdam da classe :ref:`Actor <actor-class>`. ex. você pode ter um componente que lida com o movimento do ator, e outro que manipula a animação do ator. Ambos devem ter referências dentro do script do ator, e, portanto, qualquer um dos estados do ator pode acessá-los. 
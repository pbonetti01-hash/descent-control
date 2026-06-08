# Decente Control

<img width="1241" height="701" alt="Captura de tela 2026-06-08 155022" src="https://github.com/user-attachments/assets/75d306b1-4f03-45bf-9528-af80bbefee94" />

**Decente Control** é um simulador de pouso espacial em 3D desenvolvido com foco em precisão física, controle de vetores e gerenciamento de inércia. O projeto desafia o jogador a assumir o comando de diferentes módulos de aterrissagem com o objetivo de desacelerar, alinhar e pousar com segurança em superfícies planetárias variadas.

Desenvolvido pela **Jet Planet Studios**, o jogo foi projetado dentro de um escopo enxuto e otimizado, destacando os desafios técnicos reais da física de descida e aterrissagem de forma direta e altamente desafiadora.

<img width="1242" height="695" alt="Captura de tela 2026-06-08 170016" src="https://github.com/user-attachments/assets/ab42f49e-a626-43e8-b64d-e925eebe0529" />

## 🚀 Funcionalidades e Características Principais

* **Simulação de Gravidade Realista:** Sistemas de gravidade específicos configurados para três corpos celestes distintos: **Terra, Lua e Marte**. Cada ambiente altera drasticamente a inércia e a resposta dos motores da nave.
* **Módulos de Pouso Diferenciados:** Três naves jogáveis com comportamentos, pesos e sensibilidades de controle ajustados individualmente.
* **Validação de Impacto Precisa:** Sistema matemático que checa as condições exatas no milissegundo do contato com o solo:
  * Tipo de nave correto para a missão.
  * Tolerância angular vertical máxima (ex: até 15° de inclinação).
  * Velocidade máxima segura de impacto (ex: até 7 m/s).
* **Interface Limpa e Inteligente (UI HUD):** Telemetria em tempo real que exibe coordenadas de posição e vetores durante o voo, ocultando-se de forma inteligente (desabilitando a renderização do Canvas) no momento da falha para garantir uma tela de *Game Over* limpa e minimalista.
* **Menu de Classificação (Rating System):** Pousos bem-sucedidos avaliam a performance do jogador em categorias como *Perfect Landing!*, *Good Landing* ou *Hard Landing* com base na suavidade do toque, sem poluir a tela com dados feios de texto.
* **Identidade Visual Dedicada:** Criação de terrenos detalhados, texturas customizadas, efeitos de pós-processamento (Post-Processing), shaders personalizadas e sistemas de partículas para simular propulsão e impactos.
* **Cinemática Dinâmica:** Inclusão de cutscenes que ambientam o início da missão e uma Câmera de Morte dedicada que foca na destruição da nave sob ângulos dramáticos em caso de falha.

<img width="1242" height="703" alt="Captura de tela 2026-06-08 170031" src="https://github.com/user-attachments/assets/328e55e7-f4f7-4a2d-86aa-3eb37f1b9a7e" />

## 🎮 Core Gameplay (Loop de Jogo)

O ciclo de jogabilidade é ágil, focado no aprendizado por repetição e no domínio dos comandos:

1. **Preparação:** O jogador escolhe o planeta e o módulo espacial correspondente.
2. **Descida:** Monitorando os dados de velocidade e inclinação, o jogador gerencia o empuxo (*thrust*) e a rotação para alinhar o módulo.
3. **Resultado:** * **Sucesso:** Tela de veredito (*Rating*) com a classificação limpa e transição automática para a próxima fase.
   * **Fracasso:** Ocultação instantânea da renderização da telemetria, ativação automática da câmera de morte e exibição da tela de *Game Over* com opções de reinício (*Try Again* ou *Back to Menu*).

<img width="1246" height="702" alt="Captura de tela 2026-06-08 170105" src="https://github.com/user-attachments/assets/b7343780-8efd-4770-84ec-7cf4f7b8f9e7" />

## 🛠️ Tecnologias Utilizadas

* **Engine:** Unity 3D (3D Render Pipeline)
* **Linguagem:** C# (C-Sharp)
* **UI:** TextMeshPro (TMP) para textos nítidos e responsivos.
* **Gráficos e Efeitos:** Custom Shaders, Unity Particle Systems e Post-Processing Stack.

<img width="1903" height="1076" alt="Captura de tela 2026-06-08 201536" src="https://github.com/user-attachments/assets/3e6710be-89c0-4a81-8d12-cfcc650b0ca5" />

## 📁 Estrutura dos Principais Componentes de Código

* **`SoilDetection.cs`:** Gerenciador do solo e validador de pouso. Controla as condições de vitória/derrota, congela a física da nave no sucesso, desativa a propriedade `.enabled` dos componentes Canvas de telemetria/coordenadas para limpar a tela e ativa a câmera de morte.
* **`MainMenuManager.cs`:** Controla o fluxo de navegação entre o painel inicial, seleção de fases, créditos e transições de cena.
* **`OptionsMenu.cs`:** Gerencia configurações básicas do jogo como volume geral (via `AudioListener`), qualidade gráfica e controle de tela (alternando dinamicamente através de botões entre *Tela Cheia* e *Modo Janela*).

<img width="2554" height="1434" alt="Captura de tela 2026-06-08 201810" src="https://github.com/user-attachments/assets/e776fbe0-0809-4e7a-a20c-8dbb1f28e079" />

## 👥 Créditos e Desenvolvimento

O projeto foi inteiramente concebido e desenvolvido dentro da **Jet Planet Studios**, com os seguintes focos de atuação:

* **Ideia Original e Concepção do Projeto**
* **Programação Principal (Gameplay, Física de Vetores e Sistemas de Gravidade)**
* **Design de Interface (UI/UX) e Telas de Menu**
* **Criação de Terrenos, Texturas e Shaders Customizadas**
* **Sistemas de Partículas, Pós-Processamento e Cutscenes**

*Desenvolvido como um projeto prático focado em mecânicas essenciais e física aplicada dentro da Unity.*

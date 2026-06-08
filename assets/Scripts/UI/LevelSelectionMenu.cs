using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador do Menu de Seleção de Níveis para a Global Solution.
/// Gerencia a transição entre telas de UI e o carregamento das fases do jogo.
/// </summary>
public class LevelSelectionMenu : MonoBehaviour
{
    [Header("Transição de Telas")]
    [Tooltip("Arraste o GameObject do painel do Menu Principal aqui")]
    [SerializeField] private GameObject mainMenuPanel;

    [Tooltip("Arraste o GameObject deste painel de Seleção de Fases aqui")]
    [SerializeField] private GameObject levelSelectionPanel;

    [Header("Configuração das Cenas (Nomes Identificadores)")]
    [Tooltip("Nome exato da cena correspondente à Fase 1 no Build Settings")]
    public string level1SceneName = "Level1";

    [Tooltip("Nome exato da cena correspondente à Fase 2 no Build Settings")]
    public string level2SceneName = "Level2";

    [Tooltip("Nome exato da cena correspondente à Fase 3 no Build Settings")]
    public string level3SceneName = "Level3";

    // --- FUNÇÕES PARA OS BOTÕES DAS FASES ---

    /// <summary>
    /// Função para o botão "Fase 1". Carrega a cena definida na string level1SceneName.
    /// </summary>
    public void LoadLevel1()
    {
        LoadSceneByName(level1SceneName);
    }

    /// <summary>
    /// Função para o botão "Fase 2". Carrega a cena definida na string level2SceneName.
    /// </summary>
    public void LoadLevel2()
    {
        LoadSceneByName(level2SceneName);
    }

    /// <summary>
    /// Função para o botão "Fase 3". Carrega a cena definida na string level3SceneName.
    /// </summary>
    public void LoadLevel3()
    {
        LoadSceneByName(level3SceneName);
    }

    // --- FUNÇÃO PARA O BOTÃO BACK ---

    /// <summary>
    /// Função para o botão "Back" (Voltar).
    /// Desativa o painel de seleção de fases e reativa o menu principal.
    /// </summary>
    public void BackToMainMenu()
    {
        if (mainMenuPanel != null && levelSelectionPanel != null)
        {
            levelSelectionPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Por favor, associe os painéis 'mainMenuPanel' e 'levelSelectionPanel' no Inspector!");
        }
    }

    // --- MÉTODO AUXILIAR PRIVADO ---

    /// <summary>
    /// Método interno para validar e carregar de forma segura as cenas por string.
    /// </summary>
    private void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("O nome da cena está vazio! Verifique as configurações do script no Inspector.");
        }
    }
}
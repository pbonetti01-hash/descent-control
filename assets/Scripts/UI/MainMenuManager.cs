using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Painéis do Menu (Canvas/Gis)")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    // --- BOTÃO PLAY ---
    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // --- BOTÃO OPTIONS ---
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        optionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    // --- BOTÃO CREDITS ---
    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(true); 
    }

    // --- BOTÃO VOLTAR (Para usar nos menus de Fase, Opções e Créditos) ---
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false); // Garante que o crédito suma ao voltar
    }

    // --- BOTÃO EXIT ---
    public void ExitGame()
    {
        Debug.Log("O jogo fechou! (Isso só funciona na Build final)");
        Application.Quit();
    }

    // --- FUNÇÃO EXTRA: Carregar uma fase ---
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
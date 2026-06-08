using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Transição de Telas")]
    [Tooltip("Arraste o GameObject do painel do Menu Principal aqui")]
    [SerializeField] private GameObject mainMenuPanel;

    [Tooltip("Arraste o GameObject deste painel de Opções aqui")]
    [SerializeField] private GameObject optionsPanel;

    public void BackToMainMenu()
    {
        if (mainMenuPanel != null && optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Por favor, associe os painéis 'mainMenuPanel' e 'optionsPanel' no Inspector!");
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        Debug.Log($"Volume alterado para: {volume}");
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log($"Qualidade gráfica alterada para o índice: {qualityIndex}");
    }

    public void SetFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Debug.Log("Jogo alterado para Tela Cheia");
    }

    public void SetWindowedMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Debug.Log("Jogo alterado para Modo Janela");
    }
}
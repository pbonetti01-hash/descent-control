using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SoilDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject allowedShip;
    [SerializeField] private float maxAngleTolerance = 15f;
    [SerializeField] private float maxSafeSpeed = 7f;
    [SerializeField] private string nextSceneName = "";

    [Header("Death Camera Settings")]
    [Tooltip("Arraste aqui a câmera que deve focar na explosão/morte do jogador.")]
    [SerializeField] private GameObject deathCamera;

    [Header("UI HUD (Componentes Canvas para Desativar Renderização)")]
    [Tooltip("Arraste aqui os COMPONENTES CANVAS dos menus trancados (coordenadas, telemetria, etc.) que se recusam a sumir.")]
    [SerializeField] private Canvas[] hudCanvasesToDisable;

    [Header("UI Canvases de Resultado")]
    [SerializeField] private GameObject ratingCanvas;

    [Header("UI Text Components")]
    [SerializeField] private TextMeshProUGUI ratingText;

    private bool hasLanded = false;

    private void Start()
    {
        // Garante que a câmera de morte sempre comece desativada ao carregar ou reiniciar a cena
        if (deathCamera != null)
        {
            deathCamera.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        GameObject incomingShip = collision.gameObject;

        if (incomingShip.CompareTag("Player"))
        {
            hasLanded = true;

            // Desativa a renderização de todos os Canvas em lote
            if (hudCanvasesToDisable != null)
            {
                for (int i = 0; i < hudCanvasesToDisable.Length; i++)
                {
                    if (hudCanvasesToDisable[i] != null)
                    {
                        hudCanvasesToDisable[i].enabled = false;
                    }
                }
            }

            float angle = Vector3.Angle(incomingShip.transform.up, Vector3.up);
            float speed = collision.relativeVelocity.magnitude;

            bool wrongShip = incomingShip != allowedShip;
            bool badAngle = angle > maxAngleTolerance;
            bool badSpeed = speed > maxSafeSpeed;

            if (wrongShip || badAngle || badSpeed)
            {
                // FALHA: Ativa a câmera de morte antes de explodir a nave
                if (deathCamera != null)
                {
                    deathCamera.SetActive(true);
                }

                // Dispara o GameOver direto na nave
                incomingShip.SendMessage("GameOver", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                // SUCESSO: Mostra APENAS o texto do Rating (Clean)
                ProcessSuccess(speed, angle, incomingShip);
            }
        }
    }

    private void ProcessSuccess(float speed, float angle, GameObject ship)
    {
        string rating;
        Color textColor;
        float speedRatio = speed / maxSafeSpeed;

        if (speedRatio <= 0.25f && angle <= 3f)
        {
            rating = "PERFECT LANDING!";
            textColor = Color.cyan;
        }
        else if (speedRatio <= 0.6f && angle <= 8f)
        {
            rating = "GOOD LANDING";
            textColor = Color.green;
        }
        else
        {
            rating = "HARD LANDING";
            textColor = Color.yellow;
        }

        FreezeShip(ship);
        DisplayRating(rating, textColor);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Invoke("LoadNextScene", 3f);
        }
    }

    private void DisplayRating(string rating, Color textColor)
    {
        if (ratingCanvas != null) ratingCanvas.SetActive(true);
        if (ratingText != null)
        {
            ratingText.text = rating;
            ratingText.color = textColor;
        }
    }

    private void FreezeShip(GameObject ship)
    {
        if (ship.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
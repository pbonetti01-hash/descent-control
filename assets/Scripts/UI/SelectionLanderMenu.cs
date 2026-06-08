using UnityEngine;
using TMPro;

public class SelectionLanderMenu : MonoBehaviour
{
    [Header("Carousel")]
    [SerializeField] private GameObject[] ships;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject menuUI;

    [Header("Cameras")]
    [SerializeField] private Camera previewCamera;

    [Header("Telemetry")]
    [SerializeField] private TelemetryPanel telemetryUI;

    private int currentIndex = 0;

    private void Start()
    {
        if (previewCamera != null) previewCamera.gameObject.SetActive(true);

        if (ships == null || ships.Length == 0) return;
        if (menuUI != null) menuUI.SetActive(true);
        UpdateCarousel();
    }

    public void NextShip()
    {
        currentIndex = (currentIndex + 1) % ships.Length;
        UpdateCarousel();
    }

    public void PreviousShip()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = ships.Length - 1;
        UpdateCarousel();
    }

    public void ConfirmSelection()
    {
        GameObject selectedShip = ships[currentIndex];

        if (selectedShip != null)
        {
            Rigidbody rb = selectedShip.GetComponent<Rigidbody>();

            if (selectedShip.TryGetComponent<LanderClassicController>(out var classic))
                classic.SelectAndStartShip();
            else if (selectedShip.TryGetComponent<TitanLanderController>(out var titan))
                titan.SelectAndStartShip();
            else if (selectedShip.TryGetComponent<SpiderLanderController>(out var spider))
                spider.SelectAndStartShip();

            if (previewCamera != null) previewCamera.gameObject.SetActive(false);

            Camera shipCamera = selectedShip.GetComponentInChildren<Camera>(true);
            if (shipCamera != null)
            {
                shipCamera.gameObject.SetActive(true);
            }

            if (telemetryUI != null && rb != null)
            {
                telemetryUI.InitTelemetry(rb);
            }
        }

        if (menuUI != null) menuUI.SetActive(false);
    }

    private void UpdateCarousel()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i] != null)
            {
                bool isActive = (i == currentIndex);
                ships[i].SetActive(isActive);

                if (isActive && nameText != null)
                {
                    nameText.text = ships[i].name;
                }
            }
        }
    }
}
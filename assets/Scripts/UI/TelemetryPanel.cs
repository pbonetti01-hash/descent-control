using UnityEngine;
using TMPro;

public class TelemetryPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelUI;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI posText;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.1f;

    private Rigidbody rb;
    private Transform shipTransform;
    private float timer;
    private bool isRunning = false;

    private void Start()
    {
        if (panelUI != null) panelUI.SetActive(false);
    }

    private void Update()
    {
        if (isRunning && rb != null)
        {
            if (panelUI != null && !panelUI.activeSelf) panelUI.SetActive(true);
            UpdatePanel();
        }
    }

    public void InitTelemetry(Rigidbody targetRb)
    {
        if (targetRb != null)
        {
            rb = targetRb;
            shipTransform = targetRb.transform;
            isRunning = true;
        }
    }

    private void UpdatePanel()
    {
        timer += Time.deltaTime;

        float speed = rb.linearVelocity.magnitude;
        float displaySpeed = speed * 3.6f;

        if (speedText != null)
        {
            speedText.text = $"{displaySpeed:F1}";
            speedText.color = (speed > 6f) ? Color.red : Color.green;
        }

        if (timer >= updateInterval)
        {
            timer = 0f;
            if (posText != null && shipTransform != null)
            {
                Vector3 pos = shipTransform.position;
                posText.text = $"POS_ X: {pos.x:F0}  Y: {pos.y:F0}  Z: {pos.z:F0}";
            }
        }
    }
}
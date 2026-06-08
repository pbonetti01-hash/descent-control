using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpiderLanderController : MonoBehaviour
{
    [Header("Flight")]
    [SerializeField] private float thrustImpulse = 4.5f;
    [SerializeField] private float minThrust = 8f;
    [SerializeField] private float maxThrust = 42f;
    [SerializeField] private float thrustTime = 14f;

    [Header("Rotation")]
    [SerializeField] private float tiltSpeed = 45f;
    [SerializeField] private float turnSpeed = 65f;

    [Header("Limits")]
    [SerializeField] private float maxUpVelocity = 8f;
    [SerializeField] private float maxHorVelocity = 4.5f;

    [Header("Impact")]
    [SerializeField] private float maxSafeSpeed = 9.5f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject shipModel;

    [Header("Menu")]
    [SerializeField] private GameObject menuUI;

    private Rigidbody rb;
    private PlayerControls controls;

    private bool isThrusting = false;
    private float thrustTimer = 0f;
    private Vector2 moveInput;
    private float rotInput;
    private bool isDead = false;
    private bool hasStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        rb.linearDamping = 0.05f;
        rb.angularDamping = 3.5f;
        rb.constraints = RigidbodyConstraints.None;

        rb.useGravity = false;
        rb.isKinematic = true;

        controls.Player.MainThrust.started += ctx => { if (!isDead && hasStarted) TriggerImpulse(); };
        controls.Player.MainThrust.performed += ctx => { if (!isDead && hasStarted) isThrusting = true; };
        controls.Player.MainThrust.canceled += ctx => ResetThrust();

        controls.Player.LateralMovement.performed += ctx => { if (!isDead && hasStarted) moveInput = ctx.ReadValue<Vector2>(); };
        controls.Player.LateralMovement.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Rotate.performed += ctx => { if (!isDead && hasStarted) rotInput = ctx.ReadValue<float>(); };
        controls.Player.Rotate.canceled += ctx => rotInput = 0f;

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (menuUI != null) menuUI.SetActive(true);
    }

    private void OnEnable()
    {
        if (hasStarted) controls.Player.Enable();
    }

    private void OnDisable() => controls.Player.Disable();

    private void FixedUpdate()
    {
        if (isDead || !hasStarted) return;

        ProcessThrust();
        ProcessRotation();
        LimitVelocity();
    }

    public void SelectAndStartShip()
    {
        if (hasStarted) return;

        hasStarted = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        controls.Player.Enable();

        if (menuUI != null) menuUI.SetActive(false);
    }

    private void TriggerImpulse()
    {
        Vector3 vel = rb.linearVelocity;
        if (vel.y < 0)
        {
            vel.y *= 0.5f;
            rb.linearVelocity = vel;
        }
        rb.AddForce(transform.up * thrustImpulse, ForceMode.Impulse);
    }

    private void ProcessThrust()
    {
        if (isThrusting)
        {
            thrustTimer += Time.fixedDeltaTime;
            thrustTimer = Mathf.Clamp(thrustTimer, 0f, thrustTime);
            float t = thrustTimer / thrustTime;
            float force = Mathf.Lerp(minThrust, maxThrust, t);

            rb.AddForce(transform.up * force, ForceMode.Force);
        }
    }

    private void ResetThrust()
    {
        isThrusting = false;
        thrustTimer = 0f;
    }

    private void ProcessRotation()
    {
        float yaw = rotInput * turnSpeed * Time.fixedDeltaTime;
        float pitch = moveInput.y * tiltSpeed * Time.fixedDeltaTime;
        float roll = -moveInput.x * tiltSpeed * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up, yaw, Space.Self);
        transform.Rotate(Vector3.right, pitch, Space.Self);
        transform.Rotate(Vector3.forward, roll, Space.Self);
    }

    private void LimitVelocity()
    {
        Vector3 vel = rb.linearVelocity;

        if (vel.y > maxUpVelocity)
        {
            vel.y = maxUpVelocity;
        }

        Vector2 horVel = new Vector2(vel.x, vel.z);
        if (horVel.magnitude > maxHorVelocity)
        {
            horVel = horVel.normalized * maxHorVelocity;
            vel.x = horVel.x;
            vel.z = horVel.y;
        }

        rb.linearVelocity = vel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead || !hasStarted) return;

        float speed = collision.relativeVelocity.magnitude;
        Debug.Log("Spider impact speed: " + speed);

        if (speed > maxSafeSpeed)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isDead = true;
        isThrusting = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (shipModel != null) shipModel.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }
}

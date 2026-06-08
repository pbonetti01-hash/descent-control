using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class TitanLanderController : MonoBehaviour
{
    [Header("Flight")]
    [SerializeField] private float thrustImpulse = 12f;
    [SerializeField] private float reverseThrust = 18f;
    [SerializeField] private float minThrust = 10f;
    [SerializeField] private float maxThrust = 45f;
    [SerializeField] private float thrustTime = 5f;

    [Header("Rotation")]
    [SerializeField] private float tiltSpeed = 80f;
    [SerializeField] private float turnSpeed = 120f;

    [Header("Limits")]
    [SerializeField] private float maxUpVelocity = 15f;
    [SerializeField] private float maxHorVelocity = 12f;

    [Header("Impact")]
    [SerializeField] private float maxSafeSpeed = 12f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject shipModel;

    private Rigidbody rb;
    private PlayerControls controls;

    private bool isThrusting = false;
    private bool isReversing = false;
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
        rb.angularDamping = 2.5f;
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

        InputAction reverseAction = controls.asset.FindAction("ReverseThrust");
        if (reverseAction != null)
        {
            reverseAction.performed += ctx => { if (!isDead && hasStarted) isReversing = true; };
            reverseAction.canceled += ctx => isReversing = false;
        }

        if (gameOverUI != null) gameOverUI.SetActive(false);
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
        ProcessReverse();
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
    }

    private void TriggerImpulse()
    {
        Vector3 vel = rb.linearVelocity;
        if (vel.y < 0)
        {
            vel.y *= 0.2f;
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

    private void ProcessReverse()
    {
        if (isReversing || Input.GetKey(KeyCode.Z))
        {
            rb.AddForce(-transform.up * reverseThrust, ForceMode.Force);
        }
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
        Debug.Log("Titan impact speed: " + speed);

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
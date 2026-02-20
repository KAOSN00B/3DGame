using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Cinemachine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private Transform visual;
    [SerializeField] private float rotationSpeed = 15.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private float deathDelay = 3.0f;


    private Rigidbody rb;
    private Vector2 input;
    private int itemCount;
    private bool isDead = false;
    public bool isDeadPublic => isDead;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionY;
    }

    private void Update()
    {
        if (isDead) return;

        if (Time.timeScale == 0f)
            return;

        PlayerControls();
    }

    private void PlayerControls()
    {
        input = Vector2.zero;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            input.x = -1;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            input.x = 1;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            input.y = -1;
        else if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            input.y = 1;

        input = input.normalized;

        bool isRunning = input != Vector2.zero;
        animator.SetBool("isRunning", isRunning);
    }

    private void FixedUpdate()
    {
        // Get camera directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten so we don’t move vertically
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Build movement relative to camera
        Vector3 move = camForward * input.y + camRight * input.x;

        rb.linearVelocity = move * moveSpeed;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            visual.rotation = Quaternion.Slerp(
                visual.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void PickUpItem()
    {
        itemCount++;
        itemText.text = "Bombs: " + itemCount.ToString();
    }

    public bool UseItem()
    {
        if (itemCount <= 0)
            return false;

        itemCount--;
        itemText.text = "Bombs: " + itemCount;
        return true;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        animator.SetBool("isDead", true);
        Debug.Log(animator.GetBool("isDead"));

        var cam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
        if (cam != null)
        {
            cam.Follow = null;
            cam.LookAt = null;
        }

        enabled = false;

        Respawner.Instance.RespawnPlayer(this);
    }

    public void Revive()
    {
        isDead = false;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        // TURN OFF DEATH STATE
        animator.SetBool("isDead", false);

        enabled = true;

        var cam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
        if (cam != null)
        {
            cam.Follow = transform;
            cam.LookAt = transform;
        }


    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
public class PlayerController : MonoBehaviour
{

    private Inputs inputs;
    private Vector2 inputMovementDirection;
    private Vector2 cameraRotation;
    private CharacterController characterController;

    [Header("References")]
    [SerializeField] private GameObject cam;
    [SerializeField] private Animator batAnimator;

    [Header("Parameters")]
    public float moveSpeed;

    public float sensitivity;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputs = new Inputs();
        inputs.Player.Move.performed += ctx => inputMovementDirection = inputs.Player.Move.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => inputMovementDirection = Vector2.zero;


        inputs.Player.Swing.performed += ctx => SwingBat();
        //inputs.Player.UnlockCursor.performed += ctx => SwingBat();
    }

    void OnEnable()
    {
        inputs.Enable();
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMovementDirection.magnitude != 0)
            MovePlayer();

        RotateCamera();


    }

    private void MovePlayer()
    {
        Vector3 moveDir = transform.forward * inputMovementDirection.y + transform.right * inputMovementDirection.x;
        characterController.Move(moveDir.normalized * Time.deltaTime * moveSpeed);
        transform.position = new Vector3(transform.position.x, 0.9f, transform.position.z);
    }
    private void SwingBat()
    {
        batAnimator.SetTrigger("Swing");
    }

    private void RotateCamera()
    {
        cameraRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        cameraRotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);

        cam.transform.localRotation = yQuat;
        transform.localRotation = xQuat;
    }

    private void UnlockCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

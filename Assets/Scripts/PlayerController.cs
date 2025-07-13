using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private InputActionReference walkAction;
    [SerializeField] private InputActionReference interactAction;

    [SerializeField] private Rigidbody m_rigidBody;

    private Vector2 moveAmount = new();

    void Awake()
    {
        interactAction.action.performed += Interact;
    }

    void OnEnable()
    {
        walkAction.action.Enable();
        interactAction.action.Enable();
    }

    void OnDisable()
    {
        walkAction.action.Disable();
        interactAction.action.Disable();
    }

    private void Update()
    {
        moveAmount = walkAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if(moveAmount != Vector2.zero)
        {
            Move();
        }
    }

    /// <summary>
    /// Converts the X,Y input into X,Z movement using the rigidbody
    /// </summary>
    private void Move()
    {
        Vector3 moveTarget = m_rigidBody.position;
        moveTarget.x += moveAmount.x * moveSpeed * Time.deltaTime;
        moveTarget.z += moveAmount.y * moveSpeed * Time.deltaTime;
        m_rigidBody.MovePosition(moveTarget);
    }

    void Interact(CallbackContext context)
    {
        Debug.Log("Player wants to interact");
    }
}

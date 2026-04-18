using UnityEngine;
using UnityEngine.InputSystem;

public class VegtablePickup : MonoBehaviour
{
    public Transform onhand;
    private bool isCarried = false;
    private Rigidbody rb;
    private InputAction dropAction;

    void Awake()
    {
        // Tutorial 6 section 7.3 style — create action directly in code
        dropAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Keyboard>/e"
        );
    }

    void OnEnable()
    {
        dropAction.Enable();
        dropAction.performed += OnDrop;
    }

    void OnDisable()
    {
        dropAction.performed -= OnDrop;
        dropAction.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCarried && other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        if (isCarried) Drop();
    }

    void PickUp()
    {
        isCarried = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.SetParent(onhand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isCarried = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public bool IsCarried() { return isCarried; }
}
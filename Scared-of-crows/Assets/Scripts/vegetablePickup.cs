using UnityEngine;
using UnityEngine.InputSystem;

public class vegetablePickup : MonoBehaviour
{
    public Transform onhand;
    public float pickupRange = 2f;
    public float throwForce = 15f;

    private bool isCarried = false;
    private Rigidbody rb;
    private Transform player;
    private Camera mainCam;

    private InputAction pickAction;
    private InputAction throwAction;

    void Awake()
    {
        pickAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/f");
        throwAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        pickAction.Enable();
        throwAction.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCam = Camera.main;
    }

    void Update()
    {
        // pick up
        if (pickAction.triggered && !isCarried && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= pickupRange)
            {
                PickUp();
            }
        }

        // throw
        if (throwAction.triggered && isCarried)
        {
            Throw();
        }
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

    void Throw()
    {
        isCarried = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;

        Ray ray = mainCam.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        Vector3 throwDirection;
        if (Physics.Raycast(ray, out RaycastHit hit))
            throwDirection = (hit.point - transform.position).normalized;
        else
            throwDirection = mainCam.transform.forward;

        rb.linearVelocity = throwDirection * throwForce;
    }

    public void Drop()
    {
        isCarried = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void OnDestroy()
    {
        pickAction.Dispose();
        throwAction.Dispose();
    }

    public bool IsCarried() { return isCarried; }
}
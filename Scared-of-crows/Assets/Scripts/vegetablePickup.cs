using UnityEngine;
using UnityEngine.InputSystem;

public class vegetablePickup : MonoBehaviour
{
    public Transform onhand;
    public float pickupRange = 3f;
    public float throwForce = 25f;

    private bool isCarried = false;
    private Rigidbody rb;
    private Transform player;
    private Camera mainCam;

    private InputAction pickAction;
    private InputAction throwAction;

    private static vegetablePickup currentlyCarried = null;

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
        if (pickAction.triggered && !isCarried
            && player != null && currentlyCarried == null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= pickupRange)
            {
                PickUp();
            }
        }

        if (throwAction.triggered && isCarried)
        {
            Throw();
        }
    }

    void PickUp()
    {
        isCarried = true;
        currentlyCarried = this;
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
        currentlyCarried = null;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;

        // use player forward + upward angle instead of camera
        Vector3 throwDirection = player.forward + Vector3.up * 0.5f;
        throwDirection.Normalize();

        rb.linearVelocity = throwDirection * throwForce;
    }

    public void Drop()
    {
        isCarried = false;
        currentlyCarried = null;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void OnDestroy()
    {
        if (currentlyCarried == this) currentlyCarried = null;
        pickAction.Dispose();
        throwAction.Dispose();
    }

    public bool IsCarried() { return isCarried; }
}
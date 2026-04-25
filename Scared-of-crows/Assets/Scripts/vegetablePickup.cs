using UnityEngine;
using UnityEngine.InputSystem;

public class vegetablePickup : MonoBehaviour
{
    public Transform onhand;
    public float pickupRange = 3f;
    public float throwForce = 60f; 

    private bool isCarried = false;
    private Rigidbody rb;
    private Transform player;
    private Camera mainCam;

    private InputAction pickAction;
    private InputAction dropAction;

    private static vegetablePickup currentlyCarried = null;

    void Awake()
    {
        pickAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/f");
        dropAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/e");
        pickAction.Enable();
        dropAction.Enable();
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

        if (dropAction.triggered && isCarried)
        {
            Drop();
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
        dropAction.Dispose();
    }

    public bool IsCarried() { return isCarried; }
}
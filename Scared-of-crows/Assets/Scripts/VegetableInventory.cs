using UnityEngine;
using UnityEngine.InputSystem;

public class VegetableInventory : MonoBehaviour
{
    public GameObject vegetableThrowablePrefab;
    public Transform onhand;
    public int startingVegetables = 3;
    public float throwForce = 25f;

    private int count;
    private InputAction throwAction;
    private GameObject currentVisual;
    private Camera mainCam;

    void Awake()
    {
        throwAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Mouse>/leftButton"
        );
        throwAction.performed += _ => ThrowVegetable();
        throwAction.Enable();
    }

    void Start()
    {
        count = startingVegetables;
        mainCam = Camera.main;
        ShowVegetableInHand();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowVegetableInHand()
    {
        if (count <= 0) return;

        currentVisual = Instantiate(vegetableThrowablePrefab,
            onhand.position, onhand.rotation);
        currentVisual.transform.SetParent(onhand);
        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

        Rigidbody rb = currentVisual.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void ThrowVegetable()
    {
        if (count <= 0) return;

        if (currentVisual != null)
        {
            currentVisual.transform.SetParent(null);

            currentVisual.transform.position = mainCam.transform.position;

            Rigidbody rb = currentVisual.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                Ray ray = mainCam.ScreenPointToRay(
                    Mouse.current.position.ReadValue()
                );

                Vector3 throwDir;
                if (Physics.Raycast(ray, out RaycastHit hit, 200f))
                {
                    throwDir = (hit.point - currentVisual.transform.position).normalized;
                }
                else
                {
                    throwDir = ray.direction.normalized;
                }

                rb.linearVelocity = throwDir * throwForce;
            }
            currentVisual = null;
        }

        count--;
        if (count > 0) ShowVegetableInHand();
    }

    void OnDestroy()
    {
        throwAction.Dispose();
    }
}
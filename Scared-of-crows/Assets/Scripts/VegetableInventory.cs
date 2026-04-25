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
    private GameObject currentVisual; // vegetable shown in hand

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
        ShowVegetableInHand();
    }

    void ShowVegetableInHand()
    {
        if (count <= 0) return;

        // spawn visual vegetable in hand — frozen, not throwable yet
        currentVisual = Instantiate(vegetableThrowablePrefab,
            onhand.position, onhand.rotation);

        // parent to hand so it follows Pike
        currentVisual.transform.SetParent(onhand);
        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

        // freeze physics while held
        Rigidbody rb = currentVisual.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void ThrowVegetable()
    {
        if (count <= 0)
        {
            Debug.Log("No vegetables left!");
            return;
        }

        // detach current visual and throw it
        if (currentVisual != null)
        {
            currentVisual.transform.SetParent(null);

            Rigidbody rb = currentVisual.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                Vector3 throwDir = transform.forward + Vector3.up * 0.3f;
                throwDir.Normalize();
                rb.linearVelocity = throwDir * throwForce;
            }

            currentVisual = null;
        }

        count--;
        Debug.Log("Vegetables left: " + count);

        // show next vegetable in hand
        if (count > 0)
        {
            ShowVegetableInHand();
        }
    }

    void OnDestroy()
    {
        throwAction.Dispose();
    }
}
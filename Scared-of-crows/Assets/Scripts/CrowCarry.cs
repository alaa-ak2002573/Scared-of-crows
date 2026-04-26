using UnityEngine;

public class CrowCarry : MonoBehaviour
{
    [Header("Setup")]
    public GameObject carriedObject;
    public CrowHealth crow1;
    public CrowHealth crow2;
    public CrowHealth crow3;

    private bool released = false;
    private Rigidbody rb;

    void Start()
    {
        rb = carriedObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    void Update()
    {
        if (released) return;

        if (crow1.IsDead && crow2.IsDead && crow3.IsDead)
        {
            Release();
        }

        // Keep object position between the 3 crows
        Vector3 center = (crow1.transform.position + crow2.transform.position + crow3.transform.position) / 3f;
        carriedObject.transform.position = center + Vector3.down * 5f;
    }

    void Release()
    {
        released = true;
        carriedObject.transform.SetParent(null);
        if (rb != null) rb.isKinematic = false;
    }
}
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    private int vegetableCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vegetablePickup carried = FindFirstObjectByType<vegetablePickup>();
            if (carried != null && carried.IsCarried())
            {
                carried.Drop();
                CountDelivery();
            }
        }

        if (other.CompareTag("Vegetable"))
        {
            vegetablePickup veg = other.GetComponent<vegetablePickup>();
            if (veg != null && !veg.IsCarried())
            {
                CountDelivery();
            }
        }
    }

    void CountDelivery()
    {
        vegetableCount++;
        Debug.Log("Vegetables delivered: " + vegetableCount);

        if (vegetableCount >= 3)
        {
            GameManager.instance.ChangeState(GameManager.GameState.LevelComplete);
        }
    }
}
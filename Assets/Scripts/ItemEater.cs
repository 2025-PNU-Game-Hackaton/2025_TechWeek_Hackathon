using UnityEngine;

public class ItemEater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IItem item;
        if (other.TryGetComponent(out item))
        {
            item.UseItem(gameObject);
        }
    }
}
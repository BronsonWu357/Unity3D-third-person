using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Item[] items;
    [SerializeField] InventoryManager inventoryManager;


    public void add(int index)
    {
        bool x =  inventoryManager.AddItem(items[index],1);
    }


    public void GetSelectedItem()
    {
        Item item = inventoryManager.GetSelectedItem(false);

        if (item != null)
        {
            Debug.Log("Receive");
        }
        else
        {
            Debug.Log("Not ReceiveGI");
        }
    }


    public void UseSelectedItem()
    {
        Item item = inventoryManager.GetSelectedItem(true);

        if (item != null)
        {
            Debug.Log("Used");
        }
        else
        {
            Debug.Log("Not Used");
        }
    }
}

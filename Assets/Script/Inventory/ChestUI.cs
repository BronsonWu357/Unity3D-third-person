using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

    [SerializeField] private GameObject chestUI;

    [SerializeField] private GameObject InventoryItemPrefab;


    public void OpenChest(ChestInvetory chestInvetory)
    {
        if(chestUI.activeSelf == true)
        {
            ExportData(chestInvetory);
        }
        else
        {
            //��������
            int count = 0;
            foreach (int index in chestInvetory.indexList)
            {
                SpawnNewItem(chestInvetory.items[count], slots[index], chestInvetory.countList[count]);

                //ˢ���ı�
                slots[index].GetComponentInChildren<InventoryItem>().RefreshCount();
                count++;
            }

            chestUI.SetActive(true);
        }
    }


    //��������Ʒ
    public void SpawnNewItem(Item item, InventorySlot slot,int count)
    {
        GameObject newItemGameObject = Instantiate(InventoryItemPrefab, slot.transform);

        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();

        inventoryItem.count = count;

        inventoryItem.InitialiseItem(item);
    }


    //�ݻٴ洢���е�����
    public void DestroyInvetoryItem()
    {
        //��ȡ����inventoryItem�Ӷ���
        InventoryItem[] inventoryItems =  chestUI.GetComponentsInChildren<InventoryItem>();

        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            Destroy(inventoryItem.gameObject);
        }
    }


    public void ExportData(ChestInvetory chestInvetory)
    {
        chestInvetory.indexList.Clear();
        chestInvetory.items.Clear();
        chestInvetory.countList.Clear();

        //��������
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.GetComponentInChildren<InventoryItem>() != null)
            {
                Item item = slot.GetComponentInChildren<InventoryItem>().item;

                int itemCount = slot.GetComponentInChildren<InventoryItem>().count;

                chestInvetory.items.Add(item);

                chestInvetory.indexList.Add(count);

                chestInvetory.countList.Add(itemCount);
            }

            count++;
        }

        DestroyInvetoryItem();

        chestUI.SetActive(false);
    }
}

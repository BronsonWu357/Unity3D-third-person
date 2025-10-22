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
            //导入数据
            int count = 0;
            foreach (int index in chestInvetory.indexList)
            {
                SpawnNewItem(chestInvetory.items[count], slots[index], chestInvetory.countList[count]);

                //刷新文本
                slots[index].GetComponentInChildren<InventoryItem>().RefreshCount();
                count++;
            }

            chestUI.SetActive(true);
        }
    }


    //生成新物品
    public void SpawnNewItem(Item item, InventorySlot slot,int count)
    {
        GameObject newItemGameObject = Instantiate(InventoryItemPrefab, slot.transform);

        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();

        inventoryItem.count = count;

        inventoryItem.InitialiseItem(item);
    }


    //摧毁存储槽中的物体
    public void DestroyInvetoryItem()
    {
        //获取所有inventoryItem子对象
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

        //导出数据
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

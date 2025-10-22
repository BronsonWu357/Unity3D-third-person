using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("BackPack")]
    //获取所有背包存储槽
    [SerializeField] private InventorySlot[] slots;

    //存储物品预制件
    [SerializeField] private GameObject InventoryItemPrefab;

    private int IsSelected = -1;

    [SerializeField] private Transform newItemTransform;

    [SerializeField] private float dropForce = 5f;

    [SerializeField] private Vector3 dropOffset = Vector3.zero;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private float cantPickTime;


    public void Awake()
    {
        Instance = this;
    }


    public void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number >= 1 && number <= 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }


        foreach (var slot in slots)
        {
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            if (inventoryItem != null && inventoryItem.isMouseOver && Input.GetMouseButtonDown(1))  // 右键点击 (1 代表鼠标右键)
            {
                Debug.Log("click");

                DropItem(inventoryItem);
            }
        }
    }


    public void ChangeSelectedSlot(int slotsIndex)
    {
        if (IsSelected >= 0)
        {
            slots[IsSelected].Deselect();
        }

        bool isSelected = slots[slotsIndex].Select();

        if (isSelected)
        {
            IsSelected = slotsIndex;
        }
        else
        {
            IsSelected = -1;
        }
    }


    //获取存储槽物品
    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = slots[IsSelected];

        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        //判断是否有物品
        if (itemInSlot != null)
        {
            //判断是否使用
            if (use)
            {
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return itemInSlot.item;
        }

        return null;
    }


    public bool AddItem(Item item,int count)
    {
        foreach (InventorySlot slot in slots)
        {
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            //如果存储槽有物品，物品相等，物品数量小于最大堆叠，物品可堆叠
            if (inventoryItem != null && inventoryItem.item == item 
                 && inventoryItem.item.stackable)
            {
                if(inventoryItem.count + count <= item.maxCount)
                {
                    //物品加一
                    inventoryItem.count += count;

                    //刷新文本
                    inventoryItem.RefreshCount();

                    return true;
                }
                else
                {
                    count = inventoryItem.count + count - item.maxCount;

                    inventoryItem.count = item.maxCount;

                    //刷新文本
                    inventoryItem.RefreshCount();
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            //GetComponentInChildren<>()在当前游戏对象及其所有子对象中搜索指定类型的组件
            //从最上层开始递归查找，只返回第一个找到的组件
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            //如果存储槽没有物品
            if (inventoryItem == null && count <= item.maxCount)
            {
                SpawnNewItem(item, slot);

                inventoryItem = slot.GetComponentInChildren<InventoryItem>();

                inventoryItem.count = count;
                inventoryItem.RefreshCount();
                
                return true;
            }
            else if (inventoryItem == null && count > item.maxCount)
            {
                SpawnNewItem(item, slot);

                inventoryItem = slot.GetComponentInChildren<InventoryItem>();

                inventoryItem.count = item.maxCount;
                inventoryItem.RefreshCount();

                count -= item.maxCount;
            }

        }

        return false;
    }


    //生成新物品
    public void SpawnNewItem(Item item,InventorySlot slot)
    {
        //Instantiate(originalObject, parentTransform) 用于克隆（复制）一个游戏对象，并在场景中创建它的新实例
        //新对象位于parentTransform的子级
        GameObject newItemGameObject = Instantiate(InventoryItemPrefab, slot.transform);

        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();

        inventoryItem.InitialiseItem(item);
    }


    //丢弃物品
    public void DropItem(InventoryItem inventoryItem)
    {
        // 将物品丢弃到世界中，可以使用 Instantiate 创建物品
        GameObject droppedItem = Instantiate(inventoryItem.item.prefab,playerTransform.position + dropOffset, Quaternion.identity, newItemTransform);

        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        StartCoroutine(CantPick(droppedItem.GetComponent<ItemPickUp>()));

        droppedItem.GetComponent<Highlight>().canHighlight = false;


        Vector3 horizontalDir = new Vector3(playerTransform.forward.x, 0f, playerTransform.forward.z).normalized;
        rb.AddForce(horizontalDir * dropForce, ForceMode.Impulse);  // 添加丢弃的力度

        
        inventoryItem.count--;
        inventoryItem.RefreshCount();
    }


    private  IEnumerator CantPick(ItemPickUp itemPickUp)
    {
        float timer = 0f;
        while (timer < cantPickTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        itemPickUp.isPickable = true;
    }



    public bool UseItem(int[] ints,Item item)
    {
        foreach (InventorySlot slot in slots)
        {
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
            if (inventoryItem != null && inventoryItem.item == item)
            {
                if(inventoryItem.count >= ints[1] - ints[0])
                {
                    inventoryItem.count -= (ints[1] - ints[0]);

                    ints[0] = ints[1];

                    inventoryItem.RefreshCount();

                    return true;
                }
                else
                {
                    ints[0] += inventoryItem.count;

                    inventoryItem.count = 0;

                    inventoryItem.RefreshCount();
                }
            }
        }


        return false;
    }
}

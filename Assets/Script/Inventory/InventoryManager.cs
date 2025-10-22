using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("BackPack")]
    //��ȡ���б����洢��
    [SerializeField] private InventorySlot[] slots;

    //�洢��ƷԤ�Ƽ�
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

            if (inventoryItem != null && inventoryItem.isMouseOver && Input.GetMouseButtonDown(1))  // �Ҽ���� (1 ��������Ҽ�)
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


    //��ȡ�洢����Ʒ
    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = slots[IsSelected];

        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        //�ж��Ƿ�����Ʒ
        if (itemInSlot != null)
        {
            //�ж��Ƿ�ʹ��
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

            //����洢������Ʒ����Ʒ��ȣ���Ʒ����С�����ѵ�����Ʒ�ɶѵ�
            if (inventoryItem != null && inventoryItem.item == item 
                 && inventoryItem.item.stackable)
            {
                if(inventoryItem.count + count <= item.maxCount)
                {
                    //��Ʒ��һ
                    inventoryItem.count += count;

                    //ˢ���ı�
                    inventoryItem.RefreshCount();

                    return true;
                }
                else
                {
                    count = inventoryItem.count + count - item.maxCount;

                    inventoryItem.count = item.maxCount;

                    //ˢ���ı�
                    inventoryItem.RefreshCount();
                }
            }
        }

        foreach (InventorySlot slot in slots)
        {
            //GetComponentInChildren<>()�ڵ�ǰ��Ϸ�����������Ӷ���������ָ�����͵����
            //�����ϲ㿪ʼ�ݹ���ң�ֻ���ص�һ���ҵ������
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            //����洢��û����Ʒ
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


    //��������Ʒ
    public void SpawnNewItem(Item item,InventorySlot slot)
    {
        //Instantiate(originalObject, parentTransform) ���ڿ�¡�����ƣ�һ����Ϸ���󣬲��ڳ����д���������ʵ��
        //�¶���λ��parentTransform���Ӽ�
        GameObject newItemGameObject = Instantiate(InventoryItemPrefab, slot.transform);

        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();

        inventoryItem.InitialiseItem(item);
    }


    //������Ʒ
    public void DropItem(InventoryItem inventoryItem)
    {
        // ����Ʒ�����������У�����ʹ�� Instantiate ������Ʒ
        GameObject droppedItem = Instantiate(inventoryItem.item.prefab,playerTransform.position + dropOffset, Quaternion.identity, newItemTransform);

        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        StartCoroutine(CantPick(droppedItem.GetComponent<ItemPickUp>()));

        droppedItem.GetComponent<Highlight>().canHighlight = false;


        Vector3 horizontalDir = new Vector3(playerTransform.forward.x, 0f, playerTransform.forward.z).normalized;
        rb.AddForce(horizontalDir * dropForce, ForceMode.Impulse);  // ��Ӷ���������

        
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

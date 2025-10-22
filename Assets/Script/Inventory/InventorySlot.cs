using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image image;

    [SerializeField] private Color selectedColoer;

    [SerializeField] private Color notSelectedColor;


    public bool Select()
    {
        Transform child = transform.Find("InventoryItem");
        Transform childClone = transform.Find("InventoryItem(Clone)");


        if (child != null || childClone != null)
        {
            if (child != null)
            {
                image = child.GetComponent<Image>();

                image.color = selectedColoer;

                return true;
            }
            else
            {
                image = childClone.GetComponent<Image>();

                image.color = selectedColoer;

                return true;
            }
        }
        else
        {
            return false;
        }
    }


    public void Deselect()
    {
        Transform child = transform.Find("InventoryItem");
        Transform childClone = transform.Find("InventoryItem(Clone)");

        if (child != null || childClone != null)
        {
            if (child != null)
            {
                image = child.GetComponent<Image>();

                image.color = notSelectedColor;
            }
            else
            {
                image = childClone.GetComponent<Image>();

                image.color = notSelectedColor;
            }
        }
    }


    //����ִ��˳��ΪOnBeginDrag()->OnDrop()->OnEndDrag()
    /*��ʼ��ק����OnBeginDrag()���ı�InventoryItem�ĸ���Ϊ��Ŀ¼
     Ȼ����´���OnDrop()����InventoryItem.parentAfterDrag��ֵΪΪ���߼�������transform
     ����ɿ�����OnEndDrag()����InventoryItem�ĸ�����ΪparentAfterDrag*/

    //�ж���ק�����Ƿ��ڵ�ǰ����֮��
    /*  1. �����λ�÷�������
    List<RaycastResult> results = RaycastAll(Input.mousePosition);
    
    2. �������б����߻��е�UIԪ��
    foreach (var result in results)
    {
        GameObject hitObject = result.gameObject;
        
        // 3. �����������Ƿ�ʵ���� IDropHandler
        var dropHandlers = hitObject.GetComponents<IDropHandler>();*/


    //���û���קһ�� UI Ԫ�ز��ͷŵ���ǰ������ʱ�Զ�����
    public void OnDrop(PointerEventData eventData)
    {
        //pointerDrag���Է��ص�ǰ���ڱ���ק�� GameObject
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        InventoryItem itemInSlot = transform?.GetComponentInChildren<InventoryItem>();

        //�жϵ�ǰ����������������Ƿ�Ϊ0
        if (transform.childCount == 0)
        {

            //����ǰ�����transform��ֵ������ק��gameObject
            inventoryItem.parentAfterDrag = transform;
        }
        else if (itemInSlot.item == inventoryItem.item 
            && itemInSlot.count < itemInSlot.item.maxCount)
        {
            itemInSlot.count += inventoryItem.count;

            if (itemInSlot.count > itemInSlot.item.maxCount)
            {
                inventoryItem.count = itemInSlot.count - itemInSlot.item.maxCount;

                itemInSlot.count = itemInSlot.item.maxCount;
            }
            else
            {
                inventoryItem.count = 0;
            }

            itemInSlot.RefreshCount();

            inventoryItem.RefreshCount();
        }
    }
}

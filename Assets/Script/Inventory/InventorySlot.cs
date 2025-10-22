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


    //方法执行顺序为OnBeginDrag()->OnDrop()->OnEndDrag()
    /*开始拖拽触发OnBeginDrag()，改变InventoryItem的父级为根目录
     然后放下触发OnDrop()，将InventoryItem.parentAfterDrag赋值为为射线检测物体的transform
     最后松开触发OnEndDrag()，将InventoryItem的父级设为parentAfterDrag*/

    //判断拖拽物体是否在当前物体之上
    /*  1. 从鼠标位置发射射线
    List<RaycastResult> results = RaycastAll(Input.mousePosition);
    
    2. 遍历所有被射线击中的UI元素
    foreach (var result in results)
    {
        GameObject hitObject = result.gameObject;
        
        // 3. 检查这个物体是否实现了 IDropHandler
        var dropHandlers = hitObject.GetComponents<IDropHandler>();*/


    //当用户拖拽一个 UI 元素并释放到当前物体上时自动调用
    public void OnDrop(PointerEventData eventData)
    {
        //pointerDrag属性返回当前正在被拖拽的 GameObject
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        InventoryItem itemInSlot = transform?.GetComponentInChildren<InventoryItem>();

        //判断当前物体的子物体数量是否为0
        if (transform.childCount == 0)
        {

            //将当前物体的transform赋值给被拖拽的gameObject
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

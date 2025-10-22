using UnityEngine;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isMouseOver = false;


    // 鼠标悬停在物品槽上
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }


    // 鼠标离开物品槽
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}

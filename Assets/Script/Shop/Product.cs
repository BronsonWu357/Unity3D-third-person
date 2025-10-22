using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Product : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public TMP_Text price;

    [SerializeField] public Item item;

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

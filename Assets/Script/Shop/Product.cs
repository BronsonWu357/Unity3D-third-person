using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Product : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public TMP_Text price;

    [SerializeField] public Item item;

    public bool isMouseOver = false;


    // �����ͣ����Ʒ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }


    // ����뿪��Ʒ��
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}

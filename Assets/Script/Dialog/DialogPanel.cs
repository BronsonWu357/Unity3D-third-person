using UnityEngine;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//����ʵ���� IBeginDragHandler �ӿ�ʱ
//unity��ɨ��ɨ������ MonoBehaviour ���
//�������ʵ������Щ�¼��ӿ�
//���ӿ��뷽��ǩ������ƥ��
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler ,IPointerEnterHandler,IPointerExitHandler
{
    [Header("UI")]
    private Image image;

    public Item item;

    //[HideInInspector] - ����,����һ�� Unity ���ԣ����������ǣ��� Unity Inspector ��������������ֶ�,
    //�ֶ���Ȼ�� public �ģ������ڴ����з��ʵ������� Inspector ����ʾ����
    [HideInInspector] public Transform parentAfterDrag;

    [SerializeField] private TMP_Text counText;

    public int count;

    public bool isMouseOver = false;  // �����ж�����Ƿ���ͣ


    public void Awake()
    {
        image = GetComponent<Image>();
    }


    //��ʼ����Ʒ
    public void InitialiseItem(Item newItem)
    {
        item = newItem;

        //����¡����ĸ���IsActiveΪfalseʱ���Ӷ���� Awake() �� Start() ������������ִ��
        //Ҫ���»�ȡ���
        image = GetComponent<Image>();

        image.sprite = item.image;

        RefreshCount();
    }


    public void RefreshCount()
    {
        counText.text = count.ToString();

        //Debug.Log(counText.text);

        //�����������ض���
        counText.gameObject.SetActive(count > 1);

        if (count < 1)
        {
            Destroy(gameObject);
        }
    }


    //PointerEventData��������ָ�������豸����ꡢ�������ʵȣ���ص������¼����ݡ�
    //uniy���Զ�����eventData����
    //��ʼ��ק�ǵ���
    public void OnBeginDrag(PointerEventData eventData)
    {
        /*���߼������
        ���û����������Ļʱ��
        EventSystem ����һ������
        ���߼������������ raycastTarget = true �� UI Ԫ��
        �ҵ����ϲ�Ŀɽ���Ԫ��
        ������Ӧ���¼����������ק�ȣ�*/


        //raycastTarget������� UI Ԫ���Ƿ���� Unity ���¼����߼��ϵͳ
        image.raycastTarget = false;// ��ʼ��ק������������

        //��ȡ��ǰ�������ڵĸ��� Transform
        //��¼��ǰ����
        parentAfterDrag = transform.parent;

        //transform.root ���ص��ǳ��������ĸ�����
        //���������ֱ�ӳ�Ϊ��㸸����������壨��Canvas֮�£���panelͬһ����
        transform.SetParent(transform.root);
    }


    //��ק��
    public void OnDrag(PointerEventData eventData)
    {
        //�����λ�ø�ֵ����ǰ����ʵ�ָ���
        transform.position = Input.mousePosition;
    }


    //��ק���
    public void OnEndDrag(PointerEventData eventData)
    {
        // �ָ���������
        //���ڿ����ٴα��������ק
        image.raycastTarget = true;

        //�ص�֮ǰ����֮��
        transform.SetParent(parentAfterDrag);
    }


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

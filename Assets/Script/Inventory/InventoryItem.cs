using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//当类实现了 IBeginDragHandler 接口时
//unity会扫描扫描所有 MonoBehaviour 组件
//检查它们实现了哪些事件接口
//将接口与方法签名进行匹配
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler ,IPointerEnterHandler,IPointerExitHandler
{
    [Header("UI")]
    private Image image;

    public Item item;

    //[HideInInspector] - 特性,这是一个 Unity 特性，它的作用是：在 Unity Inspector 窗口中隐藏这个字段,
    //字段仍然是 public 的，可以在代码中访问但不会在 Inspector 中显示出来
    [HideInInspector] public Transform parentAfterDrag;

    [SerializeField] private TMP_Text counText;

    public int count;

    public bool isMouseOver = false;  // 用于判断鼠标是否悬停


    public void Awake()
    {
        image = GetComponent<Image>();
    }


    //初始化物品
    public void InitialiseItem(Item newItem)
    {
        item = newItem;

        //当克隆对象的父级IsActive为false时，子对象的 Awake() 和 Start() 方法不会立即执行
        //要重新获取组件
        image = GetComponent<Image>();

        image.sprite = item.image;

        RefreshCount();
    }


    public void RefreshCount()
    {
        counText.text = count.ToString();

        //Debug.Log(counText.text);

        //根据数量隐藏对象
        counText.gameObject.SetActive(count > 1);

        if (count < 1)
        {
            Destroy(gameObject);
        }
    }


    //PointerEventData包含了与指针输入设备（鼠标、触摸、笔等）相关的所有事件数据。
    //uniy会自动传入eventData参数
    //开始拖拽是调用
    public void OnBeginDrag(PointerEventData eventData)
    {
        /*射线检测流程
        当用户点击或触摸屏幕时：
        EventSystem 发射一条射线
        射线检测所有设置了 raycastTarget = true 的 UI 元素
        找到最上层的可交互元素
        触发相应的事件（点击、拖拽等）*/


        //raycastTarget决定这个 UI 元素是否参与 Unity 的事件射线检测系统
        image.raycastTarget = false;// 开始拖拽：禁用自身检测

        //获取当前物体所在的父级 Transform
        //纪录当前父级
        parentAfterDrag = transform.parent;

        //transform.root 返回的是场景中最顶层的父物体
        //让这个物体直接成为最顶层父物体的子物体（即Canvas之下，和panel同一级）
        transform.SetParent(transform.root);
    }


    //拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        //将鼠标位置赋值给当前对象，实现跟随
        transform.position = Input.mousePosition;
    }


    //拖拽完成
    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢复交互能力
        //现在可以再次被点击和拖拽
        image.raycastTarget = true;

        //回到之前父级之下
        transform.SetParent(parentAfterDrag);
    }


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

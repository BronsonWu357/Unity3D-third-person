using Controller;
using Game.CameraSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private LayerMask pickableLayerMask;

    [SerializeField] private Transform palyerCameraTransform;

    [SerializeField] private GameObject pickUpUI;

    [SerializeField][Min(1)] private float hitRange = 3;

    private Item item;

    [SerializeField] InventoryManager inventoryManager;

    //互动后向上的加速度
    [SerializeField] private float launchForce = 5f;

    /*RaycastHit用于存储射线检测（Raycast）的结果。
包含：
collider：碰撞到的物体的碰撞体
point：射线击中的世界坐标点
normal：碰撞点表面的法线
distance：射线起点到碰撞点的距离*/
    private RaycastHit hit;


    [Header("Chest")]
    [SerializeField] private ChestUI chestUIClass;

    [SerializeField] private BackPackInventory backPackInventory;


    [Header("Move and Rotation")]
    [SerializeField] private PlayerController playerController;

    [SerializeField] private CameraController cameraController;


    [Header("Dialog")]
    [SerializeField] private DialogueManager dialogueManager;

    private bool canDetect = true;



    public void Update()
    {
        InteractiveTrigger();

        if (canDetect)
        {
            InteractiveDetection();
        }
    }


    //射线检测互动，高亮显示等
    public void InteractiveDetection()
    {
        Vector3 origin = palyerCameraTransform.position;
        Vector3 direction = palyerCameraTransform.forward;
        //用来做射线检测，检测它是否击中场景中的碰撞体（Collider）
        //out 是 C# 的一个关键字，意思是“输出参数”
        if (Physics.Raycast(origin, direction, out hit, hitRange, pickableLayerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pickable"))
            {
                Highlight highlight = hit.collider?.GetComponent<Highlight>();
                if (highlight != null && highlight.canHighlight == true)
                {
                    highlight.ToggleHighlight(true);

                    pickUpUI.SetActive(true);

                    SetPickUpUI("Press E");
                }
            }

            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Chest"))
            {
                pickUpUI.SetActive(true);

                SetPickUpUI("Open Chest");
            }

            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
            {
                pickUpUI.SetActive(true);

                SetPickUpUI("Talk to Garrosh");
            }
        }
        else
        {
            pickUpUI.SetActive(false);
        }
    }


    //互动触发
    public void InteractiveTrigger()
    {
        if (hit.collider != null)
        {
            //错误写法GameObject gameObject = hit.collider.GetComponent<GameObject>();
            GameObject gameObject = hit.collider.gameObject;

            //?.是 C# 的“空条件运算符”
            //如果左边对象是 null，就直接返回 null，而不会继续执行后面的成员访问或方法调用。
            gameObject.GetComponent<Highlight>()?.ToggleHighlight(false);

            ItemPickUp itemPickUp = gameObject.GetComponent<ItemPickUp>();
            item = itemPickUp?.itemData;

            if (Input.GetButtonDown("PickUp"))
            {
                //开箱子
                if (gameObject.CompareTag("Chest") && backPackInventory.isOpened == false)
                {
                    chestUIClass.OpenChest(gameObject.GetComponent<ChestInvetory>());

                    backPackInventory.OpenBackPack();
                    backPackInventory.canInteracte = !backPackInventory.canInteracte;

                    playerController.canMove = !playerController.canMove;
                }


                //拾取物品
                else if (gameObject.CompareTag("Item"))
                {

                    //当不可拾取时，将它变为可拾取，并且赋予刚体重力
                    if (!gameObject.GetComponent<ItemPickUp>().isPickable)
                    {
                        Rigidbody rb = gameObject?.GetComponent<Rigidbody>();

                        rb.isKinematic = false;

                        rb.useGravity = true;

                        rb.linearVelocity = Vector3.up * launchForce;

                        gameObject.GetComponent<ItemPickUp>().isPickable = true;

                        gameObject.GetComponent<Highlight>().canHighlight = false;
                    }
                    /*                    else
                                        {
                                            int count = gameObject.GetComponent<ItemPickUp>().count;

                                            inventoryManager.AddItem(item, count);

                                            Destroy(hit.collider.gameObject);
                                        }*/
                }


                else if (gameObject.CompareTag("NPC"))
                {
                    pickUpUI.SetActive(false); // 隐藏交互提示
                    canDetect = false;         // 暂停射线检测

                    dialogueManager.StartDialogue();
                }
            }
        }
    }


    public void SetPickUpUI(string str)
    {
        TMP_Text text = pickUpUI.GetComponentInChildren<TMP_Text>();

        text.text = str;
    }


    public void SetCanDetect(bool x)
    {
        canDetect = x; 
    }
}

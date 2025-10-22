using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [SerializeField] private PlayerInteraction playerInteraction;

    [SerializeField] private PlayerController playerController;

    [Header("Dialogur")]
    //Queue<>是队列，先进先出
    private Queue<DialogueLine> sentences;

    [SerializeField] private Dialogue dialogue;

    [SerializeField] private GameObject dialogUI;

    [SerializeField] private CursorController cursorController;

    [SerializeField] private GameObject choiceButtons;


    [Header("Shop")]
    [SerializeField] private ShopManager shopManager;


    [Header("Task")]
    [SerializeField] private GameObject taskUI;

    [SerializeField] private List<Item> TaskItems;

    [SerializeField] private List<TMP_Text> itemTexts;

    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private Item rewardItem1;

    private bool canClick = true;



    void Start()
    {
        sentences = new Queue<DialogueLine>();

        // 确保开始时按钮是隐藏的
        choiceButtons.SetActive(false);
    }

    private void Update()
    {
        DialogPanel dialogPanel = dialogUI.GetComponent<DialogPanel>();

        if (dialogPanel.isMouseOver && Input.GetMouseButtonDown(0) && canClick)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue()
    {
        if (dialogUI.activeSelf == false)
        {
            sentences.Clear();

            dialogUI.SetActive(true);

            cursorController.ShowCursor();

            playerController.canMove = false;

            foreach (DialogueLine line in dialogue.lines)
            {
                ////在尾部添加一个sentence
                sentences.Enqueue(line);
            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 3)
        {
            canClick = false;

            StopAllCoroutines();

            choiceButtons.SetActive(true);
            //显示名字
            DialogueLine buttonline = sentences.Dequeue();
            nameText.text = buttonline.speakerName;

            dialogueText.text = buttonline.content;
        }
        else if (sentences.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            //从头部取出一个sentence
            DialogueLine line = sentences.Dequeue();
            nameText.text = line.speakerName;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(line.content));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f); // 打字机效果
        }
    }


    void EndDialogue()
    {
        dialogUI.SetActive(false);

        DialogPanel dialogPanel = dialogUI.GetComponent<DialogPanel>();
        dialogPanel.isMouseOver = false;

        cursorController.ShowCursor();

        playerController.canMove = true;

        playerInteraction.SetCanDetect(true);
    }


    //接受任务
    public void OnAccept()
    {
        taskUI.SetActive(true);

        dialogUI.SetActive(false);
    }


    //打开商店
    public void Shopping()
    {
        shopManager.OpenShop();

        dialogUI.SetActive(false);
    }


    //退出对话
    public void ExitDialogue()
    {
        choiceButtons.SetActive(false);

        sentences.Dequeue();

        DisplayNextSentence();

        canClick = true;
    }


    public void DeliverItems()
    {
        bool[] bools = new bool[TaskItems.Count];

        for (int i = 0; i < TaskItems.Count; i++)
        {
            Item item = TaskItems[i];
            TMP_Text itemText = itemTexts[i];
            string text = itemText.text;

            int[] ints = StringChangeInt(text);

            bools[i] = inventoryManager.UseItem(ints, item);

            //string.Join(" / ", numbers) 会把数组中的每个元素转成字符串，并用 " / " 作为分隔符连接起来。
            itemText.text = string.Join(" / ", ints);
        }

        if (!bools.Contains(false))
        {
            choiceButtons.SetActive(false);

            DisplayNextSentence();

            taskUI.SetActive(false);

            dialogUI.SetActive(true);

            canClick = true;

            //添加物品
            inventoryManager.AddItem(rewardItem1,1);
        }
    }


    public int[] StringChangeInt(string str)
    {

        //将字符串将字符串转化为数组
        //Split('/'),将字符串根据/隔开
        string[] strs = str.Split('/');

        //.Select()对集合中的每个元素执行一个“映射（转换）操作”，并返回一个新的 集合（注意，是“新集合”）。

        //而执行的操作就是s => int.Parse(s.Trim())
        //=>作用是定义一个 匿名函数（小函数），写法为：输入参数 => 输出结果
        //例如x => x * 2，意思是：“传入一个 x，返回 x * 2。”

        //s.Trim()去掉空格
        //int.Parse()转化为整数
        /* string s = "1 ";           // 示例
           s.Trim () => "1"            // 去掉空格
           int.Parse("1") => 1        // 转换成整数*/

        //返回一个 IEnumerable<int> 可迭代对象
        IEnumerable<int> intsIEnumerable = strs.Select(s => int.Parse(s.Trim()));

        //将它转化为一个真正的数组
        int[] ints = intsIEnumerable.ToArray();

        return ints;
    }
}


[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string content;
}


[System.Serializable]
public class Dialogue
{
    public string dialogueName;
    public List<DialogueLine> lines;
}


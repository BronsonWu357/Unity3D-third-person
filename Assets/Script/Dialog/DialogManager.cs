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
    //Queue<>�Ƕ��У��Ƚ��ȳ�
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

        // ȷ����ʼʱ��ť�����ص�
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
                ////��β�����һ��sentence
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
            //��ʾ����
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
            //��ͷ��ȡ��һ��sentence
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
            yield return new WaitForSeconds(0.02f); // ���ֻ�Ч��
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


    //��������
    public void OnAccept()
    {
        taskUI.SetActive(true);

        dialogUI.SetActive(false);
    }


    //���̵�
    public void Shopping()
    {
        shopManager.OpenShop();

        dialogUI.SetActive(false);
    }


    //�˳��Ի�
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

            //string.Join(" / ", numbers) ��������е�ÿ��Ԫ��ת���ַ��������� " / " ��Ϊ�ָ�������������
            itemText.text = string.Join(" / ", ints);
        }

        if (!bools.Contains(false))
        {
            choiceButtons.SetActive(false);

            DisplayNextSentence();

            taskUI.SetActive(false);

            dialogUI.SetActive(true);

            canClick = true;

            //�����Ʒ
            inventoryManager.AddItem(rewardItem1,1);
        }
    }


    public int[] StringChangeInt(string str)
    {

        //���ַ������ַ���ת��Ϊ����
        //Split('/'),���ַ�������/����
        string[] strs = str.Split('/');

        //.Select()�Լ����е�ÿ��Ԫ��ִ��һ����ӳ�䣨ת������������������һ���µ� ���ϣ�ע�⣬�ǡ��¼��ϡ�����

        //��ִ�еĲ�������s => int.Parse(s.Trim())
        //=>�����Ƕ���һ�� ����������С��������д��Ϊ��������� => ������
        //����x => x * 2����˼�ǣ�������һ�� x������ x * 2����

        //s.Trim()ȥ���ո�
        //int.Parse()ת��Ϊ����
        /* string s = "1 ";           // ʾ��
           s.Trim () => "1"            // ȥ���ո�
           int.Parse("1") => 1        // ת��������*/

        //����һ�� IEnumerable<int> �ɵ�������
        IEnumerable<int> intsIEnumerable = strs.Select(s => int.Parse(s.Trim()));

        //����ת��Ϊһ������������
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


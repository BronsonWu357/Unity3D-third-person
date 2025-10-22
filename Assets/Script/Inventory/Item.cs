using System;
using UnityEngine;
using UnityEngine.Tilemaps;


//��������������� Unity �༭���� Create �˵� �д���һ���µĲ˵���������ɸ� ScriptableObject ���ʲ��ļ�
//menuName = "Scriptable object/Item" - ָ���� Create �˵��е�·��
[CreateAssetMenu(menuName ="Scriptable object/Item")]

//�̳��� Unity �� ScriptableObject ����
//���������õ������ʲ�,�����������߼�����
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    //TileBase �� Unity ��������Ƭ���͵Ļ���
    //���Դ洢�κ����͵���Ƭ
    [SerializeField] private TileBase tile;

    [SerializeField] private ItemType itemType;

    [SerializeField] private ActionType actiontype;

    //��ά������������
    [SerializeField] private Vector2Int range = new Vector2Int(5,4);

    [SerializeField] public GameObject prefab;
 

    [Header("Only UI")]
    //�Ƿ�ѵ�
    [SerializeField] public bool stackable = true;

    //�ѵ�����
    [SerializeField] public int maxCount = 20;


    [Header("Both")]
    //Sprite �� Unity ������ 2D ͼ�� �ĺ����࣬ר��������ʾ 2D ͼ��
    //��Image���У�Ҳ��Sprite�ֶ�
    [SerializeField] public Sprite image;


    public enum ItemType
    {
        BuildingBlock,
        Tool,
        Food
    }

    public enum ActionType
    {
        Dig,
        Mine,//�ڿ�
        Eat
    }
}

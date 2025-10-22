using System;
using UnityEngine;
using UnityEngine.Tilemaps;


//这个特性允许你在 Unity 编辑器的 Create 菜单 中创建一个新的菜单项，用于生成该 ScriptableObject 的资产文件
//menuName = "Scriptable object/Item" - 指定在 Create 菜单中的路径
[CreateAssetMenu(menuName ="Scriptable object/Item")]

//继承自 Unity 的 ScriptableObject 基类
//创建可配置的数据资产,并且数据与逻辑分离
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    //TileBase 是 Unity 中所有瓦片类型的基类
    //可以存储任何类型的瓦片
    [SerializeField] private TileBase tile;

    [SerializeField] private ItemType itemType;

    [SerializeField] private ActionType actiontype;

    //二维整数向量对象
    [SerializeField] private Vector2Int range = new Vector2Int(5,4);

    [SerializeField] public GameObject prefab;
 

    [Header("Only UI")]
    //是否堆叠
    [SerializeField] public bool stackable = true;

    //堆叠数量
    [SerializeField] public int maxCount = 20;


    [Header("Both")]
    //Sprite 是 Unity 中用于 2D 图形 的核心类，专门用来显示 2D 图像
    //在Image类中，也有Sprite字段
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
        Mine,//挖矿
        Eat
    }
}

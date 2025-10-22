using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<Product> products;

    [SerializeField] private List<Item> items;

    private int productListIndex = 0;

    [SerializeField] private GameObject buyPanel;

    [SerializeField] private TMP_InputField buyCountText;

    [SerializeField] private Vector3 panelOffset;

    [SerializeField] private InventoryManager inventoryManager;

    private Item item;

    [SerializeField] private GameObject shopUI;


    public void Update()
    {
        foreach (Product product in products)
        {

            if (product.isMouseOver && Input.GetMouseButtonDown(1) && buyPanel.activeSelf == false)  // 右键点击 (1 代表鼠标右键)
            {
                OpenBuyPanel();

                item = product.item;

                break;
            }
            //点击任何地方可关闭面板
            else if (Input.GetMouseButtonDown(1) && buyPanel.activeSelf == true)
            {
                OpenBuyPanel();

                break;
            }
        }
    }


    public void NextPage()
    {
        if (productListIndex < items.Count - 1)
        {
            productListIndex++;

            RefreshSlot();
        }
    }


    public void LastPage()
    {
        if (productListIndex > 0)
        {
            productListIndex--;

            RefreshSlot();
        }
    }


    public void RefreshSlot()
    {
        foreach (Product product in products)
        {
            product.item = items[productListIndex];

            product.GetComponent<Image>().sprite = product.item.image;
        }
    }


    public void OpenBuyPanel()
    {
        buyPanel.SetActive(!buyPanel.activeSelf);

        buyPanel.transform.position = Input.mousePosition + panelOffset;
    }


    public void AddBuyCount()
    {
        int count;
        if (int.TryParse(buyCountText.text, out count))
        {
            // 比如让 count 加 1 再显示回输入框
            count++;
            buyCountText.text = count.ToString();
        }
    }


    public void ReduceBuyCount()
    {
        int count;
        if (int.TryParse(buyCountText.text, out count))
        {
            // 比如让 count 加 1 再显示回输入框
            count--;
            buyCountText.text = count.ToString();
        }
    }


    public void buyButton()
    {
        int count;
        if (int.TryParse(buyCountText.text, out count))
        {
            inventoryManager.AddItem(item, count);
        }
    }


    public void OpenShop()
    {
        if (shopUI.activeSelf == false)
        {
            shopUI.SetActive(true);
        }
    }
}

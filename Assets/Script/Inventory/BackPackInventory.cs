using Game.CameraSystem;
using UnityEngine;
using UnityEngine.UI;

public class BackPackInventory : MonoBehaviour
{
    [SerializeField] private GameObject mainInventory;

    [SerializeField] private Image toolBarBG;

    [SerializeField] private GameObject toolBarText;

    [SerializeField] private CursorController cursorController;

    public bool canInteracte = true;
    public bool isOpened = false;


    public void Update()
    {
        if (Input.GetButtonDown("BackPack") && canInteracte)
        {
            isOpened = !isOpened;

            OpenBackPack();
        }
    }


    public void OpenBackPack()
    {
        mainInventory.SetActive(!mainInventory.activeSelf);

        toolBarBG.enabled = !toolBarBG.IsActive();

        toolBarText.SetActive(!toolBarText.activeSelf);

        cursorController.ShowCursor();
    }
}

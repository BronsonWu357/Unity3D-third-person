using Game.CameraSystem;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private CombatController combatController;

    [SerializeField] private CameraController cameraController;


    public void ShowCursor()
    {
        combatController.canInput = !combatController.canInput;

        cameraController.canRotate = !cameraController.canRotate;


        if (Cursor.visible)
        {
            // 显示鼠标光标
            Cursor.visible = false;

            // 解锁光标（让光标可以自由移动，不限制在游戏窗口内）
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // 显示鼠标光标
            Cursor.visible = true;

            // 解锁光标（让光标可以自由移动，不限制在游戏窗口内）
            Cursor.lockState = CursorLockMode.None;
        }
        /*            // 显示鼠标光标
                    Cursor.visible = true;

                    // 解锁光标（让光标可以自由移动，不限制在游戏窗口内）
                    Cursor.lockState = CursorLockMode.None;*/
    }
}

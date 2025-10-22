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
            // ��ʾ�����
            Cursor.visible = false;

            // ������꣨�ù����������ƶ�������������Ϸ�����ڣ�
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // ��ʾ�����
            Cursor.visible = true;

            // ������꣨�ù����������ƶ�������������Ϸ�����ڣ�
            Cursor.lockState = CursorLockMode.None;
        }
        /*            // ��ʾ�����
                    Cursor.visible = true;

                    // ������꣨�ù����������ƶ�������������Ϸ�����ڣ�
                    Cursor.lockState = CursorLockMode.None;*/
    }
}

using System.Collections;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] public Item itemData; // ���� ScriptableObject

    [SerializeField] public int count = 1;

    [SerializeField] public bool isPickable = false;

    [SerializeField] private float flySpeed = 8f;

    [SerializeField] private Vector3 playerPositionOffset = Vector3.zero;

    [SerializeField] private bool isFlyToPlayer = false;


    //ÿ֡����Ƿ���Ŀ���������ڲ�
    private void OnTriggerStay(Collider other)
    {
        // �жϽ��봥�����������
        if (other.CompareTag("Player") && isPickable == true && isFlyToPlayer == false)
        {
            StartCoroutine(FlyToPlayer(other.transform));
        }
    }


    private IEnumerator FlyToPlayer(Transform player)
    {
        isFlyToPlayer = true;

        /*        ref ���������ǡ����á���
        void ChangeValue(ref float x)
                {
                    x = 10;
                }

                float a = 5;
                ChangeValue(ref a);
                Debug.Log(a); // �����10�����ˣ���*/
        while (Vector3.Distance(transform.position, player.position + playerPositionOffset) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position + playerPositionOffset, flySpeed * Time.deltaTime);
            yield return null;
        }

        // ȷ�����������ʰȡ
        InventoryManager.Instance.AddItem(itemData, count);
        Destroy(gameObject);

        isFlyToPlayer = false;
    }
}

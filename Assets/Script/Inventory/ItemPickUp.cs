using System.Collections;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] public Item itemData; // 引用 ScriptableObject

    [SerializeField] public int count = 1;

    [SerializeField] public bool isPickable = false;

    [SerializeField] private float flySpeed = 8f;

    [SerializeField] private Vector3 playerPositionOffset = Vector3.zero;

    [SerializeField] private bool isFlyToPlayer = false;


    //每帧检测是否有目标在区域内部
    private void OnTriggerStay(Collider other)
    {
        // 判断进入触发区的是玩家
        if (other.CompareTag("Player") && isPickable == true && isFlyToPlayer == false)
        {
            StartCoroutine(FlyToPlayer(other.transform));
        }
    }


    private IEnumerator FlyToPlayer(Transform player)
    {
        isFlyToPlayer = true;

        /*        ref 参数传的是“引用”：
        void ChangeValue(ref float x)
                {
                    x = 10;
                }

                float a = 5;
                ChangeValue(ref a);
                Debug.Log(a); // 结果：10（变了！）*/
        while (Vector3.Distance(transform.position, player.position + playerPositionOffset) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position + playerPositionOffset, flySpeed * Time.deltaTime);
            yield return null;
        }

        // 确保贴近玩家再拾取
        InventoryManager.Instance.AddItem(itemData, count);
        Destroy(gameObject);

        isFlyToPlayer = false;
    }
}

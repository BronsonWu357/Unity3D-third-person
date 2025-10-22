using System.Collections.Generic;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    //��Ұ�ڵ�Ŀ��
    public List<MeleeFighter> TargetInRange {  get; private set; } = new List<MeleeFighter>();

    //��Ұ�Ƕ�
    [SerializeField] public float FieldOfView = 180f;

    //Ŀ����� 
    public MeleeFighter Target { get; set; }

    //����������
    public void OnTriggerEnter(Collider other)
    {
        TargetInRange.Add(other.GetComponent<MeleeFighter>());
    }


    //�������뿪
    public void OnTriggerExit(Collider other)
    {
        TargetInRange.Remove(other.GetComponent<MeleeFighter>());
    }

    
}

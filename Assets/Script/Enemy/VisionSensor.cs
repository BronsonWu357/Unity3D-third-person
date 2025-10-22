using System.Collections.Generic;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    //视野内的目标
    public List<MeleeFighter> TargetInRange {  get; private set; } = new List<MeleeFighter>();

    //视野角度
    [SerializeField] public float FieldOfView = 180f;

    //目标对象 
    public MeleeFighter Target { get; set; }

    //触发器进入
    public void OnTriggerEnter(Collider other)
    {
        TargetInRange.Add(other.GetComponent<MeleeFighter>());
    }


    //触发器离开
    public void OnTriggerExit(Collider other)
    {
        TargetInRange.Remove(other.GetComponent<MeleeFighter>());
    }

    
}

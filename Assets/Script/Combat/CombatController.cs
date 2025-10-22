using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeleeFighter meleeFighter;

    PlayerController playerController;

    public bool canInput = true;

    public void Awake()
    {
        meleeFighter = GetComponent<MeleeFighter>();
        playerController = GetComponent<PlayerController>();
    }


    public void Update()
    {
        if (Input.GetButton("Attack") && canInput)
        {
            meleeFighter.TryToAttack();

            transform.rotation = playerController.targetRotation;
        }
    }
}

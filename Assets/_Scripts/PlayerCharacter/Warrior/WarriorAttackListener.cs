using UnityEngine;

public class WarriorAttackListener : MonoBehaviour
{

    private WarriorMovement AttackManager;


    void Start()
    {
        AttackManager = GetComponent<WarriorMovement>();
    }

    public void AttackEvent()
    {
        AttackManager.CheckAttackPhase();
    }
}

using UnityEngine;

//This script is used for controlling player model changes
public class PlayerModel : MonoBehaviour
{
    [Header("Settings")]
    public int PlayerModelStats = 0;//Player Model Stats 0=Standing;1=Crouching; 2=Jumping;

    [Header("References")]
    public GameObject StandModel, CrouchModel, JumpModel, AttackModel;

    public void ChangePlayerModelStats()
    {
        if (PlayerModelStats == 0)
        {
            StandModel.SetActive(true);
            CrouchModel.SetActive(false);
            JumpModel.SetActive(false);
            AttackModel.SetActive(false);
        }
        if (PlayerModelStats == 1)
        {
            StandModel.SetActive(false);
            CrouchModel.SetActive(true);
            JumpModel.SetActive(false);
            AttackModel.SetActive(false);
        }
        if (PlayerModelStats == 2)
        {
            StandModel.SetActive(false);
            CrouchModel.SetActive(false);
            JumpModel.SetActive(true);
            AttackModel.SetActive(false);
        }
        if (PlayerModelStats == 3)
        {
            StandModel.SetActive(false);
            CrouchModel.SetActive(false);
            JumpModel.SetActive(false);
            AttackModel.SetActive(true);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StandModel = transform.GetChild(0).gameObject;
        CrouchModel = transform.GetChild(1).gameObject;
        JumpModel = transform.GetChild(2).gameObject;
        AttackModel = transform.GetChild(3).gameObject;
    }
}

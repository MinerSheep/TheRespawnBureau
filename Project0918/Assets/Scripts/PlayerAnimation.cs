using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController pC;
    private Rigidbody2D rB;
    public Animator AT;
    public float Playerscale = 1.5f;
    public int direction = 1; //1 means facing right while 0 means facing left
    public GameObject PlayerModel;

    private void Turn()
    {
        if (rB.linearVelocityX < -0.5f)
        {
            PlayerModel.transform.localScale = new Vector3(Playerscale, Playerscale, 1);
            direction = 0;
        }
        else if (rB.linearVelocityX > 0.5f)
        {
            PlayerModel.transform.localScale = new Vector3(-Playerscale, Playerscale, 1);
            direction = 1;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pC=GetComponent<PlayerController>();
        rB = GetComponent<Rigidbody2D>();
    }

    public void UpdateStatus()
    {
        if(pC.Jumping)
        {
            AT.SetInteger("PlayerState", 3);
        }
        else if(pC.Crouching)
        {
            AT.SetInteger("PlayerState", 2);
        }
        else if (rB.linearVelocityX > 0.5f || rB.linearVelocityX < -0.5f)
        {
            AT.SetInteger("PlayerState", 1);
        }
        else
        {
            AT.SetInteger("PlayerState", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
        UpdateStatus();
    }
}

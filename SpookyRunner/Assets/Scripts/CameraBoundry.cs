using UnityEngine;

public class CameraBoundry : MonoBehaviour
{
    public float LowerBoundry = 2;
    public bool CheckLower = true;
    public float UpperBoundry = 5;
    public bool CheckUpper = false;

    // Update is called once per frame
    void Update()
    {
        if(CheckLower&&transform.position.y < LowerBoundry)
        {
            transform.position=new Vector2(transform.position.x,LowerBoundry);
        }
        if(CheckUpper&&transform.position.y > UpperBoundry)
        {
            transform.position = new Vector2(transform.position.x, UpperBoundry);
        }
    }
}

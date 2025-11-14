using UnityEngine;

public class HeadTrigger : MonoBehaviour
{
    public bool IsTriggering = false;
    [HideInInspector] private int triggercount = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggercount++;
        if (triggercount > 0)
        {
            IsTriggering = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        triggercount--;
        if(triggercount<=0)
        {
            IsTriggering = false;
        }
    }
}

using UnityEngine;

public class TouchSimulation : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        //when you start touching the screen
        if(Input.GetMouseButtonDown(0))
        {
            SimulateTouch(Input.mousePosition, TouchPhase.Began);
        }
        //when you move your finger on the screen
        if(Input.GetMouseButton(0))
        {
            SimulateTouch(Input.mousePosition, TouchPhase.Moved);
        }
        //when you remove your finger from the screen
        if (Input.GetMouseButtonUp(0))
        {
            SimulateTouch(Input.mousePosition, TouchPhase.Ended);
        }
    }

    void SimulateTouch(Vector3 position, TouchPhase phase)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));

        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("We hit {hit.collider.name}");
        }
    }
}

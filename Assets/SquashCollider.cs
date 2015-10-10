using UnityEngine;
using System.Collections;

public class SquashCollider : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var normal = hit.normal;

        if (hit.gameObject.tag == "Player")
        {
            if (hit.normal.y > 0.5) 
            {
                Debug.Log("test");
            }
        }
    }
}

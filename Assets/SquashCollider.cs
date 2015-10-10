using UnityEngine;
using System.Collections;

public class SquashCollider : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var normal = hit.normal;

         Debug.Log("test");
        
    }
}

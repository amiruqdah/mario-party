using UnityEngine;
using System.Collections;

public class SquashCollider : MonoBehaviour
{

    // Get collider 

    // Get thing that coolides and basically go and type that happens have for hting
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
    }
}

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SquashCollider : MonoBehaviour
{
    public AudioClip death_sound;
    public Mesh death_frame;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var normal = hit.normal;

        if (hit.gameObject.tag == "Death")
        {
            this.GetComponent<MeshFilter>().mesh = death_frame;
            this.gameObject.transform.DOMove(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 12), 12f, false);
            Destroy(hit.gameObject);
        }

        if (hit.gameObject.tag == "Player")
        {
            if (hit.normal.y > 0.5) 
            {
                Destroy(hit.gameObject);
            }
        }
    }
}

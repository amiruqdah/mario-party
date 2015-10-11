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
            this.gameObject.GetComponent<MeshFilter>().mesh = death_frame;
            Destroy(this.GetComponent<Movement>());
            this.gameObject.GetComponent<MeshFilter>().mesh = death_frame;
            this.GetComponent<AudioSource>().PlayOneShot(death_sound);
            this.gameObject.GetComponent<MeshFilter>().mesh = death_frame;
            this.gameObject.transform.DOMove(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 2f), 12f, false);
        }

        if (hit.gameObject.tag == "Mario" || hit.gameObject.tag == "Luigi" || hit.gameObject.tag == "PurpleMario" || hit.gameObject.tag == "YellowLuigi")
        {
            if (hit.normal.y > 0.5) 
            {

                this.GetComponent<AudioSource>().PlayOneShot(death_sound);
                Destroy(hit.gameObject);
            }
        }
    }
}

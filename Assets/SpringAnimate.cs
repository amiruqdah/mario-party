using UnityEngine;
using System.Collections;

public class SpringAnimate : MonoBehaviour {
    public Mesh[] frames;

    public void AnimateSpring(float delay)
    {
        StartCoroutine("PlaySpring", delay);
    }

    private IEnumerator PlaySpring(float delay)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            this.gameObject.GetComponent<MeshFilter>().mesh = frames[i];
            yield return new WaitForSeconds(delay);
        }
        this.gameObject.GetComponent<MeshFilter>().mesh = frames[0];
    }
}

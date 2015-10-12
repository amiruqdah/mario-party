using UnityEngine;
using System.Collections;

public class SpringEnabler : MonoBehaviour {

    public IEnumerator PlaySpring()
    {
        this.enabled = true;
        yield return new WaitForSeconds(this.GetComponent<GeneralAnimation>().period);
        this.enabled = false;
    }
}

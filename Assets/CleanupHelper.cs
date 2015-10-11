using UnityEngine;
using System.Collections;

public class CleanupHelper : MonoBehaviour {
    public void WaitAndDestroy()
    {
        Destroy(this.gameObject);

    }
}

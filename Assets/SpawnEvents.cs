using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEvents : MonoBehaviour {
    public Queue<GameObject> flyGuys = new Queue<GameObject>();

    public void Disconnect(object sender, System.EventArgs e)
    {
        Debug.Log(sender.GetType());
        if (flyGuys.Count > 0)
            Destroy(flyGuys.Dequeue());
    }
}

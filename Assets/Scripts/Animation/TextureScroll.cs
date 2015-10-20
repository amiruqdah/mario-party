using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {


    public float scrollSpeed = .5f;
    public float offset;
    public float rotate = 0;

    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10.0f;
        this.gameObject.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));

    }

}

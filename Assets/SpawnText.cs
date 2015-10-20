using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SpawnText : MonoBehaviour {

    private TextMesh textMesh;
    private float constScale = 8f;
    private bool canStopBlink = false;
    private Color color;
    private Transform parent;
    private float yOffset;
	// Use this for initialization
	void Start () {
        textMesh = this.GetComponent<TextMesh>();
        textMesh.alignment = TextAlignment.Center;
        parent = transform.parent;
        textMesh.text = Truncate(parent.GetComponent<HFTGamepad>().getNetPlayer().Name,6);
        if (gameObject.transform.parent.tag == "Mario" || gameObject.transform.parent.tag == "YellowLuigi")
        {
            yOffset = 1.24f;
            // What tween do I put here oh god
            //gameObject.transform.DOPunchScale(new Vector3(0.12f,0.12f),0.6f,2,3);
            //gameObject.transform.DOShakeRotation(0.3f);
        }

        if (gameObject.transform.parent.tag == "Luigi" || gameObject.transform.parent.tag == "PurpleMario")
        {
            yOffset = 1.75f;
        }
        color = parent.GetComponent<Movement>().pipeSpawnColor;
        parent.DetachChildren();
        StartCoroutine(DestroyAndWait());
	}

    public void Update()
    {
      
        // Set position of GUI element
        this.transform.position = new Vector3(parent.position.x - .65f, parent.position.y * yOffset, parent.position.z);

        if (canStopBlink == false)
        {
            textMesh.color = new Color(color.r, color.g, color.b, Mathf.Sin(Time.time * constScale));
            constScale = Mathf.Lerp(constScale, 50f, Time.time * 8.5f);
        }
    }

    IEnumerator DestroyAndWait()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
    private string Truncate(this string value, int maxLength)
    {
        if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            return value.Substring(0, maxLength);
        
        return value;

    }
	

}

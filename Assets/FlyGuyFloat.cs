using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FlyGuyFloat : MonoBehaviour {
    
    public float amplitude;
    public int speed;
    private float startY;
    void Start()
    {
       float startY = transform.position.y;


       Sequence cloudTween = DOTween.Sequence();
       cloudTween.Append(this.transform.DOScale(1.3f, 1.6f));
       cloudTween.Append(this.transform.DOScale(1f, 1.8f));
       cloudTween.SetLoops(-1).SetEase(Ease.InSine);

    }
    
    void Update()
    {

        transform.position = new Vector3(transform.position.x, (transform.parent.position.y + amplitude * Mathf.Sin(speed * Time.time)) , 2);
    }
    
    
    

}

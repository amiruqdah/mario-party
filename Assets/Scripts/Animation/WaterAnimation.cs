using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class WaterAnimation : MonoBehaviour {
    private string anim_anchor;
    private int currentFrame;
    private Mesh[] waveFrame;
    private float nextFrameChange = 0.0f;
    public float period = 0.13f;
	// Use this for initialization
	void Start () {

        anim_anchor = this.transform.parent.name;
        Int32.TryParse(System.Text.RegularExpressions.Regex.Replace (anim_anchor, @"[^\d]", ""), out currentFrame);
        currentFrame -= 1;
        waveFrame = new Mesh[4];

        waveFrame[0] = Resources.Load("Models/Environment/wave_1", typeof(Mesh)) as Mesh;
        waveFrame[1] = Resources.Load("Models/Environment/wave_2", typeof(Mesh)) as Mesh;
        waveFrame[2] = Resources.Load("Models/Environment/wave_3", typeof(Mesh)) as Mesh;
        waveFrame[3] = Resources.Load("Models/Environment/wave_4", typeof(Mesh)) as Mesh;

	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > nextFrameChange){
            nextFrameChange += period;
            nextWaveFrame();
        }
    }

    private void nextWaveFrame()
    {
        if (currentFrame <= 2)
            currentFrame += 1;
        else
            currentFrame = 0;

        this.GetComponent<MeshFilter>().mesh = waveFrame[currentFrame];
    }
}

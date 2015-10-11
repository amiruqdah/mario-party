using UnityEngine;
using System.Collections;

public class PerlinWater : MonoBehaviour {

    public int size = 10; // 10 x 10
    public GameObject[] waves = new GameObject[4]; // set the four wave objects in the inspector
    public float heightScale = 1; 
    public float scale = 1;

	// Use this for initialization
	void Start () {
        for ( var x = 0; x < size; x++ ){
            for( var z = 0; z < size; z++){
                GameObject wave = Instantiate(waves[Random.Range(0, 4)], new Vector3(x, 0, z), Quaternion.identity) as GameObject;
                wave.transform.GetChild(0).localScale = new Vector3(0.125f, 0.1f, 0.6f);
                wave.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
                wave.transform.GetChild(0).gameObject.AddComponent<WaterAnimation>();
                wave.transform.parent = this.transform;
             }
        }


        this.transform.position = new Vector3(-1.7f, -19.29916f, 39.2197f);
	}

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in this.transform){
            // Applied time based perlin noise to create a subtle water effect
            child.transform.GetChild(0).localPosition = new Vector3(0, heightScale * Mathf.PerlinNoise(Time.time*(child.position.x * scale), Time.time * (transform.position.z * scale)),0);
        }
	}
}

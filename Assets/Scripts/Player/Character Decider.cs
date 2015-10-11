using UnityEngine;
using System.Collections;

public class CharacterDecider : MonoBehaviour {

	public GameObject mario;
	public GameObject luigi;
	public GameObject goonba;

	// Use this for initialization
	void Start () 
	{
		if (!GameObject.FindWithTag ("Mario")) 
		{
			Instantiate(mario);
		} 
		else if (!GameObject.FindWithTag ("Luigi")) 
		{
			Instantiate(luigi);
		} 
		else 
		{
			Instantiate(goonba);
		}

		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

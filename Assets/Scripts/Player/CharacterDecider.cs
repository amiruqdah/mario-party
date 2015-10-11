using UnityEngine;
using System.Collections;

public class CharacterDecider : MonoBehaviour {

	public GameObject mario;
	public GameObject luigi;
	public GameObject purpleMario;
	public GameObject yellowLuigi;
	public GameObject flyGuy;

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
		else if (!GameObject.FindWithTag ("PurpleMario")) 
		{
			Instantiate(purpleMario);
		} 
//		else if (!GameObject.FindWithTag ("YellowLuigi")) 
//		{
//			Instantiate(yellowLuigi);
//		} 
		else 
		{
			Instantiate(flyGuy);
		}

		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using HappyFunTimes;

public class CharacterDecider : MonoBehaviour {

	public GameObject mario;
	public GameObject luigi;
	public GameObject purpleMario;
	public GameObject yellowLuigi;
	public GameObject flyGuy;

	// Use this for initialization
	void Start () 
	{

	}

	void InitializeNetPlayer(SpawnInfo spawnInfo) 
	{
		NetPlayer m_netPlayer = spawnInfo.netPlayer;

		GameObject newChar;
		if (!GameObject.FindWithTag ("Mario")) 
		{
			newChar = Instantiate(mario);
		} 
		else if (!GameObject.FindWithTag ("Luigi")) 
		{
			newChar = Instantiate(luigi);
		} 
		else if (!GameObject.FindWithTag ("PurpleMario")) 
		{
			newChar = Instantiate(purpleMario);
		} 
		else if (!GameObject.FindWithTag ("YellowLuigi")) 
		{
			newChar = Instantiate(yellowLuigi);
		} 
		else 
		{
			newChar = Instantiate(flyGuy);
		}

		newChar.GetComponent<HFTGamepad> ().InitializeNetPlayer (m_netPlayer);

		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using HappyFunTimes;

public class RespawnCharacterDecider : MonoBehaviour {
	
	public GameObject mario;
	public GameObject luigi;
	public GameObject purpleMario;
	public GameObject yellowLuigi;
	public GameObject flyGuy;
	private NetPlayer myPlayer;

	public RespawnCharacterDecider(NetPlayer netPlayer)
	{
		myPlayer = netPlayer;
	}
	
	void Start () 
	{
		
	}
	
	void InitializeNetPlayer(NetPlayer m_netPlayer) 
	{	
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

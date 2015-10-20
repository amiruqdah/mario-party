using UnityEngine;
using System.Collections;
using HappyFunTimes;

public class CharacterDecider : MonoBehaviour {
	
	// SET THESE IN THE INSPECTOR
	public GameObject mario; 		// a reference to the Mario Prefab GameObject
	public GameObject luigi;		// a reference to the Luigi Prefab GameObject
	public GameObject purpleMario;		// a reference to the PurpleMario Prefab GameObject
	public GameObject yellowLuigi;		// a reference to the YellowLuigi Prefab GameObject
	public GameObject flyGuy;		// a reference to the FlyGuy Prefab GameObejct
	
	void InitializeNetPlayer(SpawnInfo spawnInfo) 
	{
		NetPlayer m_netPlayer = spawnInfo.netPlayer;

		GameObject newChar;
		if (!GameObject.FindWithTag ("Mario")) 
			newChar = Instantiate(mario);
		else if (!GameObject.FindWithTag ("Luigi")) 
			newChar = Instantiate(luigi);
		else if (!GameObject.FindWithTag ("PurpleMario")) 
			newChar = Instantiate(purpleMario);
		else if (!GameObject.FindWithTag ("YellowLuigi")) 
			newChar = Instantiate(yellowLuigi);
		else 
			newChar = Instantiate(flyGuy);

		newChar.GetComponent<HFTGamepad>().InitializeNetPlayer (m_netPlayer);
		Destroy (this.gameObject);
	}
        // TODO: Leaderboard Update Here
}

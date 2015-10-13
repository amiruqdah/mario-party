using UnityEngine;
using System.Collections;
using HappyFunTimes;

public class CleanupHelper : MonoBehaviour {
    
	public GameObject spawner;

	public void WaitAndDestroy()
    {
        bool rotateNew = true;
        NetPlayer jesusNetPlayer = null;
        NetPlayer m_netPlayer = this.gameObject.GetComponent<HFTGamepad> ().getNetPlayer ();
		CharacterDecider dec = spawner.GetComponent<CharacterDecider> ();
		GameObject[] flyGuys = GameObject.FindGameObjectsWithTag("FlyGuy");
        if (flyGuys.Length == 0)
        {
            rotateNew = false;
        }
        else
        {
            int randInd = (int)(Mathf.Abs(UnityEngine.Random.value * flyGuys.Length - 0.000001f));
            GameObject randFlyGuy = flyGuys[randInd];
            jesusNetPlayer = randFlyGuy.GetComponent<HFTGamepad>().getNetPlayer();
            Destroy(randFlyGuy);
        }

        GameObject newChar = null;
        if (this.gameObject.tag == "Mario") 
		{
			newChar = Instantiate(dec.mario);
		} 
		else if (this.gameObject.tag == "Luigi") 
		{
			newChar = Instantiate(dec.luigi);
		} 
		else if (this.gameObject.tag == "PurpleMario") 
		{
			newChar = Instantiate(dec.purpleMario);
		} 
		else if (this.gameObject.tag == "YellowLuigi") 
		{
			newChar = Instantiate(dec.yellowLuigi);
		}
		
        if(rotateNew)
        {
            GameObject oldChar = Instantiate(dec.flyGuy);
            newChar.GetComponent<HFTGamepad>().InitializeNetPlayer(jesusNetPlayer);
            Destroy(this.gameObject);
            oldChar.GetComponent<HFTGamepad>().InitializeNetPlayer(m_netPlayer);
        }
		else
        {
            Destroy(this.gameObject);
            newChar.GetComponent<HFTGamepad>().InitializeNetPlayer(m_netPlayer);
        }
    }
}

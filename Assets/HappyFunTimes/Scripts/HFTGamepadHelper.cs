using UnityEngine;
using System.Collections;
using HappyFunTimes;

public class HFTGamepadHelper : MonoBehaviour {

    [System.NonSerialized]
    public PlayerSpawner playerSpawner;

    // This gives you a chance to send that player's phone
    // a command to tell it to display "The game is full" or
    // whatever you want.
    //
    // Note: You can call PlayerSpawner.ReturnPlayer to eject
    // a player from their slot and get a new player for that slot
    // If you do that this function will be called for the returned
    // player.
    //
    // Simiarly you can call PlayerSpawner.FlushCurrentPlayers to
    // eject all current players in which case this will be called
    // for all players that were player.
    void WaitingNetPlayer(SpawnInfo spawnInfo) {
        // Tell the controller to display full message
        spawnInfo.netPlayer.SendCmd("full");
    }

    static private HFTGamepadHelper s_helper;

    public static HFTGamepadHelper helper
    {
        get {
            return s_helper;
        }
    }

    void Awake()
    {
        if (s_helper != null)
        {
            throw new System.InvalidProgramException("there is more than one HFTGamepadHelper component!");
        }
        s_helper = this;
        playerSpawner = GetComponent<PlayerSpawner>();
    }

    void Cleanup()
    {
        s_helper = null;
    }

    void OnDestroy()
    {
        Cleanup();
    }

    void OnApplicationExit()
    {
        Cleanup();
    }
}



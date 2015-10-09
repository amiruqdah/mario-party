using HappyFunTimes;
using HFTSounds;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (HFTGamepad))]
public class HFTSoundPlayer : MonoBehaviour
{

    private class MessageLoadSounds : MessageCmdData
    {
        public MessageLoadSounds(Sounds _sounds)
        {
            sounds = _sounds;
        }
        public Sounds sounds = null;
    }

    private class MessagePlaySound : MessageCmdData
    {
        public MessagePlaySound(string _name, bool _loop = false)
        {
            name = _name;
            loop = _loop;
        }
        public string name;
        public bool loop;
    }

    void Awake()
    {
        try {
            HFTGlobalSoundHelper.GetSounds();
        } catch (System.Exception) {
            Debug.LogError("No HFTGlobalSoundHelper in scene. Please add one");
        }
        m_gamepad = GetComponent<HFTGamepad>();
    }

    void Start()
    {
        NetPlayer netPlayer = m_gamepad.NetPlayer;
        if (netPlayer != null)
        {
            netPlayer.SendCmd("loadSounds", new MessageLoadSounds(HFTGlobalSoundHelper.GetSounds()));
        }
    }

    public void PlaySound(string name, bool loop = false)
    {
        NetPlayer netPlayer = m_gamepad.NetPlayer;
        if (netPlayer != null)
        {
            m_gamepad.NetPlayer.SendCmd("playSound", new MessagePlaySound(name, loop));
        }
    }

    private HFTGamepad m_gamepad;

};


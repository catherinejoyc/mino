using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AK.Wwise.Bank myBank = null;
    public AK.Wwise.Event myEvent = null;

    public object AKSoundEngine { get; private set; }

    void Awake()
    {
        myBank.Load();
    }


}
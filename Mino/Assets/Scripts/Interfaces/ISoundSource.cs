using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
//public class MyFloatEvent : UnityEvent<Vector3> //vector3 is position of sound
//{
//}

public interface ISoundSource{

    void PlaySound(Vector3 pos);
    //  sound.Post(this.gameObject);

    MyFloatEvent SoundEvent
    {
        get;
        set;
    }
    // Wwise Sound
    AK.Wwise.Event Sound
    {
        get;
        set;
    }
    // Sound Type
    SoundType SoundType
    {
        get;
        set;
    }
    SoundPriority SoundValues
    {
        get;
        set;
    }
    //max hearing distance (for enemy)
    float MaxDistance
    {
        get;
        set;
    }

}

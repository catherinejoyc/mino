using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OptionsMenuVariables")]

public class OptionsMenuVariables : ScriptableObject {

    private static OptionsMenuVariables _myInstance;
    public static OptionsMenuVariables _MyInstance
    {
        get
        {
            if (_myInstance == null)
                _myInstance = Resources.Load<OptionsMenuVariables>("OptionsMenuVariables");
            return _myInstance;
        }
    }

    public float volume;
    public float mouseSensitivity;
}

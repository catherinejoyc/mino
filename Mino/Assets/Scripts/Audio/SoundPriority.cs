using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundPriority")]

public class SoundPriority : ScriptableObject {

    private static SoundPriority _myInstance;
    public static SoundPriority _MyInstance
    {
        get
        {
            if (_myInstance == null)
                _myInstance = Resources.Load<SoundPriority>("SoundPriority");
            return _myInstance;
        }
    }

    [Header("value is the max hearing distance for enemy (the higher the louder)")]
    // Note: if another sound is added, also add a type for it down below and in SoundScript.cs
    // Don't forget to update the value in the Scriptable Object!
    //currently 14 sounds
    #region sound values
    [Tooltip("happens when an enemy destroys the box")]
    public float destroyedBox;

    [Tooltip("happens when the box is moved/pushed etc.")]
    public float movingBox;

    [Tooltip("happens when the door is opened/closed")]
    public float movingDoor;

    [Tooltip("happens when a pressure plate is activated")]
    public float activatedPressurePlate;

    [Tooltip("happens when something is moving in a bush")]
    public float movingBush;

    [Tooltip("happens when something is moving on gravel")]
    public float movingOnGravel;

    [Tooltip("happens when something is moving on stone")]
    public float movingOnStone;

    [Tooltip("happens when something is moving on gravel")]
    public float movingOnGrass;

    [Tooltip("happens when a thrown stone lands on hard ground (stone, gravel, wall)")]
    public float hardStoneImpact;

    [Tooltip("happens when a thrown stone lands on gras or in a bush")]
    public float softStoneImpact;

    [Tooltip("happens while picking up stones")]
    public float pickUpStone;

    [Tooltip("sound when player is sneaking")]
    public float sneaking;

    [Tooltip("sound of chalk (drawing/erase)")]
    public float useChalk;

    [Tooltip("environmental sounds, like torches etc.")]
    public float environmentSound;
    #endregion
}

// Type of sound
// Note: if another Type is added, also add a value for it up there and in SoundScript.cs
public enum SoundType //currently 14 Types
{
    DestroyedBox,
    MovingBox,
    MovingDoor,
    ActivatedPressurePlate,
    MovingBush,
    MovingOnGravel,
    MovingOnStone,
    MovingOnGrass,
    HardStoneImpact,
    SoftStoneImpact,
    PickUpStone,
    Sneaking,
    UseChalk,
    EnvironmentSound
}

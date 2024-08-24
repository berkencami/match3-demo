using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SoundConfig", fileName = "SoundConfig")]
public class SoundConfig : ScriptableObject
{
   public List<SoundFX> soundFx;
}

[Serializable]
public class SoundFX
{
   public SoundType soundType;
   public AudioClip audioClip;
   public float volume;
}

public enum SoundType
{
   Match,
   Merge,
   Bomb,
   Rocket,
   Success
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Dev.Scripts.Data
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SoundConfig", fileName = "SoundConfig")]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private List<SoundFX> _soundFx;
        public List<SoundFX> SoundFx => _soundFx;
    }

    [Serializable]
    public class SoundFX
    {
        [SerializeField] private SoundType _soundType;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private float _volume;

        public SoundType SoundType => _soundType;
        public AudioClip AudioClip => _audioClip;
        public float Volume => _volume;
    }

    public enum SoundType
    {
        Match,
        Merge,
        Bomb,
        Rocket,
        Success
    }
}
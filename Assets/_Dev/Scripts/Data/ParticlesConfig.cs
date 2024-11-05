using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Dev.Scripts.Data
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ParticlesConfig", fileName = "ParticlesConfig")]
    public class ParticlesConfig : ScriptableObject
    {
        [SerializeField] private List<Particle> _particles;
        public  List<Particle> Particles => _particles;
    }

    [Serializable]
    public class Particle
    {
        [SerializeField]private ParticleType _particleType;
        [SerializeField]private GameObject _prefab;
        [SerializeField]private float _duration;
    
        public ParticleType ParticleType => _particleType;
        public GameObject Prefab => _prefab;
        public float Duration => _duration;
    }

    public enum ParticleType
    {
        Red,
        Yellow,
        Green,
        Blue,
        Purple,
        Pink,
        Obstacle,
        RocketTrail,
        Bomb,
        StarExplode,
        Fireworks
    }
}
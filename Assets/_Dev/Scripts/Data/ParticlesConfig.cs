using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ParticlesConfig", fileName = "ParticlesConfig")]
public class ParticlesConfig : ScriptableObject
{
    public List<Particle> particles;
}

[Serializable]
public class Particle
{
    public ParticleType particleType;
    public GameObject prefab;
    public float duration;
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
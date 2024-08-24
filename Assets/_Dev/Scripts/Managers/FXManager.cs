using Lean.Pool;
using UnityEngine;

public class FXManager : Manager
{
    public static FXManager instance;
    [SerializeField] private ParticlesConfig particlesConfig;
    [SerializeField] private SoundConfig soundConfig;
    private AudioSource audioSource;
    
    protected override void Awake()
    {
        base.Awake();
        Singleton();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void PreLevelInstantiateProcess()
    {
        base.PreLevelInstantiateProcess();
        StopSound();
    }

    protected override void LevelEndProcess(bool status)
    {
        base.LevelEndProcess(status);
        if (status)
        {
            SuccessFX();
        }
    }

    private void Singleton()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
    }

    public GameObject PlayParticle(ParticleType particleType, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        Particle particle = particlesConfig.particles.Find(p => p.particleType == particleType);
        GameObject spawnedParticle = LeanPool.Spawn(particle.prefab, pos, rot, parent);
        if (parent != null)
        {
            spawnedParticle.transform.localPosition = pos;
        }

        if (spawnedParticle == null) return spawnedParticle;

        LeanPool.Despawn(spawnedParticle, particle.duration);

        return spawnedParticle;
    }

    public void PlaySoundFX(SoundType soundType)
    {
        SoundFX soundFx = soundConfig.soundFx.Find(p => p.soundType == soundType);
        audioSource.PlayOneShot(soundFx.audioClip,soundFx.volume);
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    private void SuccessFX()
    {
        PlayParticle(ParticleType.Fireworks, Vector3.zero, Quaternion.Euler(-90,0,0));
        PlaySoundFX(SoundType.Success);
    }
}


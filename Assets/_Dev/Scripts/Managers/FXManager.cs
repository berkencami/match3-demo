using _Dev.Scripts.Data;
using Lean.Pool;
using UnityEngine;

namespace _Dev.Scripts.Managers
{
    public class FXManager : Manager
    {
        public static FXManager Instance;
        [SerializeField] private ParticlesConfig _particlesConfig;
        [SerializeField] private SoundConfig _soundConfig;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            Singleton();
            _audioSource = GetComponent<AudioSource>();
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
            if (Instance != null)
            {
                Destroy(this);
            }

            Instance = this;
        }

        public GameObject PlayParticle(ParticleType particleType, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            var particle = _particlesConfig.Particles.Find(p => p.ParticleType == particleType);
            var spawnedParticle = LeanPool.Spawn(particle.Prefab, pos, rot, parent);
            if (parent != null)
            {
                spawnedParticle.transform.localPosition = pos;
            }

            if (spawnedParticle == null) return spawnedParticle;

            LeanPool.Despawn(spawnedParticle, particle.Duration);

            return spawnedParticle;
        }

        public void PlaySoundFX(SoundType soundType)
        {
            var soundFx = _soundConfig.SoundFx.Find(p => p.SoundType == soundType);
            _audioSource.PlayOneShot(soundFx.AudioClip, soundFx.Volume);
        }

        public void StopSound()
        {
            _audioSource.Stop();
        }

        private void SuccessFX()
        {
            PlayParticle(ParticleType.Fireworks, Vector3.zero, Quaternion.Euler(-90, 0, 0));
            PlaySoundFX(SoundType.Success);
        }
    }
}
using HeroSurvivor.Gameplay.Intent;
using HeroSurvivor.Gameplay.Health;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using System;
using Zenject;


namespace HeroSurvivor.Gameplay.Combat
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private IntentSource _intent;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private BulletPoolManager _bulletPool;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private CharacterConfig _characterConfig;

        public event Action Shot;

        private float _cooldown;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus) 
        { 
            _signalBus = signalBus;
        }

        void Update()
        {
            _cooldown -= Time.deltaTime;

            if (_cooldown > 0f)
                return;

            if (_intent.IsShooting == false)
                return;

            Vector3 direction = _intent.AimPoint - _muzzle.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                direction.Normalize();
            }

            if (_audioSource != null && _shootSound != null)
            {
                _audioSource.PlayOneShot(_shootSound);
            }

            GameObject bulletObj = _bulletPool.GetPooledObject();

            if (bulletObj != null)
            {
                Vector3 spawnPosition = _muzzle.position;

                Quaternion spawnRotation = direction != Vector3.zero
                    ? Quaternion.LookRotation(direction)
                    : _muzzle.rotation;

                bulletObj.transform.position = spawnPosition;
                bulletObj.transform.rotation = spawnRotation;

                if (bulletObj.TryGetComponent<Bullet>(out Bullet bullet))
                {
                    bullet.SetPosition(spawnPosition);
                    bullet.SetTargetTag(_characterConfig.targetTag);
                    bullet.SetDamage(_characterConfig.damage);
                }

                bulletObj.SetActive(true);
            }

            _cooldown = _fireRate;

            Shot?.Invoke();
            _signalBus?.Fire<WeaponFiredSignal>();
        }
    }
}
using HeroSurvivor.Gameplay.Combat;
using HeroSurvivor.Gameplay.Health;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace HeroSurvivor.Effects
{
    public class ScreenShake : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform _camera;

        [Header("On Shot")]
        [SerializeField] private bool _onShot;
        [SerializeField] private float _shotIntensity = 0.13f;
        [SerializeField] private float _shotDuration = 0.1f;
        [SerializeField] private float _shotFrequency = 10f;

        [Header("On Hit")]
        [SerializeField] private bool _onHit;
        [SerializeField] private float _hitIntensity = 0.2f;
        [SerializeField] private float _hitDuration = 0.1f;
        [SerializeField] private float _hitFrequency = 15f;

        [Header("On Kill")]
        [SerializeField] private bool _onKill;
        [SerializeField] private float _killIntensity = 0.6f;
        [SerializeField] private float _killDuration = 0.15f;
        [SerializeField] private float _killFrequency = 15f;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus?.Subscribe<WeaponFiredSignal>(OnShot);
            _signalBus?.Subscribe<AnyDamagedSignal>(OnDamaged);
            _signalBus?.Subscribe<AnyKilledSignal>(OnKilled);
        }

        private void OnDisable()
        {
            _signalBus?.TryUnsubscribe<WeaponFiredSignal>(OnShot);
            _signalBus?.TryUnsubscribe<AnyDamagedSignal>(OnDamaged);
            _signalBus?.TryUnsubscribe<AnyKilledSignal>(OnKilled);
        }

        private void OnShot()
        {
            if (_onShot)
                Shake(_shotIntensity, _shotDuration, _shotFrequency);
        }

        private void OnDamaged(AnyDamagedSignal signal)
        {
            if (_onHit)
                Shake(_hitIntensity, _hitDuration, _hitFrequency);
        }

        private void OnKilled(AnyKilledSignal signal)
        {
            if (_onKill)
                Shake(_killIntensity, _killDuration, _killFrequency);
        }

        private void Shake(float intensity, float duration, float frequency)
        {
            Tween.StopAll(onTarget: _camera);
            Tween.ShakeLocalPosition(_camera, new Vector3(intensity, intensity, intensity), duration, frequency, useUnscaledTime: true);
        }
    }
}

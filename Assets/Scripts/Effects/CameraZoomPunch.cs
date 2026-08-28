using HeroSurvivor.Gameplay.Combat;
using HeroSurvivor.Gameplay.Health;
using PrimeTween;
using UnityEngine;
using Zenject;

namespace HeroSurvivor
{
    public class CameraZoomPunch : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Camera _camera;

        [Header("On Shot")]
        [SerializeField] private bool _onShot;
        [SerializeField] private float _shotStrength = 0.15f;

        [Header("On Hit")]
        [SerializeField] private bool _onHit;
        [SerializeField] private float _hitStrength = 0.15f;

        [Header("On Kill")]
        [SerializeField] private bool _onKill;
        [SerializeField] private float _killStrength = 0.05f;

        [Header("Timing")]
        [SerializeField] private float _hold = 0.08f;
        [SerializeField] private float _returnDuration = 0.2f;

        private float _baseFov;
        private SignalBus _signalBus;

        [Inject]
        public void Construct (SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _baseFov = _camera.fieldOfView;
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
                Punch(_shotStrength);
        }

        private void OnDamaged(AnyDamagedSignal signal)
        {
            if (_onHit)
                Punch(_hitStrength);
        }

        private void OnKilled(AnyKilledSignal signal)
        {
            if (_onKill)
                Punch(_killStrength);
        }

        private void Punch(float strength)
        {
            Tween.StopAll(onTarget: _camera);

            float punched = _baseFov * (1f - strength);
            _camera.fieldOfView = punched;

            Tween.CameraFieldOfView(_camera, punched, _baseFov, _returnDuration, startDelay: _hold, useUnscaledTime: true);
        }
    }
}

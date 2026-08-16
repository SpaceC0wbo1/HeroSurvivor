using HeroSurvivor.Gameplay.Combat;
using PrimeTween;
using UnityEngine;

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

        private void Awake()
        {
            _baseFov = _camera.fieldOfView;
        }

        private void OnEnable()
        {
            Shooter.AnyShot += OnShot;
            Health.AnyDamaged += OnDamaged;
            Health.AnyKilled += OnKilled;
        }

        private void OnDisable()
        {
            Shooter.AnyShot -= OnShot;
            Health.AnyDamaged -= OnDamaged;
            Health.AnyKilled -= OnKilled;
        }

        private void OnShot()
        {
            if (_onShot)
                Punch(_shotStrength);
        }

        private void OnDamaged(Vector3 direction)
        {
            if (_onHit)
                Punch(_hitStrength);
        }

        private void OnKilled(Vector3 direction)
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

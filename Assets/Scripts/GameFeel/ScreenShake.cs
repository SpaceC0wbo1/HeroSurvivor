using HeroSurvivor.Gameplay.Combat;
using PrimeTween;
using UnityEngine;

namespace HeroSurvivor.GameFeel
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
                Shake(_shotIntensity, _shotDuration, _shotFrequency);
        }

        private void OnDamaged(Vector3 direction)
        {
            if (_onHit)
                Shake(_hitIntensity, _hitDuration, _hitFrequency);
        }

        private void OnKilled(Vector3 direction)
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

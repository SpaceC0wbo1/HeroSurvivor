using HeroSurvivor.Gameplay.Combat;
using PrimeTween;
using UnityEngine;

namespace HeroSurvivor.Effects
{
    public class TiltOnShot : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Shooter _shooter;
        [SerializeField] private Transform _target;

        [Header("Tilt")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Vector3 _kickEulerAngles = new Vector3(-15.5f, 0f, 0f);
        [SerializeField] private float _returnDuration = 0.15f;

        private Quaternion _baseRotation;

        private void Awake()
        {
            _baseRotation = _target.rotation;
        }

        private void OnEnable() => _shooter.Shot += OnShot;

        private void OnDisable() => _shooter.Shot -= OnShot;

        private void OnShot()
        {
            if (_enabled == false)
                return;

            Tween.StopAll(onTarget: _target);
            _target.localRotation = _baseRotation * Quaternion.Euler(_kickEulerAngles);
            Tween.LocalRotation(_target, _baseRotation, _returnDuration);
        }
    }
}

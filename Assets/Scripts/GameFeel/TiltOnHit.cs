using HeroSurvivor.Gameplay.Combat;
using PrimeTween;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace HeroSurvivor
{
    public class TiltOnHit : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Health _health;
        [SerializeField] private Transform _target;

        [Header("Tilt")]
        [SerializeField] private bool _enabled;
        [SerializeField] private Vector3 _kickEulerAngles = new Vector3(-25f, 0f, 0f);
        [SerializeField] private float _returnDuration = 0.15f;

        private Quaternion _baseRotation;

        private void Awake()
        {
            _baseRotation = _target.localRotation;
        }

        private void OnEnable()
        {
            _health.Damaged += OnHit;
            _health.Killed += OnHit;
        }

        private void OnDisable()
        {
            _health.Damaged -= OnHit;
            _health.Killed -= OnHit;
        }

        private void OnHit(Vector3 direction)
        {
            if(_enabled == false) 
                return;

            Tween.StopAll(onTarget: _target);
            _target.localRotation = _baseRotation * Quaternion.Euler(_kickEulerAngles);
            Tween.LocalRotation(_target, _baseRotation, _returnDuration, useUnscaledTime: true);
        }
    }
}

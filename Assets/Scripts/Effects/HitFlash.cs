using System.Collections.Generic;
using HeroSurvivor.Gameplay.Interfaces;
using PrimeTween;
using UnityEngine;

namespace HeroSurvivor.Effects
{
    public class HitFlash : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        [Header("Setup")]
        [Tooltip("Залиште порожнім для автоматичного пошуку всіх типів рендерерів")]
        [SerializeField] private Renderer[] _renderers;

        [Header("Flash Settings")]
        [SerializeField] private bool _enabled = true;
        [SerializeField] private Color _flashColor = Color.white;
        [SerializeField] private int _flashCount = 3;
        [SerializeField] private float _totalDuration = 0.2f;

        private IHitFeedback _hitFeedback;
        private readonly List<Material> _materials = new List<Material>();
        private readonly List<Color> _baseColors = new List<Color>();
        private readonly List<int> _propertyIds = new List<int>();

        private void Awake()
        {
            _hitFeedback = GetComponentInParent<IHitFeedback>();
            if (_hitFeedback == null)
                _hitFeedback = GetComponentInChildren<IHitFeedback>();

            // Знаходить і SkinnedMeshRenderer (тулуб), і MeshRenderer (голова, зброя, аксесуари)
            if (_renderers == null || _renderers.Length == 0)
                _renderers = GetComponentsInChildren<Renderer>(true);

            InitializeMaterials();
        }

        private void InitializeMaterials()
        {
            _materials.Clear();
            _baseColors.Clear();
            _propertyIds.Clear();

            foreach (Renderer rend in _renderers)
            {
                if (rend == null) continue;

                // Отримуємо всі матеріали конкретного рендера (тулуб, голова, деталі)
                Material[] instanceMaterials = rend.materials;

                for (int i = 0; i < instanceMaterials.Length; i++)
                {
                    Material mat = instanceMaterials[i];
                    if (mat == null) continue;

                    int propId = -1;
                    if (mat.HasProperty(BaseColorId))
                        propId = BaseColorId;
                    else if (mat.HasProperty(ColorId))
                        propId = ColorId;
                    else if (mat.HasProperty(EmissionColorId))
                        propId = EmissionColorId;

                    if (propId != -1)
                    {
                        _materials.Add(mat);
                        _baseColors.Add(mat.GetColor(propId));
                        _propertyIds.Add(propId);
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (_hitFeedback != null)
                _hitFeedback.OnHit += OnHit;
        }

        private void OnDisable()
        {
            if (_hitFeedback != null)
                _hitFeedback.OnHit -= OnHit;

            ResetColors();
        }

        private void OnHit(Vector3 direction)
        {
            if (!_enabled || _materials.Count == 0)
                return;

            int cycles = _flashCount * 2;
            float halfDuration = _totalDuration / cycles;

            for (int i = 0; i < _materials.Count; i++)
            {
                Material material = _materials[i];
                Color baseColor = _baseColors[i];
                int propId = _propertyIds[i];

                Tween.StopAll(onTarget: material);

                Tween.MaterialColor(
                    material,
                    propId,
                    baseColor,
                    _flashColor,
                    halfDuration,
                    Ease.Linear,
                    cycles,
                    CycleMode.Yoyo,
                    useUnscaledTime: true
                );
            }
        }

        private void ResetColors()
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                if (_materials[i] != null)
                {
                    Tween.StopAll(onTarget: _materials[i]);
                    _materials[i].SetColor(_propertyIds[i], _baseColors[i]);
                }
            }
        }
    }
}
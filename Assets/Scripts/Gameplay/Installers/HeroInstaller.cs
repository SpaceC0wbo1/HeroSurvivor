using HeroSurvivor.Gameplay.Health;
using UnityEngine;
using Zenject;

namespace HeroSurvivor.Gameplay.Installers
{
    public class HeroInstaller : MonoInstaller
    {
        [SerializeField] private CharacterConfig _config;
        [SerializeField] private HeroHealthView _healthView;
        public override void InstallBindings()
        {
            Container.Bind<HealthModel>().AsSingle().WithArguments(_config.maxHealth);
            Container.Bind<HeroHealthView>().FromInstance(_healthView).AsSingle();

            Container.BindInterfacesAndSelfTo<HeroHealthController>().AsSingle();
        }
    }
}

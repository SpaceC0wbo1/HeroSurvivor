using HeroSurvivor.Gameplay.Health;
using Zenject;

namespace HeroSurvivor.Gameplay.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<EnemyDiedSignal>();
            Container.DeclareSignal<HeroHealthChangedSignal>();
            Container.DeclareSignal<HeroDiedSignal>();
            Container.DeclareSignal<WeaponFiredSignal>();
            Container.DeclareSignal<AnyDamagedSignal>();
            Container.DeclareSignal<AnyKilledSignal>();
        }
    }
}

using _Dev.Scripts.Managers;
using Zenject;

namespace _Dev.Scripts.Installers
{
    public class MonoBehaviourInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<BoardManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<MatchManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<InputManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<BlockFactory>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<MoveManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GoalManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
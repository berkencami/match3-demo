using UnityEngine;
using Zenject;

namespace _Dev.Scripts.Installers
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField]private GridCell.GridCell _gridCell;

        public override void InstallBindings()
        {
            Container.BindFactory<GridCell.GridCell, GridFactory>().FromComponentInNewPrefab(_gridCell).AsSingle().NonLazy();
        }

        public class GridFactory : PlaceholderFactory<GridCell.GridCell>
        {
        }
    
    }
}

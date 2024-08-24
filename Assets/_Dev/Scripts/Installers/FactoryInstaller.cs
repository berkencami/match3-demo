using Zenject;

public class FactoryInstaller : MonoInstaller
{
    public GridCell gridCell;

    public override void InstallBindings()
    {
        Container.BindFactory<GridCell, GridFactory>().FromComponentInNewPrefab(gridCell).AsSingle().NonLazy();
    }

    public class GridFactory : PlaceholderFactory<GridCell>
    {
    }
    
}

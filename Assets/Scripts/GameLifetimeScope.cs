using VContainer;
using VContainer.Unity;
using ezygamers.dragndropv1;
public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Register CMSGameEventManager as a singleton
        builder.Register<CMSGameEventManager>(Lifetime.Singleton);

        //adding the GameManager
        builder.RegisterComponentInHierarchy<GameManager>();

        // Register other components or managers as needed
        builder.RegisterComponentInHierarchy<UIManager>();
        builder.RegisterComponentInHierarchy<AudioManager>();
        //builder.RegisterComponentInHierarchy<DropHandler>();
        //builder.RegisterComponentInHierarchy<PrefabUIManager>();

    }
}
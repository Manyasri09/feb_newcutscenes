using PlayerProgressSystem;
using RewardSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private RewardConfig rewardConfig; // Assign via inspector
    //[SerializeField] private GameObject DailyRewardsUI; // Assign via inspector
    protected override void Configure(IContainerBuilder builder)
    {
        // Register CMSGameEventManager as a singleton
        builder.Register<CMSGameEventManager>(Lifetime.Singleton);

        //adding the GameManager
        builder.RegisterComponentInHierarchy<GameManager>();

        // Register other components or managers as needed
        builder.RegisterComponentInHierarchy<UIManager>();

        // adding Player Progress Manager
        builder.Register<IPlayerProgressStorage, PlayerPrefsStorage>(Lifetime.Singleton);
        builder.Register<PlayerProgressManager>(Lifetime.Singleton).WithParameter("storage", context => context.Resolve<IPlayerProgressStorage>());

        // Register the repository
        builder.Register<IRewardRepository>(c => new ScriptableObjectRewardRepository(rewardConfig), Lifetime.Singleton);

        // Register the state service (implemented by main project or separate package)
        builder.Register<IRewardStateService, MyCustomRewardStateService>(Lifetime.Singleton);

        // Register the delivery service
        builder.Register<IRewardDeliveryService, MyCustomRewardDeliveryService>(Lifetime.Singleton);

        // Register the reward manager
        builder.Register<IRewardManager, DefaultRewardManager>(Lifetime.Singleton);

        builder.Register<DailyRewardsUI>(Lifetime.Singleton);

    }
}
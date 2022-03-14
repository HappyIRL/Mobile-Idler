using ModestTree;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

	[CreateAssetMenu(menuName = "ScriptableObjects/DefaultSceneInstaller")]
	public class DefaultSceneInstaller : ScriptableObjectInstaller<DefaultSceneInstaller>
	{
		[SerializeField] private GameObject mainCameraPrefab, playerInputHandlerPrefab, deviceOrientationPrefab, selectorPrefab, playerWalletPrefab, casinoTileHandlerPrefab;
		[SerializeField] private GameObject incomePrefab, cashierPrefab;

		public override void InstallBindings()
		{
			Container.Bind<PlayerCamera>().FromComponentInNewPrefab(mainCameraPrefab).AsSingle().NonLazy();
			Container.Bind<PlayerInputBroadcast>().FromComponentInNewPrefab(playerInputHandlerPrefab).AsSingle().NonLazy();
			Container.Bind<DeviceOrientationHandler>().FromComponentInNewPrefab(deviceOrientationPrefab).AsSingle().NonLazy();
			Container.Bind<Selector>().FromComponentInNewPrefab(selectorPrefab).AsSingle().NonLazy();
			Container.Bind<PlayerWallet>().FromComponentInNewPrefab(playerWalletPrefab).AsSingle().NonLazy();
			Container.Bind<CasinoTileSpawner>().FromComponentInNewPrefab(casinoTileHandlerPrefab).AsSingle().NonLazy();
			Container.Bind<Income>().FromComponentInNewPrefab(incomePrefab).AsSingle().NonLazy();
			Container.Bind<Cashier>().FromComponentInNewPrefab(cashierPrefab).AsSingle().NonLazy();


			Container.BindFactory<GameObject, Transform, PrefabFactory>().FromFactory<NormalPrefabFactory>();
		}
	}

	public class PrefabFactory : PlaceholderFactory<GameObject, Transform>
	{
		public Transform Create(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			var t = Create(prefab);
			t.SetParent(parent, worldPositionStays: false);
			t.position = position;
			t.rotation = rotation;
			return t;
		}
		public Transform Create(GameObject prefab, Transform parent)
		{
			var t = Create(prefab);
			t.SetParent(parent, worldPositionStays: false);
			return t;
		}
	}

	public class NormalPrefabFactory : IFactory<GameObject, Transform>
	{
		[Inject]
		readonly DiContainer _container = null;

		public Transform Create(GameObject prefab)
		{
			Assert.That(prefab != null, "Null prefab given to factory create method");

			return _container.InstantiatePrefab(prefab).transform;
		}
	}
using System;
using ModestTree;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "ScriptableObjects/DefaultSceneInstaller")]
public class DefaultSceneInstaller : ScriptableObjectInstaller<DefaultSceneInstaller>
{
	[SerializeField] private GameObject mainCameraPrefab, playerInputHandlerPrefab, deviceOrientationPrefab, selectorPrefab;

	[SerializeField]
	private GameObject saveHandlerPrefab, gameHandlerPrefab, prototypeSelectorPrefab, sceneViewPrefab;

	public override void InstallBindings()
	{
		Container.Bind<PlayerCamera>().FromComponentInNewPrefab(mainCameraPrefab).AsSingle().NonLazy();
		Container.Bind<PlayerInputBroadcast>().FromComponentInNewPrefab(playerInputHandlerPrefab).AsSingle().NonLazy();
		Container.Bind<DeviceOrientationHandler>().FromComponentInNewPrefab(deviceOrientationPrefab).AsSingle().NonLazy();
		Container.Bind<Selector>().FromComponentInNewPrefab(selectorPrefab).AsSingle().NonLazy();
		Container.Bind<SaveHandler>().FromComponentInNewPrefab(saveHandlerPrefab).AsSingle().NonLazy();
		Container.Bind<OnGUISceneView>().FromComponentInNewPrefab(sceneViewPrefab).AsSingle().NonLazy();
		Container.Bind<GameHandler>().FromComponentInNewPrefab(gameHandlerPrefab).AsSingle().NonLazy();
		Container.Bind<PrototypeSelector>().FromComponentInNewPrefab(prototypeSelectorPrefab).AsSingle().NonLazy();

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
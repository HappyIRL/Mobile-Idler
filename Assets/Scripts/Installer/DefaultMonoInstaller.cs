using System.ComponentModel;
using Assets.Scripts.UI;
using ModestTree;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Assets.Scripts.Installer
{
	public class DefaultMonoInstaller : MonoInstaller<DefaultMonoInstaller>
	{
		[SerializeField] private Tilemap floorMap;
		[SerializeField] private Tilemap casinoMap;
		[SerializeField] private ScriptableObject casinoSprites;
		[SerializeField] private FrontendUI frontendUI;

		public override void InstallBindings()
		{
			Container.Bind<Tilemap>().WithId("floorMap").FromInstance(floorMap);
			Container.Bind<Tilemap>().WithId("casinoMap").FromInstance(casinoMap);
			Container.Bind<CasinoSprites>().FromScriptableObject(casinoSprites).AsSingle().NonLazy();
			Container.Bind<FrontendUI>().FromInstance(frontendUI).AsSingle().NonLazy();
		}
	}
}

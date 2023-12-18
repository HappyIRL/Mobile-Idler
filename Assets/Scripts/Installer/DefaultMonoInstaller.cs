using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Assets.Scripts.Installer
{
	public class DefaultMonoInstaller : MonoInstaller<DefaultMonoInstaller>
	{
		[SerializeField] private Tilemap roomMap;
		[SerializeField] private Tilemap slotMap;
		[SerializeField] private ScriptableObject casinoSprites;
		[SerializeField] private FrontendUI frontendUI;

		public override void InstallBindings()
		{
			Container.Bind<Tilemap>().WithId("roomMap").FromInstance(roomMap);
			Container.Bind<Tilemap>().WithId("slotMap").FromInstance(slotMap);
			Container.Bind<CasinoSprites>().FromScriptableObject(casinoSprites).AsSingle().NonLazy();
			Container.Bind<FrontendUI>().FromInstance(frontendUI).AsSingle().NonLazy();
		}
	}
}

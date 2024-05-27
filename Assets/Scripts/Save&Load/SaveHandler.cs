using UnityEngine;

public class SaveHandler : MonoBehaviour
{
	[Zenject.Inject] private GameHandler gameHandler;

	private Saver saver;
	private bool gameDataLoaded = false;

	private void Start()
	{
		if (!gameDataLoaded)
		{
			gameDataLoaded = true;
			Load();
		}
	}

	[NaughtyAttributes.Button("Save")]
	public void Save()
	{
		saver ??= new Saver(gameHandler);

		saver.Save();
	}

	[NaughtyAttributes.Button("Load")]
	public void Load()
	{
		saver ??= new Saver(gameHandler);
		saver.Load();
	}

	[NaughtyAttributes.Button("Load New")]
	public void LoadNew()
	{
		saver ??= new Saver(gameHandler);
		saver.LoadNew();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && !gameDataLoaded)
		{
			gameDataLoaded = true;
			Load();
		}
		else if(pauseStatus)
		{
			gameDataLoaded = false;
			Save();
		}
	}

	private void OnDisable()
	{
		gameDataLoaded = false;
		saver ??= new Saver(gameHandler);
		saver.Save();
	}
}
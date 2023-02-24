using UnityEngine;

public class SaveHandler : MonoBehaviour
{
	[Zenject.Inject] private GameHandler gameHandler;

	private Saver saver;

	private void Start()
	{
		Load();
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

	private void OnDisable()
	{
		saver ??= new Saver(gameHandler);
		saver.Save();
	}
}
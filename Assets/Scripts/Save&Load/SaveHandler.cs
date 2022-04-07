using UnityEngine;

public class SaveHandler : MonoBehaviour
{
	[Zenject.Inject] private GameHandler gameHandler;

	private void Start()
	{
		Load();
	}

	[NaughtyAttributes.Button("Save")]
	public void Save()
	{
		Saver saver = new Saver(gameHandler);
		saver.Save();
	}

	[NaughtyAttributes.Button("Load")]
	public void Load()
	{
		Saver saver = new Saver(gameHandler);
		saver.Load();
	}

	[NaughtyAttributes.Button("Load New")]
	public void LoadNew()
	{
		Saver saver = new Saver(gameHandler);
		saver.LoadNew();
	}
}
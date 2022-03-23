using System.IO;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
	[Zenject.Inject] private GameHandler gameHandler;

	private const string SaveName = "SaveFile.data";

	private void Start()
	{
		Load();
	}

	private string GetFullSavePath()
	{
		return Application.persistentDataPath + SaveName;
	}

	[NaughtyAttributes.Button("Save")]
	public void Save()
	{
		Saver saver = new Saver(gameHandler);
		saver.SaveTo(GetFullSavePath());
	}

	[NaughtyAttributes.Button("Load")]
	public void Load()
	{
		Saver saver = new Saver(gameHandler);

		if (!SaveFileExists())
		{
			Debug.Log("No Savefile found -> New Game.");
			saver.LoadNew();
			return;
		}

		saver.LoadFrom(GetFullSavePath());
	}

	private bool SaveFileExists()
	{
		return File.Exists(GetFullSavePath());
	}
}

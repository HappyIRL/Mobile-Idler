using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Saver
{
	private readonly GameHandler gameHandler;
	private const string SaveName = "SaveFile.data";

	public Saver(GameHandler gameHandler)
	{
		this.gameHandler = gameHandler;
	}

	public void Save()
	{
		var stream = File.Open(GetFullSavePath(), FileMode.OpenOrCreate);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, gameHandler.GetGameData());
		stream.Close();
	}

	public void Load()
	{
		if (!SaveFileExists())
		{
			Debug.Log("No Savefile found -> New Game.");
			LoadNew();
			return;
		}

		var stream = File.OpenRead(GetFullSavePath());
		BinaryFormatter formatter = new BinaryFormatter();

		GameData? gameData = formatter.Deserialize(stream) as GameData?;
		gameHandler.SetGameData(gameData);

		stream.Close();
	}

	public void LoadNew()
	{
		gameHandler.SetGameData(null);
	}

	private bool SaveFileExists()
	{
		return File.Exists(GetFullSavePath());
	}

	private string GetFullSavePath()
	{
		return Application.persistentDataPath + SaveName;
	}
}
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Saver
{
	private readonly GameHandler gameHandler;

	public Saver(GameHandler gameHandler)
	{
		this.gameHandler = gameHandler;
	}

	public void SaveTo(string path)
	{
		var stream = File.Open(path, FileMode.OpenOrCreate);
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, gameHandler.GetGameData());
		stream.Close();
	}

	public void LoadFrom(string path)
	{
		var stream = File.OpenRead(path);
		BinaryFormatter formatter = new BinaryFormatter();

		GameData? gameData = formatter.Deserialize(stream) as GameData?;
		gameHandler.SetGameData(gameData);

		stream.Close();
	}

	public void LoadNew()
	{
		gameHandler.SetGameData(null);
	}
}

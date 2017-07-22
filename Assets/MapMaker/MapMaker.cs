using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MapMaker : MonoBehaviour {

	public string directory;

	private string[][] currentLevel;

	public GameObject spawnPrefab;
	public GameObject wallPrefab;
	public GameObject goalPerfab;

	private const string s_spawn = "2";
	private const string s_goal = "3";
	private const string s_pathway = "0";
	private const string s_wall = "1";

	public void makeMap(string file, float cellSize) {
		currentLevel = LoadLevel(file);
		Populate(currentLevel, cellSize);
	}

	private string[][] LoadLevel(string file){
		file = directory + "\\" + file;
		string text = System.IO.File.ReadAllText(file);
		string[] lines = Regex.Split(text, "\r\n");
		int rows = lines.Length;

		string[][] levelBase = new string[rows][];
		for (int i = 0; i < lines.Length; i++)  {
			string[] stringsOfLine = Regex.Split(lines[i], ",");
			levelBase[i] = stringsOfLine;
		}
		return levelBase;
	}

	private void Populate(string [][] level, float cellSize) {
		for(int z = 0; z < level.Length; z++) {
			for(int x = 0; x < level[0].Length; x++) {
				switch(level[z][x]) {
				case s_spawn:
					PlacePlayerSpawn(new Vector3(x * cellSize, cellSize/2, -z * cellSize));
					break;
				case s_goal:
					PlaceGoal(new Vector3(x * cellSize, cellSize/2, -z * cellSize));
					break;
				case s_pathway:
					break;
				case s_wall:
					PlaceWall(new Vector3(x * cellSize, cellSize/2, -z * cellSize));
					break;
				}
			}
		}
	}

	private void PlaceWall(Vector3 position)
	{
		GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
	}

	private void PlacePlayerSpawn(Vector3 position)
	{
		GameObject playerSpawn = Instantiate(spawnPrefab, position, Quaternion.identity);
	}

	private void PlaceGoal(Vector3 position)
	{
		GameObject goal = Instantiate(goalPerfab, position, Quaternion.identity);
	}
}

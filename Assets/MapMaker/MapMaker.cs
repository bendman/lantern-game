using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MapMaker : MonoBehaviour {

	public string directory;

	private string[][] currentLevel;

	public GameObject spawnPrefab;
	public GameObject wallPrefab;
	public GameObject floorPrefab;
	public GameObject goalPerfab;

	public const string s_spawn = "2";
	public const string s_goal = "3";
	public const string s_pathway = "0";
	public const string s_wall = "1";

	public void makeMap(string file, float cellSize) {
		currentLevel = LoadLevel(file);
		Populate(currentLevel, cellSize);
	}

	private string[][] LoadLevel(string file){
		file = directory + "/" + file;
		string text = System.IO.File.ReadAllText(file);
		string[] lines = Regex.Split(text, "[\r\n]{1,2}");
		int rows = lines.Length;

		string[][] levelBase = new string[rows][];
		for (int i = 0; i < lines.Length; i++)  {
			string[] stringsOfLine = Regex.Split(lines[i], ",");
			levelBase[i] = stringsOfLine;
		}
		return levelBase;
	}

	private void Populate(string [][] level, float cellSize) {
		float tempX, tempZ;

		PlaceFloor(new Vector3(0f + (cellSize/2),0f,0f + (cellSize/2)));

		for(int z = 0; z < level.Length; z++) {
			for(int x = 0; x < level[0].Length; x++) {
				tempX = (x*cellSize) - (level.Length*cellSize/2);
				tempZ = (-z*cellSize) + (level[0].Length*cellSize/2);

				switch(level[z][x]) {
				case s_spawn:
					PlacePlayerSpawn(new Vector3(tempX + (cellSize/2), 0f, tempZ + (cellSize/2)));
					break;
				case s_goal:
					PlaceGoal(new Vector3(tempX + (cellSize/2), cellSize/2, tempZ + (cellSize/2)));
					break;
				case s_pathway:
					break;
				case s_wall:
					PlaceWall(new Vector3(tempX + (cellSize/2), wallPrefab.transform.localScale.y/2, tempZ + (cellSize/2)));
					break;
				}
			}
		}
	}

	private void PlaceFloor(Vector3 postion) {
		GameObject floor = Instantiate(floorPrefab, postion, Quaternion.identity);
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

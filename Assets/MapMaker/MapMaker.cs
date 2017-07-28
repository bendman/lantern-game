using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class MapMaker : NetworkBehaviour {

	public string directory;

	private string[][] currentLevel;

	private GameObject wallContainer;
	private GameObject torchContainer;

	public GameObject spawnPrefab;
	public GameObject wallPrefab;
	public GameObject floorPrefab;
	public GameObject goalPerfab;
	public GameObject lanternPrefab;

	public const string s_spawn = "2";
	public const string s_goal = "3";
	public const string s_pathway = "0";
	public const string s_wall = "1";

	public void MakeMap(string file, float cellSize) {
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

				if (level[z][x] != s_wall && // Not within walls
					x > 1 && x < level[0].Length - 1 && z > 0 && z < level.Length - 2 &&
					x % 2 != z % 2) // every other space
				{
					// TODO: orient lantern to nearest wall
					PlaceLantern(
						new Vector3( tempX + (cellSize / 2), 4f, tempZ + (cellSize / 2)),
						level,
						new Vector2(z, x)
					);
				}

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

	private Vector2 GetNearestWall(string[][] level, Vector2 gridSquare)
	{
		int x = Mathf.FloorToInt(gridSquare.x);
		int y = Mathf.FloorToInt(gridSquare.y);
		if (x-1 >= 0 && level[x-1][y] == s_wall) { return Vector2.left; }
		else if (x+1 <= level.Length - 1 && level[x+1][y] == s_wall) { return Vector2.right; }
		else if (y-1 >= 0 && level[x][y-1] == s_wall) { return Vector2.down; }
		else { return Vector2.up; }
	}

	private void PlaceFloor(Vector3 postion) {
		GameObject floor = Instantiate(floorPrefab, postion, Quaternion.identity);
		NetworkServer.Spawn(floor);
	}

	private void PlaceWall(Vector3 position)
	{
		if (!wallContainer)
		{
			wallContainer = new GameObject("Walls");
			wallContainer.transform.position = Vector3.zero;
			NetworkServer.Spawn(wallContainer);
		}

		GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
		wall.transform.parent = wallContainer.transform;
		NetworkServer.Spawn(wall);
	}

	private void PlacePlayerSpawn(Vector3 position)
	{
		GameObject playerSpawn = Instantiate(spawnPrefab, position, Quaternion.identity);
		NetworkServer.Spawn(playerSpawn);
	}

	private void PlaceGoal(Vector3 position)
	{
		GameObject goal = Instantiate(goalPerfab, position, Quaternion.identity);
		NetworkServer.Spawn(goal);
	}

	private void PlaceLantern(Vector3 position, string[][] level, Vector2 gridSquare)
	{
		if (!torchContainer)
		{
			torchContainer = new GameObject("Torches");
			torchContainer.transform.position = Vector3.zero;
			NetworkServer.Spawn(torchContainer);
		}

		Vector2 nearestWallAngle = GetNearestWall(level, gridSquare);
		float wallAngle = Vector2.Angle(Vector2.left, nearestWallAngle);
		Quaternion wallRotation = Quaternion.Euler(0, wallAngle, 0);
		GameObject torch = Instantiate(lanternPrefab, position, wallRotation);
		torch.transform.parent = torchContainer.transform;
		NetworkServer.Spawn(torch);
	}

}

using System;
using System.Collections.Generic;
using UnityEngine;
using random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class count
	{
		public int minimum;
		public int maximum;

		public count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	//setting dimensions of our board
	public int Columns = 8;
	public int Rows = 8;

	//setting gameobject to put in our board
	public count wallCount = new count (5, 9);
	public count foodCount = new count (1, 5);
	public GameObject Exit; 
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] EnemyTiles;
	public GameObject[] outerwallTiles; 

	private Transform boardHolder;
	private List <Vector3> gridPosition = new List <Vector3>();

	void IntialiseList()
	{
		gridPosition.Clear ();
		for(int x=1; x < Columns-1; x++){
			for(int y=1; y< Rows-1; y++){
				gridPosition.Add (new Vector3 (x, y, 0f));
			}
		}
	}	

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;

		for(int x=-1; x<Columns+1; x++){
			for(int y=-1; y<Rows+1; y++){
				GameObject ToIntantiate = floorTiles[random.Range(0,floorTiles.Length)];
				if(x==-1 || x==Columns || y==-1 || y==Rows){
					ToIntantiate = outerwallTiles [random.Range (0,outerwallTiles.Length)];
				}
				GameObject Instance = Instantiate (ToIntantiate, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
				Instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition(){
		int RandomIndex = random.Range (0,gridPosition.Count);
		Vector3 RandomPosition = gridPosition [RandomIndex];
		gridPosition.RemoveAt (RandomIndex);
		return RandomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max){
		int objectCount = random.Range (min,max+1);

		for(int i=0; i<objectCount; i++){
			Vector3 RandomPos = RandomPosition ();
			GameObject tileChoice = tileArray [random.Range (0, tileArray.Length)];
			Instantiate (tileChoice , RandomPos , Quaternion.identity);
		}
	}

	public void SetupScene(int level){
		BoardSetup ();
		IntialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

		int enemyCount = (int)Mathf.Log (level,2f);
		LayoutObjectAtRandom (EnemyTiles, enemyCount, enemyCount);
		Instantiate (Exit, new Vector3(Columns-1,Rows-1,0),Quaternion.identity  );
	}
}

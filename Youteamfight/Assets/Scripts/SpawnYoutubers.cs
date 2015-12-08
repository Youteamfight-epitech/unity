using UnityEngine;
using System.Collections;

public class SpawnYoutubers : MonoBehaviour {

	public	bool			startSpawning;
	public	float			gameDuration;
	public	Material[]		YTBList;
	private	GameObject[]	spawnList;
	private	float			countdown;
	private float			timeSpawn;
	private	int				allyToSpawn;
	private	int				enemyToSpawn;
	private bool			spawn;

	// Use this for initialization
	void Start () 
	{
		countdown = 0.0f;
		timeSpawn = 2.5f;
		spawnList = GameObject.FindGameObjectsWithTag("Spawn");
	}
	
	// Update is called once per frame
	void Update () 
	{
		int	randYtb;
		int	randSpawn;
		//add the time between each update
		countdown += Time.deltaTime;
		//Start to spawn entity if startSpawning is true
		if (startSpawning == true)
		{
			StartSpawning();
			startSpawning = false;
		}
		//Check if conditions are ok to spawn entity
		if (countdown >= timeSpawn && spawn == true) 
		{
			//Reset the timer
			countdown = 0.0f;
			//Determine which entity to spawn
			if (allyToSpawn == 0)
				randYtb = 1;
			else if (enemyToSpawn == 0)
				randYtb = 0;
			else
				randYtb = (int)(Mathf.Round(Random.value));
			//Decrease counter of entity to spawn
			if (randYtb == 0)
				allyToSpawn -= 1;
			else
				enemyToSpawn -= 1;
			//Choose randomly the spawn
			randSpawn = (int)(Mathf.Round(Random.Range(0.0f, spawnList.Length - 1)));
			//Instantiate the entity
			spawnList[randSpawn].GetComponent<MeshRenderer>().material = YTBList[randYtb];
			spawnList[randSpawn].GetComponent<Animator>().SetTrigger("play2");
	//			Animation[0].wrapMode = WrapMode.Once;

			//spawnList[randSpawn].GetComponent<Animator>().SetBool("play", false);
			//Check if there is no entity left to spawn
			if (allyToSpawn == 0 && enemyToSpawn == 0)
				spawn = false;
		}
	}

	int StartSpawning()
	{
		int nbToSpawn;

		//Calculate the number of entity to spawn
		nbToSpawn = (int)(Mathf.Round (gameDuration / timeSpawn));
		//Calculate randomly how many entity will be ally or enemy
		allyToSpawn = (int)(Mathf.Round (Random.Range (0.0f, nbToSpawn - 1)));
		enemyToSpawn = nbToSpawn - allyToSpawn;
		//Set spawn to begin the update
		spawn = true;
		return (allyToSpawn);
	}
}

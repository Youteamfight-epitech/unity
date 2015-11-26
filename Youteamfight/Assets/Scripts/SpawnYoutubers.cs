using UnityEngine;
using System.Collections;

public class SpawnYoutubers : MonoBehaviour {

	public	GameObject[]	YTBList;
	private	GameObject[]	spawnList;
	private	double			countdown;
	private	int				randYtb;
	private	int				randSpawn;

	// Use this for initialization
	void Start () 
	{
		countdown = 0.0f;
		spawnList = GameObject.FindGameObjectsWithTag("Spawn");
	}
	
	// Update is called once per frame
	void Update () 
	{
		countdown += Time.deltaTime;
		if (countdown >= 2.5f) 
		{
			countdown = 0.0f;
			randYtb = (int)(Mathf.Round(Random.value));
			randSpawn = (int)(Mathf.Round(Random.Range(0.0f, spawnList.Length - 1)));
			Instantiate(YTBList[randYtb], spawnList[randSpawn].transform.position, Quaternion.identity);
		}
	}
}

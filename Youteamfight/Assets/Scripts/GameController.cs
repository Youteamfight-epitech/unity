using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random; 

public class GameController : MonoBehaviour {

	public Transform environnement;
	public GameObject[] terrain;
	public Text text;

	public bool inTransition = false;
	public float speed = 5.0f;

	private TransitionText transit;

	private Vector3 currentPosition;
	private Vector3 previousPosition;
	private Vector3 nextPosition;

	private GameController gameController;
	private GameObject previousLvl;
	private GameObject currentLvl;
	private GameObject nextLvl;
	
	private int lvl = 0;

	private int counter = 0;

	private float currentTime = 0f;
	private float totalTime = 5f;


	// Load one lvl
	void Start () {
		text.text = "NOP";
		transit = this.GetComponent<TransitionText>();
		// get GameObject tagged gameController
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		// define position based on light and camera
		currentPosition = new Vector3(environnement.position.x, environnement.position.y, environnement.position.z);

		// randomize the terrain /!\ This part is to complete to count the difficulty of lvl /!\
		GameObject toInstantiate = terrain[Random.Range (0, terrain.Length)];
		GameObject toInstantiate2 = terrain[Random.Range (0, terrain.Length)];

		// instantiate the current lvl
		currentLvl = Instantiate (toInstantiate, currentPosition, Quaternion.identity) as GameObject;

		// if instantiate don't fail
		if (currentLvl != null) {
			// pos : position of the next lvl
			Vector3 pos = new Vector3(currentPosition.x + currentLvl.GetComponent<Collider>().bounds.size.x, currentPosition.y, currentPosition.z);

			// instantiate the next lvl at position "pos"
			nextLvl = Instantiate (toInstantiate2, pos, Quaternion.identity) as GameObject;

			// if instantiate don't fail : set all parent lvl to GameController
			if (nextLvl != null) {
				currentLvl.transform.SetParent(gameController.transform);
				nextLvl.transform.SetParent(gameController.transform);
			}
			else
				Debug.Log("Failed to instantiate the next lvl");
		}
		else
			Debug.Log("Failed to instantiate the current lvl");
		// at the beginning there is no preivous lvl
		previousLvl = null;
	}

	void setCounter(int nb) {
		counter = nb;
		text.text = "Tap : " + nb;
	}



	void setTimer()
	{
	}

	void Update() {


		if (Input.GetMouseButtonDown(0) && !inTransition) {
			setCounter(counter + 1);
		}


		// if the user use button /!\ MAY CHANGE to : if the game is over /!\
		if (counter > 10 && !inTransition)
		{

			//set the transition on
			inTransition = true;
			transit.startTransit(1);
			currentTime = 0f;
			// start move the lvl
			startLvl();
			setCounter (0);
		}

		// If the lvl is changing
		if (inTransition) {
			//Calcul time for Vector3.Lerp
			currentTime += Time.deltaTime * speed;
			float perc = currentTime / totalTime;
			previousLvl.GetComponent<Transform>().position = Vector3.Lerp(currentPosition, previousPosition, perc);
			currentLvl.GetComponent<Transform>().position = Vector3.Lerp(nextPosition, currentPosition, perc);
			// if the move is ended : Generate the next lvl and go on normal mode
			if (perc >= 1f && transit.isEnded()) {
				nextGenerateLvl();
				inTransition = false;

			}
		}
	}

	// Starting a Lvl : - Switch to the next lvl (already created) 
	//					- Move them
	void startLvl() {
		// if there is a previous lvl, destroy it, we don't need it anymore
		if (previousLvl != null)
			Destroy(previousLvl);
		// switch lvl 
		previousLvl = currentLvl;
		currentLvl = nextLvl;
		nextLvl = null;
		lvl++;
		// redefine the position before and after with the real size of the scene
		previousPosition = new Vector3(currentPosition.x - previousLvl.GetComponent<Collider>().bounds.size.x, currentPosition.y, currentPosition.z);
		nextPosition = new Vector3(currentPosition.x + currentLvl.GetComponent<Collider>().bounds.size.x, currentPosition.y, currentPosition.z);
	}

	// Generate the next lvl at the right position
	bool nextGenerateLvl() {
		// randomize the next scene
		GameObject toInstantiate = terrain[Random.Range (0, terrain.Length)];
		// The next position with the real size of scene
		Vector3 pos = new Vector3(currentPosition.x + currentLvl.GetComponent<Collider>().bounds.size.x, currentPosition.y, currentPosition.z);
		// instantiate the next lvl
		nextLvl = Instantiate (toInstantiate, pos, Quaternion.identity) as GameObject;
		//if instantiate fails
		if (nextLvl == null) {
			Debug.Log("Failed to instantiate the next lvl");
			return false;
		}
		// set to the scene : Parent GameController
		nextLvl.transform.SetParent(gameController.transform);
		return true;
	}
}

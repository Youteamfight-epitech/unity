using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TransitionText : MonoBehaviour {

	public Text final;
	public float totalTime = 1f;
	public int countdown = 3;
	public int step = 0;

	private float currentTime = 0f;
	private bool started = false;
	private int currentCountDown = 0;

	// Use this for initialization
	void Start () {
		final.text = "";
		started = false;
		step = 0;
	}

	void setWinner(int option = 1) {
		if (option == 1)
			final.text = "Bravo !";
		else
			final.text = "Perfect !";
	}

	void setCountDown(int i)
	{
		final.text = string.Format("{0}", i);
	}

	// Update is called once per frame
	void Update () {
		if (started == false)
			return;
		currentTime += Time.deltaTime;

		// Show text final
		if (step == 0)
		{
			if (currentTime >= totalTime)
			{
				step++;
				currentCountDown = countdown;
				currentTime = 0f;
				setCountDown(currentCountDown);
			}
		}
		// CountDonw
		else if (step == 1)
		{
			if (currentTime >= totalTime)
			{
				currentCountDown--;
				if (currentCountDown == 0){
					step++;
					final.text = "Go !";
				}
				else {
					setCountDown(currentCountDown);
				}
				currentTime = 0f;
			}

		}
		else if (step == 2)
		{
			if (currentTime >= totalTime)
			{
				started = false;
				final.text = "";
			}
		}
	}

	public void startTransit(int op)
	{
		if (started == false)
		{
			started = true;
			setWinner(op);
			currentTime = 0;
			step = 0;
		}
	}

	public bool isEnded()
	{
		if (started == false && step == 2)
		{
			step = 0;
			return true;
		}
		return false;
	}
}

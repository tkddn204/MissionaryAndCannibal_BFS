using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public Text missionaryText;
	public Text cannibalText;
	public Text boatText;
	public GameObject inputCanvas;
	public GameObject resultCanvas;
	public GameObject ProceedCanvas;

	// Use this for initialization
	void Start () {
		goToInputCanvas ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void goToResultCanvas() {
		inputCanvas.SetActive (false);
		resultCanvas.SetActive (true);
		GameObject.Find ("/Managers/AlgorithmManager")
			.GetComponent<AlgorithmManager> ()
			.startBfsAlgorithm (int.Parse(missionaryText.text), int.Parse(cannibalText.text),
				int.Parse(boatText.text));
		GameObject.Find ("/Managers/ProceedManager")
			.GetComponent<ProceedManager> ()
			.saveGameStateList ();
	}

	public void goToInputCanvas() {
		inputCanvas.SetActive (true);
		resultCanvas.SetActive (false);
		ProceedCanvas.SetActive (false);
	}

	public void showProceedCanvas(bool isShown) {
		ProceedCanvas.SetActive (isShown);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmManager : MonoBehaviour {

	public Text totalText;
	public GameObject resultNumberSelecter;
	public string currentNumberSelecterTextStringValue;
	public ArrayList resultNodeList;

	public int maxMissionary;
	public int maxCannibal;
	public int maxBoatNumber;

	public void startBfsAlgorithm(int missionary, int cannibal, int boatNumber) {
		this.maxMissionary = missionary;
		this.maxCannibal = cannibal;
		this.maxBoatNumber = boatNumber;

		var initState = new MAC.State(missionary, cannibal, MAC.Solve.LEFT_BOAT_STATE);
		var root = new MAC.Node(0, 0, null, initState);

		resultNodeList = new MAC.Solve (missionary, cannibal, boatNumber).findAndReturnResultList (root);

		totalText.text = "총 경우의 수 : " + resultNodeList.Count.ToString(); 
		resultNumberSelecter.GetComponent<NumberSelecterScript> ().minNumber = 1;
		resultNumberSelecter.GetComponent<NumberSelecterScript> ().maxNumber = resultNodeList.Count;

		//foreach(ArrayList nodelist in resultNodeList) {
		//	foreach(MAC.Node node in nodelist) {
		//		Debug.Log (node); 
		//	}
		//	break;
		//}
	}
}

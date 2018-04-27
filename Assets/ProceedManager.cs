using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceedManager : MonoBehaviour {

	CharacterManager characterManager;
	AlgorithmManager algorithmManager;
	
	public Text caseNumberSelecterText;
	public Text currentStateText;
	public Slider proceedSlider;
	public Toggle autoToggle;
	public Slider speedSlider;

	private ArrayList proceedList;
	private int maxMissionary;
	private int maxCannibal;


	private int currentProceedIndex;
	private float proceedSliderValue;

	// Use this for initialization
	void Start () {
		characterManager = GameObject.Find ("/Managers/CharacterManager")
			.GetComponent<CharacterManager> ();
		algorithmManager = GameObject.Find ("/Managers/AlgorithmManager")
			.GetComponent<AlgorithmManager> ();
		proceedList = new ArrayList ();
	}

	IEnumerator autoProceed() {
		while (autoToggle.isOn) {
			yield return new WaitForSeconds (speedSlider.value);
			if (autoToggle.isOn) {
				proceedSlider.value++;
			}
		}
		isNowOn = true;
	}

	bool isNowOn = true;
	void Update () {
		if (proceedSliderValue != proceedSlider.value) {
			proceedSliderValue = proceedSlider.value;
			
			// 렌더링
			GameState currentState = ((List<GameState>)proceedList [currentProceedIndex]) [(int)proceedSliderValue];
			characterManager.renderGameState (currentState);
			
			// 현재 상태 텍스트 변경
			currentStateText.text = currentState.printState();
			Debug.Log (currentState);
		}
		if (autoToggle.isOn && isNowOn) {
			StartCoroutine (autoProceed());
			isNowOn = false;
		}
	}

	public void startProceed() {
		currentProceedIndex = int.Parse (caseNumberSelecterText.text) - 1;
		List<GameState> gameStateList = (List<GameState>)proceedList [currentProceedIndex];
		proceedSlider.minValue = 0f;
		proceedSlider.maxValue = (float) gameStateList.Count - 1;
	}

	public void saveGameStateList() {
		maxMissionary = algorithmManager.maxMissionary;
		maxCannibal = algorithmManager.maxCannibal;

		ArrayList resultNodeList = algorithmManager.resultNodeList;
		foreach(ArrayList nodeList in resultNodeList) {
			makeGameState (nodeList);
		}
	}

	private void makeGameState(ArrayList nodeList) {
		List<GameState> gameStateList = new List<GameState> ();

		for (var i = 0; i < nodeList.Count; i++) {
			if (i + 1 < nodeList.Count) {
				MAC.State state = ((MAC.Node)nodeList [i]).state;
				MAC.State nextState = ((MAC.Node)nodeList [i + 1]).state;

				// Ready
				// 노드 자체의 상태
				gameStateList.Add (new GameState (
					state.missionary,
					state.cannibal,
					maxMissionary - state.missionary,
					maxCannibal - state.cannibal,
					state.boatMove ? State.Boat.LEFT_R : State.Boat.RIGHT_L));

				// Move
				// 노드 - 다음 노드 사이의 상태
				if (state.boatMove) {
					// 오른쪽에서 왼쪽

					// RIGHT_RIDDEN_BOAT
					gameStateList.Add (new GameState (
						State.People.RIGHT_RIDDEN_BOAT,
						state.missionary,
						state.cannibal,
						nextState.missionary - state.missionary,
						nextState.cannibal - state.cannibal,
						maxMissionary - nextState.missionary,
						maxCannibal - nextState.cannibal,
						State.Boat.RIGHT_L));

					// MOVING_FROM_RIGHT_TO_LEFT
					gameStateList.Add (new GameState (
						State.People.MOVING_FROM_RIGHT_TO_LEFT,
						state.missionary,
						state.cannibal,
						nextState.missionary - state.missionary,
						nextState.cannibal - state.cannibal,
						maxMissionary - nextState.missionary,
						maxCannibal - nextState.cannibal,
						State.Boat.FROM_RIGHT_TO_LEFT_L));
				} else {
					// 왼쪽에서 오른쪽

					// LEFT_RIDDEN_BOAT
					gameStateList.Add (new GameState (
						State.People.LEFT_RIDDEN_BOAT,
						nextState.missionary,
						nextState.cannibal,
						state.missionary - nextState.missionary,
						state.cannibal - nextState.cannibal,
						maxMissionary - state.missionary,
						maxCannibal - state.cannibal,
						State.Boat.LEFT_R));

					// MOVING_FROM_LEFT_TO_RIGHT
					gameStateList.Add (new GameState (
						State.People.MOVING_FROM_LEFT_TO_RIGHT,
						nextState.missionary,
						nextState.cannibal,
						state.missionary - nextState.missionary,
						state.cannibal - nextState.cannibal,
						maxMissionary - state.missionary,
						maxCannibal - state.cannibal,
						State.Boat.FROM_LEFT_TO_RIGHT_R));
				}

			} else {
				MAC.State lastState = ((MAC.Node)nodeList [i]).state;

				gameStateList.Add (new GameState(
					lastState.missionary,
					lastState.cannibal,
					maxMissionary - lastState.missionary,
					maxCannibal - lastState.cannibal,
					State.Boat.RIGHT_L));
			}
		}

		//foreach(GameState gameState in gameStateList) {
		//	Debug.Log(gameState);
		//}
		//Debug.Log ("----------------------------");

		proceedList.Add(gameStateList);
	}
}

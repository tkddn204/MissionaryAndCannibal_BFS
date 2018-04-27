using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSelecterScript : MonoBehaviour {
	
	private Text numberText;
	private Button leftButton;
	private Button rightButton;
	public int minNumber;
	public int maxNumber;

	// Use this for initialization
	void Start () {
		numberText = transform.Find ("Text").GetComponent<Text> ();
		leftButton = transform.Find ("LeftButton").GetComponent<Button>();
		rightButton = transform.Find ("RightButton").GetComponent<Button>();

		leftButton.onClick.AddListener (minusNumber);
		rightButton.onClick.AddListener (plusNumber);
	}

	public void minusNumber() {
		if (int.Parse(numberText.text) <= minNumber)
			return;
		numberText.text = (int.Parse (numberText.text) - 1).ToString();
	}

	public void plusNumber() {
		if (int.Parse(numberText.text) >= maxNumber)
			return;
		numberText.text = (int.Parse (numberText.text) + 1).ToString();
	}
}

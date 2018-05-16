using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	public static int score = 0;
	private Text myText;

	void Start(){
		myText = this.GetComponent<Text>();
		Reset();
	}

	public void ScorePoints(int points){
		score += points;
		myText.text = score.ToString();
	}

	public static void Reset(){
		score = 0;
	}
	
}

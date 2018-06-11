using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text>().text += PlayerScript.money;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

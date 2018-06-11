using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
	public static float money = 20;
	private float drugBaseCost = 10;
	public float multiplier = 1.2f;
	// Use this for initialization
	public Rigidbody2D body;
	public float spd = 5;
	public float expandedRadius;
	public CircleCollider2D DetectRange;
	//Animation Code:
	public GameObject drug;

	private SpriteRenderer rend;
	public TextMesh text;

	public GameObject[] sellers;
	public static float timeRemaining = 60;
	void droppedDrugs(){
		DetectRange.radius = 0;
		DetectRange.enabled = false;

		drug.gameObject.transform.parent = null;
		drug.gameObject.transform.position = new Vector2(drug.gameObject.transform.position.x + (drug.gameObject.transform.position.x - this.transform.position.x), drug.gameObject.transform.position.y + (drug.gameObject.transform.position.y - this.transform.position.y));
		drug = null;

		drugBaseCost = drugBaseCost / multiplier / multiplier;
		money = 0.1f * money;
	}

	void Start () {
		rend = GetComponent<SpriteRenderer>();

		DetectRange = GetComponent<CircleCollider2D> ();
		body = GetComponent<Rigidbody2D> ();

		sellers = GameObject.FindGameObjectsWithTag("Seller");

		DetectRange.radius = 0;
		DetectRange.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		body.velocity = new Vector2(Input.GetAxis("Horizontal") * spd, Input.GetAxis("Vertical") * spd);
		if (Input.GetKeyDown(KeyCode.Q) && drug != null) {
			droppedDrugs ();
		}

		//deal with money
		text.text = "$" + String.Format("{0:0.00}", money);
		if(money < 0) {
			GAME_OVER();
		}
		timeRemaining -= Time.deltaTime;
		text.text += "\n (" + String.Format("{0:0.00}", timeRemaining) + ") to go";
		if(timeRemaining < 0) {
			GAME_OVER();
		}
	}



	public void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Drugs") {
			collision.transform.parent = this.gameObject.transform;
			collision.GetComponent<BoxCollider2D>().isTrigger = true;
			drug = collision.gameObject;
			DetectRange.radius = expandedRadius;
			DetectRange.enabled = true;
		} 
		else if (collision.tag == "Buyer") {
			if (drug != null) {
				money += drugBaseCost;
				drugBaseCost *= multiplier;
				DetectRange.radius = 0;
				DetectRange.enabled = false;

				drug.gameObject.transform.parent = null;
				drug.transform.position = (Vector2)sellers[(int)(UnityEngine.Random.value * sellers.Length)].transform.position;
				drug = null;
			}
		}
	}
	public static void GAME_OVER() {
		SceneManager.LoadScene("GameEnd");
	}
}

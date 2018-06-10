using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class wasd : MonoBehaviour {

	// Use this for initialization
	public Rigidbody2D body;
	public float spd = 5;
	public float expandedRadius;
	public CircleCollider2D DetectRange;
	//Animation Code:
	public Sprite[] frames; 
	public GameObject drug;
	public float framesPerSecond = 10.0f;

	void droppedDrugs(){
		DetectRange.radius = 0;

		drug.gameObject.transform.parent = null;
		drug.gameObject.GetComponent<BoxCollider2D> ().isTrigger = false;
		//drug.gameObject.GetComponent<Rigidbody2D> ().AddForce (body.velocity*2f);
		drug.gameObject.transform.position = new Vector2(drug.gameObject.transform.position.x + (drug.gameObject.transform.position.x - this.transform.position.x), drug.gameObject.transform.position.y + (drug.gameObject.transform.position.y - this.transform.position.y));
		drug = null;


	}

	void Start () {
		DetectRange = this.GetComponent<CircleCollider2D> ();
		body = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		int index = Convert.ToInt32(Time.time * framesPerSecond); 
		index = index % frames.Length; 
		GetComponent<SpriteRenderer>().sprite = frames[index]; 

		body.velocity = new Vector2(Input.GetAxis("Horizontal") * spd, Input.GetAxis("Vertical") * spd);
		if (Input.GetKeyDown(KeyCode.Q) && drug != null) {
			droppedDrugs ();
		}
	}



	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Drugs") {
			col.gameObject.transform.parent = this.gameObject.transform;
			col.gameObject.GetComponent<BoxCollider2D> ().isTrigger = true;
			drug = col.gameObject;
			DetectRange.radius = expandedRadius;
		}
	}
}

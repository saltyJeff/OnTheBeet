using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour {
	private Rigidbody2D rb;
	public float width;
	public static Vector2[] waypoints;
	public string waypointTag;
	public float speed = 1;
	public LayerMask npcLayers;
	private Vector2 moveTo;
	private RaycastHit2D hit;
	private Vector2 currentMotion;
	public bool onPath = true;
	public HashSet<Collider2D> partOf = new HashSet<Collider2D>();
	// Use this for initialization
	public virtual void Start () {
		rb = GetComponent<Rigidbody2D>();
		if(waypoints == null) {
			GameObject[] waypointObjs = GameObject.FindGameObjectsWithTag(waypointTag);
			Debug.Log(waypointObjs.Length);
			waypoints = new Vector2[waypointObjs.Length];
			for(int i = 0; i < waypointObjs.Length; i++) {
				waypoints[i] = waypointObjs[i].transform.position;
			}
		}
		selectPoint();
		currentMotion = (moveTo - (Vector2)transform.position).normalized;
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if(Vector2.Distance(transform.position, moveTo) < 2) {
			//arrived at position, goto next position
			selectPoint();
		}
		else {
			//keep moving
			Vector2 motionToGoal = (moveTo - (Vector2)transform.position).normalized;
			bool goalBlocked = Physics2D.CircleCast(transform.position, width, motionToGoal, 1, npcLayers);
			bool forwardBlock = Physics2D.CircleCast(transform.position, width, currentMotion, 1, npcLayers);
			if (onPath) {
				if (forwardBlock) {
					Debug.Log("obstacle avoided");
					currentMotion = currentMotion.Rotate(60);
					onPath = false;
				}
				else {
					currentMotion = motionToGoal;
				}
			}
			else {
				if (!goalBlocked) {
					currentMotion = motionToGoal;
					onPath = true;
				}
				else if(forwardBlock) {
					Debug.Log("obstacle avoided");
					currentMotion = Random.value < 0.5 ? currentMotion.Rotate(60) : currentMotion.Rotate(-60);
					onPath = false;
				}
			}
			Debug.DrawRay(transform.position, currentMotion);
			rb.velocity = currentMotion * speed;
		}
	}

	public void selectPoint() {
		var nxt = moveTo;
		while(nxt == moveTo) {
			nxt = waypoints[(int)(Random.value * waypoints.Length)];
		}
		moveTo = nxt;
	}
	public virtual void OnInterest() {}
	public virtual void OnUninterest() { }

	public void OnTriggerEnter2D(Collider2D collider) {
		if(collider.tag == "Drugs" || collider.tag == "Buyer" || collider.tag == "Seller") {
			return;
		}
		//hit = Physics2D.Raycast(transform.position, collider.transform.position - transform.position);
		//if((!hit || hit.collider.gameObject == collider.gameObject) && !partOf.Contains(collider)) {
		//	partOf.Add(collider);
		//	if(partOf.Count == 1) {
		//		OnInterest();
		//	}
		//}
		if(!partOf.Contains(collider)) {
			Debug.Log(collider.name);
			OnInterest();
			partOf.Add(collider);
		}
	}
	public void OnTriggerExit2D(Collider2D collider) {
		//hit = Physics2D.Raycast(transform.position, collider.transform.position - transform.position);
		//if ((!hit || hit.collider.tag == "Player") && partOf.Contains(collider)) {
		//	partOf.Remove(collider);
		//	if(partOf.Count == 0) {
		//		OnUninterest();
		//	}
		//}
		if(collider.tag == "Drugs" || collider.tag == "Buyer" || collider.tag == "Seller") {
			return;
		}
		if (partOf.Contains(collider)) {
			Debug.Log(collider.name);
			OnUninterest();
			partOf.Remove(collider);
		}
	}
}

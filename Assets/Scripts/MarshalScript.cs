using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshalScript : NPCScript {
	private CircleCollider2D alertCollider;
	public float expandedRadius;
	public float countdown;
	public TextMesh text;
	private float countdownStart;
	public bool counting = false;
	private float startSpeed;
	public override void Start() {
		startSpeed = base.speed;
		base.Start();
		tag = "Marshal";
		alertCollider = GetComponent<CircleCollider2D>();
	}
	// Update is called once per frame
	void Update() {
		if (counting) {
			if (Time.time > countdownStart + countdown) {
				alertCollider.radius = expandedRadius;
				text.text = "Game Over";
				PlayerScript.GAME_OVER();
			}
			else {
				text.text = "? " + String.Format("{0:0.00}", countdownStart + countdown - Time.time);
			}
		}
		else {
			text.text = "OK";
		}
	}
	public override void OnInterest() {
		counting = true;
		countdownStart = Time.time;
		speed = 0;
	}
	public override void OnUninterest() {
		counting = false;
		alertCollider.radius = 0;
		speed = startSpeed;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivillianScript : NPCScript {
	private CircleCollider2D alertCollider;
	public float expandedRadius;
	public float countdown;
	public TextMesh text;
	private float countdownStart;
	public bool counting = false;
	public override void Start() {
		base.Start();
		tag = "Civilian";
		alertCollider = GetComponent<CircleCollider2D>();
	}
	// Update is called once per frame
	void Update () {
		if(counting) {
			if(Time.time > countdownStart + countdown) {
				alertCollider.radius = expandedRadius;
				text.text = "!";
			}
			else {
				text.text = "? "+ String.Format("{0:0.00}", countdownStart + countdown - Time.time);
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
		speed = 3;
	}
}

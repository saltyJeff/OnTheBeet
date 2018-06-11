using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopScript : NPCScript {
	private static float baseCost = 3;
	public float multiplier = 1.2f;
	public float countdown;
	public TextMesh text;
	private float countdownStart;
	public bool counting = false;

	private float startSpeed;
	public bool paidOff = false;
	public override void Start() {
		base.Start();
		startSpeed = base.speed;
		tag = "Cop";
	}
	// Update is called once per frame
	void Update() {
		if (counting) {
			if (Time.time > countdownStart + countdown) {
				text.text = "BRIBED FOR $"+baseCost;
				speed = 0;
				if(!paidOff) {
					baseCost *= multiplier;
					PlayerScript.money -= baseCost;
					paidOff = true;
				}
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
		speed = startSpeed;
		paidOff = false;
	}
}

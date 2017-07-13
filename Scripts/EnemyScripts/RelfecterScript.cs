﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelfecterScript : MonoBehaviour
{

	GameController GCS;
	Rigidbody2D rb;
	float X, VerticalSpeed = 0.1f, HorizontalSpeed = 0.2f;
	bool MovingLeft = false;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
	}


	void FixedUpdate ()
	{
		if (!GCS.GamePaused) {
			if (transform.position.x > 23f)
				MovingLeft = true;
			if (transform.position.x < -23f)
				MovingLeft = false;
			X = MovingLeft ? transform.position.x - HorizontalSpeed : transform.position.x + HorizontalSpeed;
			rb.MovePosition (new Vector2 (X, transform.position.y - VerticalSpeed));
		}
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "PlayerAmmunition") {
			collision.transform.Rotate (new Vector3 (0, 0, 180));
			collision.GetComponent<LaserDiscScript> ().TravelDistance = -0.35f;

			ParticleSystem tmp = collision.GetComponentInChildren<ParticleSystem> ();
			ParticleSystem.MainModule tmp2 = tmp.main;
			tmp2.gravityModifier = 0.1f;

			collision.GetComponent<Rigidbody2D> ().mass += 1f;
			collision.tag = "EnemyAmmunition";
		}
		if (collision.tag == "Raygun" || collision.tag == "MissileExplosion" || collision.tag == "Player")
			Die ();
	}

	public void Die ()
	{
		GCS.DropCollectable (0.3f, transform.position);
		GCS.Score += 250;
		Instantiate (GCS.EnemyExplosions [3], transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}

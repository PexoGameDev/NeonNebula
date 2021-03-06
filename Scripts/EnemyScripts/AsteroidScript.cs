﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
	//Publics
	public int HP = 2, Score = 25;
	public float ExplosionScale = 0f;
	//Privates
	GameController GCS;
	Rigidbody2D rb;
	float defaultx;
	float rotateX, rotateY, rotateZ;

	void Start ()
	{
		rb = gameObject.GetComponent<Rigidbody2D> ();
		defaultx = rb.position.x;
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		rotateZ = Random.Range (0f, 1f) / 10;
		HP += GCS.Difficulty;
	}

	void Update ()
	{

		if (!GCS.GamePaused) {
			rb.MovePosition (new Vector2 (defaultx, rb.position.y - 0.1f));
			transform.Rotate (0f, 0f, rotateZ);
		}

	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.tag == "PlayerAmmunition")
			HP--;

		if (collision.tag == "Raygun" || collision.tag == "MissileExplosion")
			HP -= 5;

		if (HP < 1 || collision.tag == "Player")
			Die ();
	}

	public void Die ()
	{
		GameObject Boom = Instantiate (GCS.EnemyExplosions [0], transform.position, Quaternion.identity);
		Boom.transform.localScale += new Vector3 (ExplosionScale, ExplosionScale, ExplosionScale);
		GCS.DropCollectable (0.05f, transform.position);
		GCS.Score += Score;
		Destroy (gameObject);
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerScript : MonoBehaviour
{

	public float GivenY = 6f;
	public float Speed = 0.05f;
	bool MovingLeft = true;
	GameController GCS;
	Rigidbody2D rb;
	float defaultY;
	int HP = 1;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		HP += GCS.Difficulty;
		StartCoroutine (MovementVertical ());
		StartCoroutine (Shooting ());
        
	}

	IEnumerator MovementVertical ()
	{
		float startx = transform.position.x;
		while (transform.position.y > GivenY) {
			if (!GCS.GamePaused) {
				rb.MovePosition (new Vector2 (startx, transform.position.y - 0.25f));
				yield return new WaitForSeconds (0.01f);
			} else
				yield return new WaitForEndOfFrame ();
		}
		defaultY = transform.position.y;
		StartCoroutine (MovementHorizontal ());
	}

	IEnumerator MovementHorizontal ()
	{
        
		for (;;) {

			if ((transform.position.x < -23 && MovingLeft) || (!MovingLeft && transform.position.x > 23))
				MovingLeft = !MovingLeft;

			if (!GCS.GamePaused) {
				if (MovingLeft)
					rb.MovePosition (new Vector2 (transform.position.x - Speed, defaultY));
				else
					rb.MovePosition (new Vector2 (transform.position.x + Speed, defaultY));
				yield return new WaitForSeconds (0.01f);

			} else
				yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator Shooting ()
	{
		float WaitTime = 0f;
		yield return new WaitForSeconds (Random.Range (1f, 4f));
		for (;;) {
			if (!GCS.GamePaused) {
				WaitTime = Random.Range (1.2f, 3f);
				Instantiate (GCS.EnemyAmmunition [2], new Vector3 (transform.position.x, transform.position.y - 1.45f, transform.position.z), Quaternion.identity);
				for (int i = 0; i < 101; i++)
					if (GCS.GamePaused) {
						i--;
						yield return new WaitForEndOfFrame ();
					} else
						yield return new WaitForSeconds (WaitTime / 100);
			} else
				yield return new WaitForEndOfFrame ();
		}
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{

		if (collision.tag == "PlayerAmmunition")
			HP--;
		if (collision.tag == "Raygun" || collision.tag == "MissileExplosion")
			HP -= 5;
		if (collision.tag == "Player")
			HP = 0;

		if (HP < 1)
			Die ();
	}

	public void Die ()
	{
		GCS.DropCollectable (0.1f, transform.position);
		Instantiate (GCS.EnemyExplosions [4], transform.position, Quaternion.identity);
		GCS.Score += 50;
		Destroy (gameObject);
	}
}


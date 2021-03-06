﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerScript : MonoBehaviour
{

	GameController GCS;
	Rigidbody2D rb;
	bool MovingLeft = false, MovingUp = false;
	int HP = 4;
	float Y = 0;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		StartCoroutine (Shooting ());
		StartCoroutine (Movement ());
		HP += GCS.Difficulty;
	}

	void FixedUpdate ()
	{
		if (!GCS.GamePaused)
			rb.MoveRotation (rb.rotation + 100 * Time.fixedDeltaTime); 
	}

	IEnumerator Shooting ()
	{
		float WaitTime = 3f;
		for (;;) {
			if (GCS.GamePaused)
				yield return new WaitForEndOfFrame ();
			else {
				Instantiate (GCS.EnemyAmmunition [0], new Vector3 (transform.position.x, transform.position.y - 1f, 0f), Quaternion.identity);
				for (int i = 0; i < 101; i++)
					if (GCS.GamePaused) {
						i--;
						yield return new WaitForEndOfFrame ();
					} else
						yield return new WaitForSeconds (WaitTime / 100);
				if (WaitTime > 0.8f)
					WaitTime -= 0.05f;
			}
		}
	}

	IEnumerator Movement ()
	{
		for (;;) {
			if (!GCS.GamePaused) {

				if ((transform.position.x < -20 && MovingLeft) || (!MovingLeft && transform.position.x > 20))
					MovingLeft = !MovingLeft;
				if ((transform.position.y < -2 && !MovingUp) || (transform.position.y >= 6f && MovingUp))
					MovingUp = !MovingUp;
                 



				if (MovingUp)
					Y = transform.position.y + 0.05f;
				else
					Y = transform.position.y - 0.05f;

				if (MovingLeft)
					rb.MovePosition (new Vector2 (transform.position.x - 0.1f, Y));
				else
					rb.MovePosition (new Vector2 (transform.position.x + 0.1f, Y));
				yield return new WaitForSeconds (0.01f);
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
		if (HP < 1 || collision.tag == "Player")
			Die ();
	}

	public void Die ()
	{
		GCS.DropCollectable (0.2f, transform.position);
		Instantiate (GCS.EnemyExplosions [2], transform.position, Quaternion.identity);
		GCS.Score += 175;
		Destroy (gameObject);
	}
}

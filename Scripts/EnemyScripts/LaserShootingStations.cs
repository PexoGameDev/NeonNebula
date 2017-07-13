﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShootingStations : MonoBehaviour
{
	public float HP = 6;
	public GameObject Boss, Laser;
	BossScript BossScript;
	GameController GCS;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		BossScript = Boss.GetComponent<BossScript> ();
		StartCoroutine (Shooting ());
		HP += GCS.Difficulty * 2;
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
		BossScript.ModuleDestroyed ();
		GCS.DropCollectable (0.4f, transform.position);
		Instantiate (GCS.EnemyExplosions [1], transform.position, Quaternion.identity);
		GCS.Score += 500;
		Destroy (gameObject);
	}

	IEnumerator Shooting ()
	{
		float WaitTime = Random.Range (1.2f, 3f);
		yield return new WaitForSeconds (WaitTime);
		for (;;) {
			if (GCS.GamePaused)
				yield return new WaitForEndOfFrame ();
			else {
				Instantiate (Laser, new Vector3 (transform.position.x, transform.position.y - 3f, 0f), Quaternion.identity);
				for (int i = 0; i < 101; i++)
					if (GCS.GamePaused) {
						i--;
						yield return new WaitForEndOfFrame ();
					} else
						yield return new WaitForSeconds (WaitTime / 100);
				if (WaitTime > 0.6f)
					WaitTime -= 0.05f;
			}
		}
	}
}

﻿using System.Collections;
using UnityEngine;

public class EnemySpawnerModule : MonoBehaviour
{

	public float HP = 6;
	public GameObject Boss;
	BossScript BossScript;
	GameController GCS;

	void Start ()
	{
		GCS = GameObject.FindGameObjectWithTag ("GameScripts").GetComponent<GameController> ();
		BossScript = Boss.GetComponent<BossScript> ();
		HP += GCS.Difficulty * 2;
		StartCoroutine (Spawning ());
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

	IEnumerator Spawning ()
	{
		float WaitTime = 6f;
		yield return new WaitForSeconds (WaitTime);
		for (;;) {
			if (GCS.GamePaused)
				yield return new WaitForEndOfFrame ();
			else {
				GameObject tmp = Instantiate (GCS.Enemies [(int)Random.Range (1f, 5f)], new Vector3 (transform.position.x, transform.position.y, 0f), Quaternion.identity);
                tmp.transform.SetParent(GCS.transform);
				for (int i = 0; i < 101; i++)
					if (GCS.GamePaused) {
						i--;
						yield return new WaitForEndOfFrame ();
					} else
						yield return new WaitForSeconds (WaitTime / 100);
				if (WaitTime > 3f)
					WaitTime -= 0.05f;
			}
		}
	}

	public void Die ()
	{
		GCS.Score += 500;
		BossScript.ModuleDestroyed ();
		GCS.DropCollectable (0.4f, transform.position);
		Instantiate (GCS.EnemyExplosions [3], transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}

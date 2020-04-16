﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class connection : MonoBehaviour
{
	private string player_name = ""; //Todo: remplacer par PlayerStat.playerName -> Boulot de Adrien/Léo
	private int score = PlayerStat.score;
	[SerializeField] private bool isalive = PlayerStat.isAlive;
	private bool sended = false;

	public void callsendscore() {
		if (!isalive)
		{
			StartCoroutine(Sendscore());
			
		}
	}

	IEnumerator Sendscore()
	{
		WWWForm form = new WWWForm();
		form.AddField("player_name", player_name);
		form.AddField("score", score);
		WWW www = new WWW("http://localhost:8090/site%20web/Accueil/score.php",form);
		yield return www;
		if (www.text == "0")
		{
			Debug.Log("score sended successfully");
		}
		else {
			Debug.Log("score send failed with error code " + www.text);
		}
	}

	private void Update()
	{
		if (!sended)
		{
			callsendscore();
			sended = true;
		}
	}
}

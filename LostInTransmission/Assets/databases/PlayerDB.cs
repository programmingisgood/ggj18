using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDB : MonoBehaviour
{
	public string player1Name = "John";
	public string player2Name = "Kevin";
	public string message = "This is the message";

	public void SetPlayer1Name(string setName)
	{
		player1Name = setName;
	}

	public void SetPlayer2Name(string setName)
	{
		player2Name = setName;
	}

	public string GetPlayer1Name()
	{
		return player1Name;
	}

	public string GetPlayer2Name()
	{
		return player2Name;
	}

	public string GetMessage()
	{
		return message;
	}

	public void SetMessage(string setMessage)
	{
		message = setMessage;
	}
}

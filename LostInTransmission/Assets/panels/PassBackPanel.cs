using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassBackPanel : MonoBehaviour
{
	public PlayerDB playerDB;
	public GameObject writePanel;
	public Text text;
	public Button button;

	void Start()
	{
		button.onClick.AddListener(TaskOnClick);
	}

	void OnEnable()
	{
		text.text = "Pass to " + playerDB.GetPlayer2Name();
	}

	void TaskOnClick()
	{
		gameObject.SetActive(false);
		writePanel.SetActive(true);
	}
}

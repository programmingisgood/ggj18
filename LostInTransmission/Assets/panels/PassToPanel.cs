using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassToPanel : MonoBehaviour
{
	public PlayerDB playerDB;
	public GameObject boxPanel;
	public Text text;
	public Button button;

	void Start()
	{
		button.onClick.AddListener(TaskOnClick);
	}

	void OnEnable()
	{
		text.text = "Pass to " + playerDB.GetPlayer1Name();
	}

	void TaskOnClick()
	{
		gameObject.SetActive(false);
		boxPanel.SetActive(true);
	}
}

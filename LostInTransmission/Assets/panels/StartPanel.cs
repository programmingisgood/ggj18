using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
	public DictionaryTester dict;
	public PlayerDB playerDB;
	public GameObject writePanel;
	public Button begin;
	public Text beginText;
	public InputField name1;
	public InputField name2;
	public Text loadingText;

	void Start()
	{
		begin.onClick.AddListener(TaskOnBegin);
		name1.onValueChanged.AddListener(TaskOnPlayer1NameChanged);
		name2.onValueChanged.AddListener(TaskOnPlayer2NameChanged);
		begin.gameObject.SetActive(false);
		beginText.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		begin.gameObject.SetActive(false);
		beginText.gameObject.SetActive(false);
	}

	void TaskOnBegin()
	{
		gameObject.SetActive(false);
		writePanel.SetActive(true);
	}

	void TaskOnPlayer1NameChanged(string newText)
	{
		playerDB.SetPlayer2Name(newText);
		CheckReadyToPlay();
	}

	void TaskOnPlayer2NameChanged(string newText)
	{
		playerDB.SetPlayer1Name(newText);
		CheckReadyToPlay();
	}

	void CheckReadyToPlay()
	{
		string name1Text = name1.text;
		string name2Text = name2.text;

		bool readyToPlay = (name1Text.Length > 0 && name2Text.Length > 0);

		begin.gameObject.SetActive(readyToPlay);
		beginText.gameObject.SetActive(readyToPlay);

		if (readyToPlay)
		{
			beginText.text = "Very good. " + name2.text + ", please avert your vision and then we may";
		}
	}

	void Update()
	{
		loadingText.text = (int) (dict.GetLoadingProgress() * 100f) + "%";
	}
}

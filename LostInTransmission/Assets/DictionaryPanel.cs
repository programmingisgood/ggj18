using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryPanel : MonoBehaviour
{
	public InputField inputField;
	public Text outputField;
	public Button submit;
	public DictionaryTester dict;

	void Start()
	{
		submit.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		outputField.text = "";

		string inputText = inputField.text;
		string[] words = inputText.Split(' ');
		for (int w = 0; w < words.Length; w++)
		{
			string word = words[w];
			List<string> synonyms = dict.GetWordSynonyms(word);
			if (synonyms != null)
			{
				string randomSyn = synonyms[Random.Range(0, synonyms.Count)];
				outputField.text += randomSyn + " ";
				Debug.Log(word + " = " + randomSyn);
			}
		}
	}
}

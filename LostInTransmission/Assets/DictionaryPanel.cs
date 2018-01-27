using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryPanel : MonoBehaviour
{
	public InputField inputField;
	public Text outputField;
	public DictionaryTester dict;

	private string lastInputText;

	public void Update()
	{
		if (lastInputText != inputField.text)
		{
			lastInputText = inputField.text;
			List<string> synonyms = dict.GetWordSynonyms(inputField.text);
			if (synonyms != null)
			{
				outputField.text = synonyms[Random.Range(0, synonyms.Count)];
			}
		}
	}
}

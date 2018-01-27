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

	public int maxWords = 5;

	public float chanceOfRemoval = 0.5f;
	public float chanceOfSwap = 0.5f;
	public float chanceOfSynonym = 0.3f;

	void Start()
	{
		submit.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		outputField.text = "";

		string inputText = inputField.text;
		string[] words = inputText.Split(' ');
		for (int t = 0; t < words.Length; t++)
		{
			words[t] = words[t].Trim();
		}

		List<string> wordsList = new List<string>(words);
		while (wordsList.Count > maxWords)
		{
			int trashIndex = Random.Range(0, words.Length);
			Debug.Log("Too many, trashing " + wordsList[trashIndex]);
			wordsList.RemoveAt(trashIndex);
		}

		// Verify each word is a real word.
		for (int w = wordsList.Count - 1; w >= 0; w--)
		{
			if (!dict.GetIsWordValid(wordsList[w]))
			{
				Debug.Log("Removing invalid word " + wordsList[w]);
				wordsList.RemoveAt(w);
			}
		}

		if (Random.value <= chanceOfRemoval)
		{
			int removeIndex = Random.Range(0, wordsList.Count);
			Debug.Log("Chance: Removing " + wordsList[removeIndex]);
			wordsList.RemoveAt(removeIndex);
		}

		if (Random.value <= chanceOfSwap)
		{
			int oldIndex = Random.Range(0, wordsList.Count);
			int newIndex = Random.Range(0, wordsList.Count);
			if (wordsList.Count > 1)
			{
				while (newIndex == oldIndex)
				{
					newIndex = Random.Range(0, wordsList.Count);
				}
				Debug.Log("Chance: Swapping " + wordsList[oldIndex]);
				string oldWord = wordsList[oldIndex];
				wordsList.RemoveAt(oldIndex);
				wordsList.Insert(newIndex, oldWord);
			}
		}

		for (int w = 0; w < wordsList.Count; w++)
		{
			if (Random.value <= chanceOfSynonym)
			{
				string word = wordsList[w];
				List<string> synonyms = dict.GetWordSynonyms(word);
				if (synonyms != null)
				{
					string randomSyn = synonyms[Random.Range(0, synonyms.Count)];
					Debug.Log("Chance: " + word + " becomes " + randomSyn);
					wordsList[w] = randomSyn;
				}
			}
		}

		for (int f = 0; f < wordsList.Count; f++)
		{
			outputField.text += wordsList[f] + " ";
		}
	}
}

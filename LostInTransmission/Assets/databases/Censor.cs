using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Censor : MonoBehaviour
{
	public DictionaryTester dict;

	public int maxChars = 30;
	public float chanceOfRemoval = 0.5f;
	public float chanceOfSwap = 0.5f;
	public float chanceOfSynonym = 0.3f;
	public float chanceOfComrade = 0.01f;

	public class Message
	{
		public string beforeCensor;
		public string afterCensor;
	}
	public List<Message> messageHistory = new List<Message>();

	private string censorText = "[CENSORED]";
	private string comradeText = "comrade";

	public string CleanMessage(string message, List<string> excludeWords)
	{
		Message messageInHistory = new Message();
		messageInHistory.beforeCensor = message;

		List<string> wordsList = new List<string>(message.Split(' '));
		for (int i = wordsList.Count - 1; i >= 0; i--)
		{
			wordsList[i] = wordsList[i].ToLower();
			wordsList[i] = wordsList[i].Trim();
			// Remove punctuation.
			wordsList[i] = new string(wordsList[i].Where(c => !char.IsPunctuation(c)).ToArray());
			if (wordsList[i].Length == 0)
			{
				wordsList.RemoveAt(i);
			}
		}

		// Verify length of message.
		/*int numberToRemove = wordsList.Count - maxWords;
		while (numberToRemove > 0)
		{
			int trashIndex = Random.Range(0, wordsList.Count);
			while (wordsList[trashIndex] == censorText)
			{
				trashIndex = Random.Range(0, wordsList.Count);
			}
			Debug.Log("Too many, trashing " + wordsList[trashIndex]);
			wordsList[trashIndex] = censorText;
			numberToRemove--;
		}*/

		// Verify each word is a real word.
		for (int w = wordsList.Count - 1; w >= 0; w--)
		{
			string word = wordsList[w];
			if (word == censorText)
			{
				continue;
			}

			if (!dict.GetIsWordValid(word))
			{
				// Try again but remove the trailing s if it exists.
				if (word[word.Length - 1] == 's')
				{
					word = word.Substring(0, word.Length - 1);
					if (dict.GetIsWordValid(word))
					{
						wordsList[w] = word;
						continue;
					}
				}

				Debug.Log("Invalid word " + wordsList[w]);
				wordsList[w] = censorText;
			}
		}

		// Remove duplicate words.
		for (int w = 0; w < wordsList.Count; w++)
		{
			string wordA = wordsList[w];
			if (wordA == censorText)
			{
				continue;
			}

			for (int d = 0; d < wordsList.Count; d++)
			{
				if (w != d)
				{
					string wordB = wordsList[d];
					if (wordA == wordB)
					{
						Debug.Log("Duplicate " + wordA);
						wordsList[w] = censorText;
					}
				}
			}
		}

		// Remove words that are too small.
		for (int w = 0; w < wordsList.Count; w++)
		{
			string word = wordsList[w];
			if (word.Length < 3)
			{
				Debug.Log("Too small " + word);
				wordsList[w] = censorText;
			}
		}

		// Remove words matching the current items or their synonyms.
		for (int w = 0; w < wordsList.Count; w++)
		{
			string word = wordsList[w];
			for (int e = 0; e < excludeWords.Count; e++)
			{
				string excludeWord = excludeWords[e];
				bool matches = FindWordMatchesWord(word, excludeWord);
				bool matchesReverse = FindWordMatchesWord(excludeWord, word);
				if (matches || matchesReverse)
				{
					Debug.Log("Match for " + word);
					wordsList[w] = censorText;
					break;
				}
			}
		}

		// Small chance to replace each word with Comrade.
		for (int w = 0; w < wordsList.Count; w++)
		{
			if (wordsList[w] == censorText)
			{
				continue;
			}

			if (Random.value <= chanceOfComrade)
			{
				Debug.Log("Comrade " + wordsList[w]);
				wordsList[w] = comradeText;
			}
		}

		// Remove a word at random sometimes.
		if (wordsList.Count > 0 && Random.value <= chanceOfRemoval)
		{
			int removeIndex = Random.Range(0, wordsList.Count);
			Debug.Log("Chance: Removing " + wordsList[removeIndex]);
			wordsList[removeIndex] = censorText;
		}

		// Swap a word at random sometimes.
		if (wordsList.Count > 0 && Random.value <= chanceOfSwap)
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

		// Replace words with a synonym sometimes.
		if (wordsList.Count > 0)
		{
			for (int w = 0; w < wordsList.Count; w++)
			{
				if (wordsList[w] == censorText || wordsList[w] == comradeText)
				{
					continue;
				}

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
		}

		string cleanMessage = "";
		for (int f = 0; f < wordsList.Count; f++)
		{
			cleanMessage += wordsList[f] + " ";
		}

		messageInHistory.afterCensor = cleanMessage;
		messageHistory.Add(messageInHistory);

		return cleanMessage;
	}

	private bool FindWordMatchesWord(string word1, string word2)
	{
		List<string> synonyms = dict.GetWordSynonyms(word2);
		if (synonyms != null)
		{
			synonyms.Add(word2);
	 		return synonyms.Contains(word1);
		}

		return false;
	}
}

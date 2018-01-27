using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WritePanel : MonoBehaviour
{
	public DictionaryTester dict;
	public ItemDB itemDB;
	public PlayerDB playerDB;
	public GameObject passToPanel;
	public List<InputField> inputFields;
	public List<GameObject> goodItems;
	public List<GameObject> returnedGoodItems;
	public List<GameObject> badItems;
	public List<GameObject> trashItems;
	public Button submit;

	public int maxWords = 5;

	public float chanceOfRemoval = 0.5f;
	public float chanceOfSwap = 0.5f;
	public float chanceOfSynonym = 0.3f;

	void Start()
	{
		submit.onClick.AddListener(TaskOnClick);

		for (int i = 0; i < inputFields.Count; i++)
		{
			inputFields[i].onValueChanged.AddListener(InputChanged);
		}
	}

	void InputChanged(string input)
	{
		// Check if we should enable the submit button.
		int numWithText = 0;
		for (int i = 0; i < inputFields.Count; i++)
		{
			if (inputFields[i].text.Length > 0)
			{
				numWithText++;
			}
		}

		if (numWithText == inputFields.Count)
		{
			submit.gameObject.SetActive(true);
		}
	}

	void OnEnable()
	{
		itemDB.GenerateGameItems(6, 2, 2);

		DisplayGoodItems();
		DisplayBadItems();

		for (int t = 0; t < trashItems.Count; t++)
		{
			trashItems[t].SetActive(false);
		}

		submit.gameObject.SetActive(false);

		for (int i = 0; i < inputFields.Count; i++)
		{
			inputFields[i].text = "";
		}

		UnpackBox();
	}

	void DisplayGoodItems()
	{
		for (int g = 0; g < goodItems.Count; g++)
		{
			GameObject goodItemGO = goodItems[g];
			goodItemGO.SetActive(true);
			Text goodItem = goodItemGO.GetComponentsInChildren<Text>()[0];
			goodItem.text = itemDB.GetGoodItem(g).name;
		}
	}

	void DisplayBadItems()
	{
		for (int b = 0; b < badItems.Count; b++)
		{
			GameObject badItemGO = badItems[b];
			badItemGO.SetActive(true);
			Text badItem = badItemGO.GetComponentsInChildren<Text>()[0];
			badItem.text = itemDB.GetBadItem(b).name;
		}
	}

	void UnpackBox()
	{
		List<ItemDB.Item> boxItems = itemDB.GetItemsInBoxPreviously();
		for (int i = 0; i < boxItems.Count; i++)
		{
			ItemDB.Item boxItem = boxItems[i];
			int goodIndex = CheckItemInList(boxItem, goodItems);
			int badIndex = CheckItemInList(boxItem, badItems);
			if (goodIndex != -1)
			{
				goodItems[goodIndex].SetActive(false);
				returnedGoodItems[goodIndex].GetComponentsInChildren<Text>()[0].text = boxItem.name;
			}
			else if (badIndex != -1)
			{
				badItems[badIndex].GetComponent<Image>().color = Color.red;
			}
			else
			{
				for (int t = 0; t < trashItems.Count; t++)
				{
					GameObject trashItem = trashItems[t];
					if (!trashItem.activeSelf)
					{
						trashItem.SetActive(true);
						trashItem.GetComponentsInChildren<Text>()[0].text = boxItem.name;
						break;
					}
				}
			}
		}
	}

	int CheckItemInList(ItemDB.Item checkItem, List<GameObject> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].GetComponentsInChildren<Text>()[0].text == checkItem.name)
			{
				return i;
			}
		}

		return -1;
	}

	void TaskOnClick()
	{
		List<string> wordsList = new List<string>();
		for (int i = 0; i < inputFields.Count; i++)
		{
			wordsList.Add(inputFields[i].text.Trim());
		}

		while (wordsList.Count > maxWords)
		{
			int trashIndex = Random.Range(0, wordsList.Count);
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

		if (wordsList.Count > 0 && Random.value <= chanceOfRemoval)
		{
			int removeIndex = Random.Range(0, wordsList.Count);
			Debug.Log("Chance: Removing " + wordsList[removeIndex]);
			wordsList.RemoveAt(removeIndex);
		}

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

		if (wordsList.Count > 0)
		{
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
		}

		string message = "";
		for (int f = 0; f < wordsList.Count; f++)
		{
			message += wordsList[f] + " ";
		}
		playerDB.SetMessage(message);

		gameObject.SetActive(false);
		passToPanel.SetActive(true);
	}
}

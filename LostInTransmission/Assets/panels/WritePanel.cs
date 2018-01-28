using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WritePanel : MonoBehaviour
{
	public Censor censor;
	public DictionaryTester dict;
	public ItemDB itemDB;
	public PlayerDB playerDB;
	public GameObject passToPanel;
	public InputField inputField;
	public List<GameObject> goodItems;
	public List<GameObject> returnedGoodItems;
	public List<GameObject> badItems;
	public List<GameObject> trashItems;
	public Button submit;
	public Text wordLimitCurrText;
	public Text wordLimitMax;

	void Start()
	{
		submit.onClick.AddListener(TaskOnClick);
		inputField.onValueChanged.AddListener(InputChanged);
	}

	void InputChanged(string input)
	{
		// Check if we should enable the submit button.
		int numOfWords = 0;
		string[] words = inputField.text.Split(' ');
		for (int i = 0; i < words.Length; i++)
		{
			string word = words[i];
			word = word.Trim();
			if (word.Length > 2)
			{
				numOfWords++;
			}
		}

		wordLimitCurrText.text = numOfWords.ToString();
		wordLimitMax.text = "/" + censor.maxWords;
		wordLimitCurrText.color = Color.black;

		if (numOfWords >= censor.maxWords)
		{
			if (numOfWords > censor.maxWords)
			{
				wordLimitCurrText.color = Color.red;
			}
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

		for (int r = 0; r < returnedGoodItems.Count; r++)
		{
			returnedGoodItems[r].SetActive(false);
		}

		submit.gameObject.SetActive(false);

		inputField.text = "";
		inputField.Select();
 		inputField.ActivateInputField();

		wordLimitCurrText.color = Color.black;
		wordLimitCurrText.text = "0";
		wordLimitMax.text = "/" + censor.maxWords;

		UnpackBox();
	}

	void DisplayGoodItems()
	{
		for (int g = 0; g < goodItems.Count; g++)
		{
			GameObject goodItemGO = goodItems[g];
			goodItemGO.SetActive(true);
			Image goodItem = goodItemGO.GetComponentsInChildren<Image>()[0];
			goodItem.sprite = itemDB.GetGoodItem(g).sprite;
		}
	}

	void DisplayBadItems()
	{
		for (int b = 0; b < badItems.Count; b++)
		{
			GameObject badItemGO = badItems[b];
			badItemGO.SetActive(true);
			Image badItem = badItemGO.GetComponentsInChildren<Image>()[0];
			badItem.sprite = itemDB.GetBadItem(b).sprite;
		}
	}

	void UnpackBox()
	{
		List<ItemDB.Item> boxItems = itemDB.GetItemsInBoxPreviously();
		for (int i = 0; i < boxItems.Count; i++)
		{
			ItemDB.Item boxItem = boxItems[i];
			int goodIndex = CheckItemInList(boxItem, itemDB.GetGoodItems());
			int badIndex = CheckItemInList(boxItem, itemDB.GetBadItems());
			if (goodIndex != -1)
			{
				goodItems[goodIndex].SetActive(false);
				returnedGoodItems[goodIndex].SetActive(true);
				returnedGoodItems[goodIndex].GetComponentsInChildren<Image>()[0].sprite = boxItem.sprite;
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
						trashItem.GetComponentsInChildren<Image>()[0].sprite = boxItem.sprite;
						break;
					}
				}
			}
		}
	}

	int CheckItemInList(ItemDB.Item checkItem, List<ItemDB.Item> items)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].name == checkItem.name)
			{
				return i;
			}
		}

		return -1;
	}

	void TaskOnClick()
	{
		List<ItemDB.Item> items = itemDB.GetGameItems();
		List<string> itemStrs = new List<string>();
		foreach (ItemDB.Item item in items)
		{
			itemStrs.Add(item.name);
		}

		string cleaned = censor.CleanMessage(inputField.text, itemStrs);
		playerDB.SetMessage(cleaned);

		gameObject.SetActive(false);
		passToPanel.SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	private bool gameOver = false;
	private bool playersWon = false;

	void Start()
	{
		submit.onClick.AddListener(TaskOnClick);
		inputField.onValueChanged.AddListener(InputChanged);
	}

	void InputChanged(string input)
	{
		// Check if we should enable the submit button.
		int numOfChars = inputField.text.Length;

		wordLimitCurrText.text = numOfChars.ToString();
		wordLimitMax.text = "/" + censor.maxChars;
		wordLimitCurrText.color = (numOfChars > censor.maxChars) ? Color.red : Color.black;

		bool showButton = numOfChars > 0 && numOfChars <= censor.maxChars;
		submit.gameObject.SetActive(showButton || gameOver);
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
		wordLimitMax.text = "/" + censor.maxChars;

		UnpackBox();

		CheckGameOver();
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
		int numGoodInBox = 0;
		for (int i = 0; i < boxItems.Count; i++)
		{
			ItemDB.Item boxItem = boxItems[i];
			int goodIndex = CheckItemInList(boxItem, itemDB.GetGoodItems());
			int badIndex = CheckItemInList(boxItem, itemDB.GetBadItems());
			if (goodIndex != -1)
			{
				numGoodInBox++;
				goodItems[goodIndex].SetActive(false);
				returnedGoodItems[goodIndex].SetActive(true);
				returnedGoodItems[goodIndex].GetComponentsInChildren<Image>()[0].sprite = boxItem.sprite;
			}
			else if (badIndex != -1)
			{
				badItems[badIndex].GetComponent<Image>().color = Color.red;
				gameOver = true;
				playersWon = false;
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

		if (!gameOver && numGoodInBox == 6)
		{
			gameOver = true;
			playersWon = true;
		}
	}

	void CheckGameOver()
	{
		if (gameOver)
		{
			submit.transform.Find("Text").GetComponent<Text>().text = "Restart";
			submit.gameObject.SetActive(true);
			Text inputText = inputField.transform.Find("Text").GetComponent<Text>();
			inputText.fontSize = 20;
			inputText.resizeTextForBestFit = false;
			inputField.text = playersWon ? "Success!" : "Illegal Package!";

			for (int h = 0; h < censor.messageHistory.Count; h++)
			{
				Censor.Message currMessage = censor.messageHistory[h];
				inputField.text += "\n\nMessage " + (h + 1);
				inputField.text += "\n---Before---\n" + currMessage.beforeCensor;
				inputField.text += "\n---After---\n" + currMessage.afterCensor;
			}

			wordLimitCurrText.gameObject.SetActive(false);
			wordLimitMax.gameObject.SetActive(false);
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
		if (gameOver)
		{
			SceneManager.LoadScene("brian");
			return;
		}

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

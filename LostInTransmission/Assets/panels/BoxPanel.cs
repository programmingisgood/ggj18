using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxPanel : MonoBehaviour
{
	public GameObject passBackPanel;
	public ItemDB itemDB;
	public PlayerDB playerDB;
	public List<GameObject> items;
	public Text messageDisplay;
	public Button sendButton;
	public Button boxButton;
	public Image boxImage;
	public Text boxText;

	private bool boxOpen = true;
	private Dictionary<GameObject, ItemDB.Item> itemMap;

	class ButtonPressData
	{
		public ButtonPressData(int setI)
		{
			index = setI;
		}

		public int index;
	}

	void Start()
	{
		sendButton.onClick.AddListener(OnSendIt);
		boxButton.onClick.AddListener(OnBox);

		for (int g = 0; g < items.Count; g++)
		{
			GameObject itemGO = items[g];
			Button itemButton = itemGO.GetComponent<Button>();
			ButtonPressData data = new ButtonPressData(g);
			itemButton.onClick.AddListener(delegate { OnItemClicked(data); });
		}
	}

	void OnEnable()
	{
		boxOpen = true;
		boxText.text = "Box Open";

		itemMap = new Dictionary<GameObject, ItemDB.Item>();
		DisplayItems();

		messageDisplay.text = playerDB.GetMessage();

		sendButton.gameObject.SetActive(false);

		itemDB.EmptyBox();
	}

	void DisplayItems()
	{
		List<ItemDB.Item> allGameItems = itemDB.GetGameItems();
		ItemDB.Shuffle(allGameItems);

		for (int g = 0; g < items.Count; g++)
		{
			GameObject itemGO = items[g];
			itemGO.SetActive(true);
			Text item = itemGO.GetComponentsInChildren<Text>()[0];
			int randomIndex = Random.Range(0, allGameItems.Count);
			item.text = allGameItems[randomIndex].name;
			itemMap[itemGO] = allGameItems[randomIndex];
			allGameItems.RemoveAt(randomIndex);
		}

		for (int g = 0; g < items.Count; g++)
		{
			GameObject itemGO = items[g];
			Text item = itemGO.GetComponentsInChildren<Text>()[0];
			if (itemDB.CheckItemWasPacked(item.text))
			{
				itemGO.SetActive(false);
			}
		}
	}

	void OnItemClicked(ButtonPressData data)
	{
		GameObject itemGO = items[data.index];
		itemGO.SetActive(false);
		ItemDB.Item itemInDB = itemMap[itemGO];
		itemDB.AddItemToBox(itemInDB);
	}

	void OnSendIt()
	{
		gameObject.SetActive(false);
		passBackPanel.SetActive(true);
	}

	void OnBox()
	{
		boxOpen = !boxOpen;
		if (boxOpen)
		{
			boxText.text = "Box Open";
			sendButton.gameObject.SetActive(false);
		}
		else
		{
			boxText.text = "Box Closed";
			sendButton.gameObject.SetActive(true);
		}
	}
}

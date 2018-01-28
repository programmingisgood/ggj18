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
	public Image boxImage;

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
			Image item = itemGO.GetComponentsInChildren<Image>()[0];
			int randomIndex = Random.Range(0, allGameItems.Count);
			item.sprite = allGameItems[randomIndex].sprite;
			itemMap[itemGO] = allGameItems[randomIndex];
			allGameItems.RemoveAt(randomIndex);
		}

		for (int g = 0; g < items.Count; g++)
		{
			GameObject itemGO = items[g];
			Image itemImage = itemGO.GetComponentsInChildren<Image>()[0];
			if (itemDB.CheckItemWasPacked(itemImage.sprite))
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

		sendButton.gameObject.SetActive(true);
	}

	void OnSendIt()
	{
		gameObject.SetActive(false);
		passBackPanel.SetActive(true);
	}
}

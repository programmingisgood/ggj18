using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
	[System.Serializable]
	public class Item
	{
		public string name;
		public Sprite sprite;
	}

	public List<Item> allItems;

	private List<Item> goodItems;
	private List<Item> neutralItems;
	private List<Item> badItems;

	private List<Item> itemsInBox;
	private List<Item> itemsInBoxPreviously;

	private bool generated = false;

	public void GenerateGameItems(int numGood, int numNeutral, int numBad)
	{
		if (generated)
		{
			return;
		}

		List<Item> itemsCopy = new List<Item>(allItems);

		goodItems = new List<Item>();
		PopulateWithRandom(itemsCopy, goodItems, numGood);

		neutralItems = new List<Item>();
		PopulateWithRandom(itemsCopy, neutralItems, numNeutral);

		badItems = new List<Item>();
		PopulateWithRandom(itemsCopy, badItems, numBad);

		EmptyBox();
		itemsInBoxPreviously = new List<Item>();

		generated = true;
	}

	void PopulateWithRandom(List<Item> fromList, List<Item> toList, int number)
	{
		for (int i = 0; i < number; i++)
		{
			int randomIndex = Random.Range(0, fromList.Count);
			toList.Add(fromList[randomIndex]);
			fromList.RemoveAt(randomIndex);
		}
	}

	public List<Item> GetGoodItems()
	{
		return goodItems;
	}

	public Item GetGoodItem(int index)
	{
		if (!generated)
		{
			GenerateGameItems(6, 2, 2);
		}

		return goodItems[index];
	}

	public List<Item> GetBadItems()
	{
		return badItems;
	}

	public Item GetBadItem(int index)
	{
		if (!generated)
		{
			GenerateGameItems(6, 2, 2);
		}

		return badItems[index];
	}

	public List<Item> GetGameItems()
	{
		if (!generated)
		{
			GenerateGameItems(6, 2, 2);
		}

		List<Item> allItems = new List<Item>();
		foreach (Item item in goodItems)
		{
			allItems.Add(item);
		}
		foreach (Item item in neutralItems)
		{
			allItems.Add(item);
		}
		foreach (Item item in badItems)
		{
			allItems.Add(item);
		}

		return allItems;
	}

	public void EmptyBox()
	{
		itemsInBox = new List<Item>();
	}

	public void AddItemToBox(Item newItem)
	{
		if (itemsInBox == null)
		{
			EmptyBox();
		}

		itemsInBox.Add(newItem);
		itemsInBoxPreviously.Add(newItem);
	}

	public List<Item> GetItemsInBox()
	{
		if (itemsInBox == null)
		{
			EmptyBox();
		}

		return itemsInBox;
	}

	public List<Item> GetItemsInBoxPreviously()
	{
		return itemsInBoxPreviously;
	}

	public bool CheckItemWasPacked(string checkItem)
	{
		for (int i = 0; i < itemsInBoxPreviously.Count; i++)
		{
			if (itemsInBoxPreviously[i].name == checkItem)
			{
				return true;
			}
		}

		return false;
	}

	public bool CheckItemWasPacked(Sprite checkItem)
	{
		for (int i = 0; i < itemsInBoxPreviously.Count; i++)
		{
			if (itemsInBoxPreviously[i].sprite == checkItem)
			{
				return true;
			}
		}

		return false;
	}

	public static void Shuffle(List<Item> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Item temp = list[i];
			int randomIndex = Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
	}
}

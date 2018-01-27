using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//appendage|3
//(noun)|extremity|member|external body part
//(noun)|process|outgrowth|body part
//(noun)|part|portion

public class DictionaryTester : MonoBehaviour
{
	private Dictionary<string, List<string>> words = new Dictionary<string, List<string>>();
	private StreamReader streamReader;
	private bool loaded = false;

	void Start()
	{
		string filePath = Path.Combine(Application.dataPath, "th_en_US_new.dat");
		FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		BufferedStream bs = new BufferedStream(fileStream);
		streamReader = new StreamReader(bs);
	}

	void Update()
	{
		if (!loaded)
		{
			for (int i = 0; i < 100; i++)
			{
				string line;
				if ((line = streamReader.ReadLine()) != null)
				{
					string[] wordData = line.Split('|');
					string wordStr = wordData[0];
					int numSyms = int.Parse(wordData[1]);
					List<string> synonyms = new List<string>();
					for (int s = 0; s < numSyms; s++)
					{
						line = streamReader.ReadLine();
						string[] synonymData = line.Split('|');
						for (int sd = 1; sd < synonymData.Length; sd++)
						{
							synonyms.Add(synonymData[sd]);
						}
					}
					words[wordStr] = synonyms;
				}
				else
				{
					Debug.Log("Loaded " + words.Count + " words");
					loaded = true;
					break;
				}
			}
		}
	}

	public bool GetIsWordValid(string word)
	{
		return words.ContainsKey(word);
	}

	public List<string> GetWordSynonyms(string word)
	{
		if (GetIsWordValid(word))
		{
			return words[word];
		}

		return null;
	}
}

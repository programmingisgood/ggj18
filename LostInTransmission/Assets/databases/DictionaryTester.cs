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
	public TextAsset dictionaryAsset;



	private Dictionary<string, List<string>> words = new Dictionary<string, List<string>>();
	private StreamReader streamReader;
	private bool loaded = false;

	private int bytesRead = 0;
	private int linesRead = 0;
	private int totalBytes;

	void Start()
	{
		/*string filePath = Path.Combine(Application.dataPath, "th_en_US_new.dat");
		FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
		BufferedStream bs = new BufferedStream(fileStream);
		streamReader = new StreamReader(bs);*/

		//FileInfo fileInfo = new FileInfo(filePath);
		//FileInfo fileInfo = new FileInfo(dictionaryAsset.text);

   		//totalBytes = (int) fileInfo.Length;
		//streamReader = theSourceFile.OpenText();

		totalBytes = dictionaryAsset.text.Length;
	}

	void Update()
	{
		if (!loaded)
		{
			for (int i = 0; i < 2000; i++)
			{
				string line;
				//if ((line = streamReader.ReadLine()) != null)
				string[] lines = dictionaryAsset.text.Split('\n');
				Debug.Log(lines[0]);
				int lineCount = lines.Length;
				int currentLine = 0;
				while(currentLine < lineCount)
				{

				
					
					line = lines[currentLine];
					linesRead++;
					// + 1 here due to the end line character.
					bytesRead += line.Length + 1;

					string[] wordData = line.Split('|');
					string wordStr = wordData[0];
					//Debug.Log(wordStr.Length);
					//Debug.Log(wordData.Length);
					int numSyms = int.Parse(wordData[1]);
					List<string> synonyms = new List<string>();
					for (int s = 0; s < numSyms; s++)
					{
						line = lines[currentLine];
						currentLine++;
						linesRead++;
						// + 1 here due to the end line character.
						bytesRead += line.Length + 1;

						string[] synonymData = line.Split('|');
						for (int sd = 1; sd < synonymData.Length; sd++)
						{
							synonyms.Add(synonymData[sd]);
						}
					}
					words[wordStr] = synonyms;
					currentLine++;
				}
				/*else
				{
					Debug.Log("Loaded " + words.Count + " words");
					loaded = true;
					Debug.Log("Read " + bytesRead + " / " + totalBytes);
					Debug.Log("Lines " + linesRead);
					break;
				}
				*/
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

	public float GetLoadingProgress()
	{
		if (totalBytes > 0)
		{
			return (float) bytesRead / (float) totalBytes;
		}
		return 0;
	}
}

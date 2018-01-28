using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
	public DictionaryTester dict;
	public Censor censor;
	public InputField input;
	public InputField itemsInput;
	public Text output;
	public Text loading;

	void Start()
	{
		input.onValueChanged.AddListener(InputChanged);
	}

	void Update()
	{
		loading.text = (int) (dict.GetLoadingProgress() * 100f) + "%";
	}

	void InputChanged(string change)
	{
		List<string> itemWords = new List<string>(itemsInput.text.Split(' '));
		string cleaned = censor.CleanMessage(change, itemWords);
		output.text = cleaned;
	}
}

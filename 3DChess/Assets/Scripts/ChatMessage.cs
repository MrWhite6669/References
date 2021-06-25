using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour {

    string text;
    string sender;

    Text textUI;
    Text nameUI;

    void Awake()
    {
        textUI = transform.GetChild(1).GetComponent<Text>();
        nameUI = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        nameUI.text = sender + ": ";
        textUI.text = text;
    }

    public void Popup(string name,string text)
    {
        this.text = text;
        this.sender = name;
        if (text.Length > 21) GetComponent<RectTransform>().sizeDelta += new Vector2(0, text.Length / 2);
    }

    public void ChangeColor(Color color)
    {
        nameUI.color = color;
    }

    public void MakeStatic()
    {

    }
	
}

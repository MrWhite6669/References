using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCPWhite;

public class Chat : MonoBehaviour {

    #region Singleton

    public static Chat instance;

    void Awake() { instance = this; }

    #endregion

    public bool enabledChat;
    public GameObject content;
    public InputField input;

    public GameObject messagePrefab;
    public ScrollRect scrollRect;

    public void Update()
    {
        if (enabledChat && Input.GetKeyUp(KeyCode.Return))
        {
            SendMessage();
        }
    }

    void Start()
    {
        ScrollToBottom();
    }

    void ScrollToBottom()
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    public void ShowMessage(string text , string name)
    {
        GameObject msg = Instantiate(messagePrefab, content.transform);
        if (name == Manager.instance.playerName) { msg.GetComponent<ChatMessage>().ChangeColor(Color.red); name = "You"; }
        msg.GetComponent<ChatMessage>().Popup(name, text);
        content.GetComponent<RectTransform>().sizeDelta += new Vector2(0, (text.Length / 2) + 28);
        ScrollToBottom();
    }

    public void SendMessage()
    {
        Message msg = new Message(ID.ChatMessage);
        msg.data.Add("text", input.text);
        ServerListener.instance.SendMessageToServer(msg);
        input.text = "";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCPWhite;

public class Authentication : MonoBehaviour {

    public InputField nameField, passwordField; // Text Fields from UI references

    public void Login()
    {
        Manager.instance.ShowLoadingPanel();
        ServerListener.instance.Connect(); // Connect to server
        Message msg = new Message(ID.LoginRequest); // || Send message with credentials to server
        msg.data.Add("name", nameField.text); // ---------------------------------
        msg.data.Add("password", passwordField.text); // -------------------------
        ServerListener.instance.SendMessageToServer(msg); // -------------------||
    }

    public void Logout()
    {
        Manager.instance.ShowLoadingPanel(); 
        ServerListener.instance.SendMessageToServer(new Message(ID.LogoutRequest));
    }

    public void Register()
    {
        Manager.instance.ShowLoadingPanel();
        ServerListener.instance.Connect(); // Connect to server
        Message msg = new Message(ID.RegisterRequest); // || Send message with credentials to server
        msg.data.Add("name", nameField.text); //------------------------
        msg.data.Add("password", passwordField.text); //----------------
        ServerListener.instance.SendMessageToServer(msg);//-----------||
    }

    public void DebugLogin(int code)
    {
        switch (code)
        {
            default: break;
            case 1:
                {
                    Manager.instance.ShowLoadingPanel();
                    ServerListener.instance.Connect(); // Connect to server
                    Message msg = new Message(ID.LoginRequest); // || Send message with credentials to server
                    msg.data.Add("name", "root"); // ---------------------------------
                    msg.data.Add("password", "r1o2o3t"); // -------------------------
                    ServerListener.instance.SendMessageToServer(msg); // -------------------||
                }
                break;
            case 2:
                {
                    Manager.instance.ShowLoadingPanel();
                    ServerListener.instance.Connect(); // Connect to server
                    Message msg = new Message(ID.LoginRequest); // || Send message with credentials to server
                    msg.data.Add("name", "MrWhite"); // ---------------------------------
                    msg.data.Add("password", "pizza"); // -------------------------
                    ServerListener.instance.SendMessageToServer(msg); // -------------------||
                }
                break;
        }
    }

}

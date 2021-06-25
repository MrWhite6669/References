using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCPWhite;

public class ServerListener : MonoBehaviour {

    #region Singleton
    public static ServerListener instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public TCPClient client;
    public string ipAddress = "127.0.0.1";
    public int port = 6666;
    public bool debug = false;

    public void Connect() // Connect to server
    {
        if(client == null)
        {
            client = new TCPClient(ipAddress, port);
            if (debug) // If debug mode is on automaticly login as root user
            {
                Message msg = new Message(ID.LoginRequest);
                msg.data.Add("name", "root");
                msg.data.Add("password", "r1o2o3t");
                SendMessageToServer(msg);
            }
        }
    }

    void Update()
    {
        if(client!=null && client.IsConnected())ListenToMessages();
    }

    public void SendMessageToServer(Message msg)
    {
        client.SendMessage(msg);
    }

    void ListenToMessages() // Listen to messages from the server and sort them by its ID
    {
        Message msg;
        try { msg = client.GetMessageFromQueue(); }
        catch { msg = null; }
        if (msg != null)
        {
            switch (msg.id)
            {
                default: { Debug.Log(string.Format("{0}||{1}", msg.id, GetMessageData(msg))); }
                    break;
                case ID.LoginAnswer:
                    {
                        Manager.instance.HideLoadingPanel();
                        if (msg.data["status"] == "ok")
                        {
                            Manager.instance.ShowMessagePanel("Succesfully loginned in!", Color.green, 3f);
                            Manager.instance.LoadMenu();
                        }
                        else
                        {
                            Manager.instance.ShowMessagePanel(msg.data["reason"], Color.red, 3f);
                        }
                    }
                    break;
                case ID.RegisterAnswer:
                    {
                        Manager.instance.HideLoadingPanel();
                        if (msg.data["status"] == "ok")
                        {
                            Manager.instance.ShowMessagePanel("Succesfully registered , now you can login!", Color.green, 3f);
                        }
                        else
                        {
                            Manager.instance.ShowMessagePanel(msg.data["reason"], Color.red, 3f);
                        }
                    }
                    break;
                case ID.InfoAnswer:
                    {
                        Manager.instance.UpdateInfo(msg.data["name"], int.Parse(msg.data["wins"]), int.Parse(msg.data["losts"]));
                    }
                    break;
                case ID.LogoutAnswer:
                    {
                        Manager.instance.HideLoadingPanel();
                        Manager.instance.LoadLoginMenu();
                        client.Close();
                        client = null;
                    }
                    break;
                case ID.MatchAnswer:
                    {
                        Manager.instance.HideLoadingPanel();
                        Manager.instance.ShowMessagePanel("Match found!", Color.green, 3f);
                        if (msg.data["black"] == Manager.instance.playerName)
                        {
                            Manager.instance.playerColor = Manager.PlayerColor.Black;
                            Manager.instance.opponentText.text = msg.data["white"];
                        }
                        else
                        {
                            Manager.instance.opponentText.text = msg.data["black"];
                            Manager.instance.playerColor = Manager.PlayerColor.White;
                        }
                        Manager.instance.Play();
                    }
                    break;
                case ID.OpponentDisconnected:
                    {
                        Manager.instance.ShowMessagePanel("Opponent disconnected!", Color.red, 2f);
                        ChessManager.instance.EndMatch();
                    }
                    break;
                case ID.Move:
                    {
                        if (msg.data["status"] == "ok")
                        {
                            string[] pos = msg.data["position"].Split(':');
                            ChessManager.instance.Move(ChessManager.instance.FindNodeByCordinates(pos[0]), ChessManager.instance.FindNodeByCordinates(pos[1]));
                        }
                        else
                        {
                            Manager.instance.ShowMessagePanel("It's not your turn!", Color.red, 1);
                        }
                    }
                    break;
                case ID.SwitchTurn:
                    {
                        Manager.instance.SwitchTurnPanel();
                        ChessManager.instance.isYourTurn = !ChessManager.instance.isYourTurn;
                    }
                    break;
                case ID.LeaveMatch:
                    {
                        Manager.instance.HideLoadingPanel();
                        Manager.instance.ShowMessagePanel("Match left", Color.red, 2);
                        ChessManager.instance.EndMatch();
                    }
                    break;
                case ID.ChatMessage:
                    {
                        Chat.instance.ShowMessage(msg.data["text"], msg.data["sender"]);
                    }
                    break;
                case ID.Check:
                    {
                        ChessManager.instance.Check();
                        Manager.instance.ShowMessagePanel("Check!", Color.cyan, 3);
                        CheckPhase.instance.Initiate(msg.data["attacker"]);
                        ChessManager.instance.onFigureMoved.Invoke();
                    }
                    break;
                case ID.EndMatch:
                    {
                        if(msg.data["status"] == "win")
                        {
                            Manager.instance.ShowMessagePanel("You won , congratulatons!!", Color.green, 3);
                        }
                        else
                        {
                            Manager.instance.ShowMessagePanel("Maybe next time...", Color.red, 3);
                        }

                        ChessManager.instance.EndMatch();
                        SendMessageToServer(new Message(ID.InfoRequest));
                    }
                    break;
                case ID.ThreatenNode:
                    {
                        ChessManager.instance.ThreatenNode(ChessManager.instance.FindNodeByCordinates(msg.data["node"]));
                    }
                    break;
            }
        }
    }

    public bool IsConnected()
    {
        if (client != null && client.IsConnected()) return true;
        return false;
    }

    private string GetMessageData(Message message) // Gets all data from message [DEBUG ONLY]
    {
        string output = "--------------------------------------------";
        if (message.data != null)
        {
            foreach (string key in message.data.Keys)
            {
                output += string.Format("\n{0}|{1}", key, message.data[key]);
            }
        }
        output += "\n---------------------------------------";
        return output;
    }

}

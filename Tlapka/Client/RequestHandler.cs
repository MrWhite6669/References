using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestHandler : MonoBehaviour
{

    #region Singleton
    public static RequestHandler Instance { get; set; }
    public void Awake()
    {
        Instance = this;
    }
    #endregion
    public void Handle(int ID, string text)
    {
        Debug.Log("Reply from '"+ID.ToString()+"': " + text);
        switch (ID)
        {
            case RequestID.GetPoints:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    Points.instance.AddPoints(json);
                    Database.instance.connected = true;
                }
                break;
            case RequestID.GetLocations:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    DataManager.Instance.GetLocations(json);
                    DataManager.Instance.GetNearestLocation();
                }
                break;
            case RequestID.AddLocation:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    if (json["status"].ToString() == "Ok")
                    {
                        DebugModeManager.Instance.ReloadSlots();
                    }
                }
                break;
            case RequestID.EditLocation:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    if (json["status"].ToString() == "Ok")
                    {
                        DebugModeManager.Instance.ReloadSlots();
                    }
                }
                break;
            case RequestID.GetUsers:
                {
                    if (text == "No users for this location found.")
                    {
                        DebugModeManager.Instance.nouserssign.text = text;
                    }
                    else
                    {
                        JsonData json = JsonMapper.ToObject(text);
                        DebugModeManager.Instance.JsonToUsers(json);
                    }
                    DebugModeManager.Instance.SetInfo();
                }
                break;
            case RequestID.Connect:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    InputManager.instance.StopLoading();
                    if (json["status"].ToString() == "Ok")
                    {
                        DataManager.Instance.currentLocation = new Location(int.Parse(json["data"]["index"].ToString()), json["data"]["serialCode"].ToString(), json["data"]["location"].ToString(), int.Parse(json["data"]["maxPoints"].ToString()));
                        Database.instance.GetPoints();
                        Database.instance.RequestInfo();
                        InputManager.instance.ConnectSuccesfull();
                        Database.instance.ChangeUserLoc();
                    }
                    else
                    {
                        InputManager.instance.MessageText(json["status"].ToString());
                    }
                }
                break;
            case RequestID.ChangeLocation:
                {
                    print(text);
                }
                break;
            case RequestID.GetUser:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    if (json["status"].ToString() == "Ok")
                    {
                        DataManager.Instance.user = json["data"];
                        Database.instance.debugText.text = DataManager.Instance.user["userID"].ToString();
                        Debug.Log(json["data"]["userID"]);
                    }
                    else if (json["status"].ToString() == "error1") Database.instance.NewUser();
                    else
                    {
                        InputManager.instance.MessageText(json["status"].ToString());
                    }
                }
                break;
            case RequestID.NewUser:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    if (json["status"].ToString() == "Ok")
                    {
                        PlayerPrefs.SetString("UserID", json["data"].ToString());
                        PlayerPrefs.Save();
                        Database.instance.GetUser();
                    }
                    else
                    {
                        InputManager.instance.MessageText(json["status"].ToString());
                    }
                }
                break;
            case RequestID.Manual:
                {
                    InputManager.instance.manualText.text = text;
                }
                break;
            case RequestID.DeleteLast:
                {
                    AdminInputManager.Instance.DebugMessage(text);
                }
                break;
            case RequestID.SavePoint:
                {
                    Database.instance.connected = true;
                }
                break;
            case RequestID.Version:
                {
                    VersionChecker.Instance.NewestVersion = int.Parse(text);
                }
                break;
            case RequestID.SavePoints:
                {

                }
                break;
            case RequestID.GetLocationIndex:
                {
                    AdminInputManager.Instance.locationIndex = int.Parse(text);
                }
                break;
        }

        InputManager.instance.StopLoading();
    }

    public void Handle(int ID,string text,object[] data)
    {
        Debug.Log(text);
        switch (ID)
        {

            case RequestID.RemoveLocation:
                {
                    JsonData json = JsonMapper.ToObject(text);
                    if (json["status"].ToString() == "Ok")
                    {
                        DebugModeManager.Instance.RemoveSlot((int)data[0]);
                    }

                    InputManager.instance.StopLoading();
                }
                break;
        }

        InputManager.instance.StopLoading();
    }
}

public class RequestID
{
    public const int GetPoints = 001;
    public const int GetLocations = 002;
    public const int AddLocation = 003;
    public const int EditLocation = 004;
    public const int RemoveLocation = 005;
    public const int GetUsers = 006;
    public const int Connect = 007;
    public const int ChangeLocation = 008;
    public const int GetUser = 009;
    public const int NewUser = 010;
    public const int Manual = 011;
    public const int DeleteLast = 012;
    public const int SavePoint = 013;
    public const int Version = 014;
    public const int SavePoints = 015;
    public const int GetLocationIndex = 016;
}

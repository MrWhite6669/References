using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModeManager : MonoBehaviour
{

    public static DebugModeManager Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }

    public bool debugMode = false;
    public string password = "tlapkaadmin1";
    public int holdTime = 100;

    public InputField passwordField,questionField;
    public Toggle toggle;
    public GameObject loginPanel, adminPanel, debugPanel, userInfoPrefab, content,actionsContent,actionPrefab,joinButton,locationsPanel,surePanel;
    public Text info,nouserssign,GPSText;

    public GameObject selectedAction;

    string oldInfo = "Název akce: {0}\nLokace: {1} , {2} , {3}\nKonec akce: {4}\nPočet aktivních uživatelů: {5}";

    int holdTreshold = 0;
    int userCount = 0;

    public int selectedLocation = 0;

    void Update()
    {
        GPSText.text = string.Format("Long: {0} ||Lat:{1} ||Alt:{2}", GPS.instance.longitude, GPS.instance.latitude, GPS.instance.altitude);
        if (Input.GetKey(KeyCode.Escape)) holdTreshold++;
        else holdTreshold = 0;
        if (holdTreshold >= holdTime)
        {
            OpenLocationMode();
            holdTreshold = 0;
        }
    }

    public void DebugButtonDown()
    {
        holdTreshold++;
    }

    public void DebugButtonUp()
    {
        OpenLocationMode();
    }

    public void ResetInfo()
    {
        nouserssign.text = "";
        info.text = oldInfo;
        EmptyUserTable();
    }

    void EmptyUserTable()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i));
        }
    }

    public void AskEdit()
    {
        selectedAction.GetComponent<ActionHandler>().ShowInputs();
    }

    public void AskAdd()
    {
        selectedAction = Instantiate(actionPrefab, actionsContent.transform);
        selectedAction.GetComponent<ActionHandler>().newLoc = true;
        AskEdit();
    }

    public void AddPoint()
    {
        int question = 0;
        if (questionField.text != null && int.TryParse(questionField.text, out question))
        {
            Database.instance.GeneratePoint(question);
        }
    }

    public void ReloadSlots()
    {
        for (int i = 0; i < actionsContent.transform.childCount; i++)
        {
            Destroy(actionsContent.transform.GetChild(i).gameObject);
        }
        Database.instance.RequestLocations();
    }

    public void EditLocation()
    {
        ActionHandler action = selectedAction.GetComponent<ActionHandler>();
        action.HideInputs();
        if (action.name.text == "" || action.maxPoints.text == "")
        {
            if (action.newLoc) Destroy(selectedAction);
            return;
        }
        if (!action.newLoc){
            Database.instance.EditLocation(action.name.text, int.Parse(action.maxPoints.text),action.ActionIndex);
        }
        else
        {
            Database.instance.SaveLocation(action.name.text, int.Parse(action.maxPoints.text));
        }
    }

    public void AskDeletion()
    {
        surePanel.SetActive(true);
    }

    public void AnswerNo()
    {
        surePanel.SetActive(false);
    }

    public void AnswerYes()
    {
        surePanel.SetActive(false);
        Database.instance.RemoveLoc(selectedLocation);
    }

    public void RemoveSlot(int index)
    {
        for (int i = 0; i < actionsContent.transform.childCount; i++)
        {
            if (actionsContent.transform.GetChild(i).GetComponent<ActionHandler>().ActionIndex == index) Destroy(actionsContent.transform.GetChild(i).gameObject);
        }
    }


    public void JsonToUsers(JsonData data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            AddUserInfo(data[i]["userID"].ToString(), data[i]["points"].ToString());
            userCount++;
        }
    }

    public void SetInfo()
    {
        info.text = string.Format(info.text, DataManager.Instance.currentLocation.Name, GPS.instance.latitude, GPS.instance.longitude, GPS.instance.altitude, "error", userCount);
    }
    
    public void Login()
    {
        if(passwordField.text == password)
        {
            locationsPanel.SetActive(true);
            loginPanel.SetActive(false);
        }
    }

    public void OpenAdminMode()
    {
        locationsPanel.SetActive(false);
        debugMode = true;
        debugPanel.SetActive(true);
    }

    public void OpenLocationMode()
    {
        adminPanel.SetActive(true);
        if (IsSaved())
        {
            locationsPanel.SetActive(true);
            debugMode = true;
            loginPanel.SetActive(false);
        }
    }

    public void AddUserInfo(string id,string points)
    {
        GameObject temp = Instantiate(userInfoPrefab,content.transform);
        temp.transform.GetChild(0).GetComponent<Text>().text = id;
        temp.transform.GetChild(1).GetComponent<Text>().text = points;
    }

    public void AddActionInfo(string name,int maxPoints, Position coordinates, int id)
    {
        GameObject temp = Instantiate(actionPrefab, actionsContent.transform);
        temp.transform.GetChild(0).GetComponent<Text>().text = name;
        temp.transform.GetChild(1).GetComponent<Text>().text = maxPoints.ToString();
        temp.GetComponent<ActionHandler>().ActionIndex = id;
        temp.GetComponent<ActionHandler>().coordinates = coordinates;
    }

    public void DeselectAll()
    {
        for (int i = 0; i < actionsContent.transform.childCount; i++)
        {
            actionsContent.transform.GetChild(i).GetComponent<ActionHandler>().Deselect();
        }
    }

    public void CloseAdminMode()
    {
        adminPanel.SetActive(false);
    }

    public void ToggleChanged()
    {
        if (toggle.isOn) SaveDevice();
        else RemoveDevice();
    }

    public void BackToLocs()
    {
        debugPanel.SetActive(false);
        locationsPanel.SetActive(true);
        InputManager.instance.Disconnect();
    }

    bool IsSaved()
    {
        if (!PlayerPrefs.HasKey("debugDevice") || PlayerPrefs.GetInt("debugDevice") == 0) return false;
        else return true;
    }

    void SaveDevice()
    {
        PlayerPrefs.SetInt("debugDevice", 1);
        PlayerPrefs.Save();
    }

    void RemoveDevice()
    {
        PlayerPrefs.SetInt("debugDevice", 0);
        PlayerPrefs.Save();
    }
}

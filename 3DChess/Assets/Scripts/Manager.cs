using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCPWhite;

public class Manager : MonoBehaviour {

    public enum PlayerColor
    {
        Black = 1,
        White = 2
    }

    #region Singleton
    public static Manager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject loadingPanel, messagePanel, gameLabel, loginPanel, menuPanel, opponentLabel,blackFigures,whiteFigures,turnPanel,chatPanel;
    public Animator anim;


    public delegate void OnMatchStarted();
    public OnMatchStarted onMatchStarted;

    public string playerName;
    public PlayerColor playerColor;
    
    public Text messagePanelText,infoText,opponentText;

    public void LoadMenu() // Load Main Menu
    {
        loginPanel.SetActive(false);
        menuPanel.SetActive(true);
        ServerListener.instance.SendMessageToServer(new Message(ID.InfoRequest));
    }

    public bool NothingToHighlight
    {
        get
        {
            GameObject ownFigures;

            if (playerColor == PlayerColor.Black) ownFigures = blackFigures;
            else ownFigures = whiteFigures;

            for (int i = 0; i < ownFigures.transform.childCount; i++)
            {
                GameObject temp = ownFigures.transform.GetChild(i).gameObject;
                if (temp.GetComponentInChildren<HighlightManager>().toHighlight.Count > 0) return false;
            }
            return true;
        }
    }

    public void BackToMenu() // Go back to Main Menu
    {
        if (playerColor == PlayerColor.Black)
        {
            anim.Play("BackFromBlack");
        }
        else
        {
            anim.Play("BackFromWhite");
        }
        menuPanel.SetActive(true);
        gameLabel.SetActive(true);
        opponentLabel.SetActive(false);
        chatPanel.SetActive(false);
        Chat.instance.enabledChat = false;
    }

    public void SwitchTurnPanel()
    {
        turnPanel.SetActive(!turnPanel.activeSelf);
    }

    public void HideTurnPanel()
    {
        turnPanel.SetActive(false);
    }

    public void UpdateInfo(string name,int winGames,int lostGames) // Update player info UI
    {
        float ratio;
        playerName = name;
        if (lostGames == 0) ratio = (float)winGames / 1;
        else ratio = (float)winGames / (float)lostGames;
        infoText.text = string.Format("{0}\n----------------------\nGames Won: {1}\nGames Lost: {2}\nLoss/Win Ratio: {3:f}", name, winGames, lostGames, ratio);
    }

    public void WaitForMatch() // Request server for match and wait for its start
    {
        ShowLoadingPanel();
        ServerListener.instance.SendMessageToServer(new Message(ID.MatchRequest));
    }

    public void Play()
    {
        chatPanel.SetActive(true);
        Chat.instance.enabledChat = true;
        menuPanel.SetActive(false);
        gameLabel.SetActive(false);
        opponentLabel.SetActive(true);
        if (playerColor == PlayerColor.Black)
        {
            for (int i = 0; i < blackFigures.transform.childCount; i++)
            {
                blackFigures.transform.GetChild(i).GetComponent<Figure>().isYours = true;
                if (blackFigures.transform.GetChild(i).GetComponent<Figure>().IsKing) ChessManager.instance.king = blackFigures.transform.GetChild(i).GetComponent<Figure>();
            }
            anim.Play("MoveCameraToBlack");
        }
        else
        {
            for (int i = 0; i < whiteFigures.transform.childCount; i++)
            {
                whiteFigures.transform.GetChild(i).GetComponent<Figure>().isYours = true;
                if (whiteFigures.transform.GetChild(i).GetComponent<Figure>().IsKing) ChessManager.instance.king = whiteFigures.transform.GetChild(i).GetComponent<Figure>();
            }
            anim.Play("MoveCameraToWhite");
        }
        onMatchStarted.Invoke();
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void LoadLoginMenu()
    {
        loginPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    public void ShowMessagePanel(string text,Color color,float time)
    {
        messagePanel.SetActive(true);
        messagePanelText.text = text;
        messagePanelText.color = color;
        StartCoroutine(HidePanel(time));
    }

    IEnumerator HidePanel(float time)
    {
        yield return new WaitForSeconds(time);
        messagePanel.SetActive(false);
    }

    public void LeaveMatch()
    {
        ShowLoadingPanel();
        ServerListener.instance.SendMessageToServer(new Message(ID.LeaveMatch));
    }

    void OnApplicationQuit()
    {
        if(ServerListener.instance.IsConnected())ServerListener.instance.SendMessageToServer(new Message(ID.LogoutRequest));

        ChessManager.instance.SaveLog();
    }

    public void QuitApp()
    {
        Application.Quit();
    }
	
}

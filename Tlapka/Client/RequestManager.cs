using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RequestManager : MonoBehaviour
{
    #region Singleton
    public static RequestManager Instance { get; set; }
    public void Awake()
    {
        Instance = this;
    }
    #endregion

    bool isSending = false;
    [SerializeField]
    List<Request> requests = new List<Request>();

    private void Update()
    {
        if(!isSending && requests.Count > 0)
        {
            SendRequest();
        }
    }

    public void FormRequest(int id,string link)
    {
        requests.Add(new Request(id, link));
    }

    public void FormRequest(int id,string link,List<IMultipartFormSection> form)
    {
        requests.Add(new Request(id, link, form));
    }

    public void FormRequest(int id,string link,object[] data)
    {
        Request r = new Request(id, link);
        r.Data = data;
        requests.Add(r);
    }

    public void FormRequest(int id, string link,List<IMultipartFormSection> form, object[] data)
    {
        Request r = new Request(id, link,form);
        r.Data = data;
        requests.Add(r);
    }

    void SendRequest()
    {
        isSending = true;
        StartCoroutine(SendRequestC());
    }

    IEnumerator SendRequestC()
    {
        Request toSend = requests[0];
        InputManager.instance.StartLoading();
        print("Connecting to: "+toSend.Link);
        UnityWebRequest request;
        if (toSend.HasForm) request = UnityWebRequest.Post(toSend.Link, ToWWWForm(toSend.Form));
        else request = UnityWebRequest.Get(toSend.Link);
        request.chunkedTransfer = false;
        request.useHttpContinue = false;
        request.SetRequestHeader("upgrade-insecure-requests", "1");
        yield return request.SendWebRequest();
        RequestHandler.Instance.Handle(toSend.ID, request.downloadHandler.text);
        request.Dispose();
        requests.RemoveAt(0);
        isSending = false;
    }

    WWWForm ToWWWForm(List<IMultipartFormSection> form)
    {
        WWWForm temp = new WWWForm();
        for (int i = 0; i < form.Count; i++)
        {
            string key = form[i].sectionName;
            string value = Encoding.UTF8.GetString(form[i].sectionData, 0, form[i].sectionData.Length);
            temp.AddField(key, value);
        }
        return temp;
    }

}
public class Request
{
    [SerializeField]
    public int ID { get; set; }
    [SerializeField]
    public string Link { get; set; }
    public bool Finished { get; set; }
    public List<IMultipartFormSection> Form { get; set; }
    [SerializeField]
    public bool HasForm { get; set; }
    [SerializeField]
    public object[] Data { get; set; }

    public Request(int id,string link)
    {
        ID = id;
        Link = link;
        HasForm = false;
    }

    public Request(int id,string link,List<IMultipartFormSection> form) : this(id, link)
    {
        Form = form;
        HasForm = true;
    }

}

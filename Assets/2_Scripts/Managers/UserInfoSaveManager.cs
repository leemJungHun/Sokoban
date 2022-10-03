using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class UserInfoSaveManager : MonoBehaviour
{
    static UserInfoSaveManager _unique;

    string _id;
    string _name;
    int _playTime;

    
    public static UserInfoSaveManager _Instance
    {
        get { return _unique; }
    }

    private void Awake()
    {
        _unique = this;    
    }

    public void SaveXML(int playTime)
    {
        _id = FaceBookManager._Instance.GetId;
        _name = FaceBookManager._Instance.GetName;
        _playTime = playTime;
        WriteXml("UserInfo.xml");
    }

    public void WriteXml(string filePath)
    {
        XmlDocument Document = new XmlDocument();
        XmlElement userInfo = Document.CreateElement("UserInfo");
        Document.AppendChild(userInfo);

        XmlElement idElement = Document.CreateElement(_name);
        idElement.SetAttribute("이름", _name);
        idElement.SetAttribute("플레이타임", _playTime.ToString());
        userInfo.AppendChild(idElement);
        Document.Save(filePath);
    }

    public void ReadXml(string filePath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        XmlElement list = doc["UserInfo"];
        foreach (XmlElement Info in list.ChildNodes)
        {
            string name = Info.GetAttribute("이름");
            int playTime = System.Convert.ToInt32(Info.GetAttribute("플레이타임"));

            Debug.Log("name" + name);
            Debug.Log("playTime" + playTime);
        }
    }
}

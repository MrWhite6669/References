using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance {get;set;}

    void Awake()
    {
        Instance = this;
    }
    
    public JsonData user;
    [SerializeField]
    public List<Location> locations = new List<Location>();
    public Location nearestLocation;
    public Location currentLocation;
    public string locationCode;
    public bool nearLocation = false;

    public bool DebugMode
    {
        get
        {
            if (PlayerPrefs.HasKey("debug")) return bool.Parse(PlayerPrefs.GetString("debug"));
            else return false;
        }
        set
        {
            PlayerPrefs.SetString("debug", value.ToString());
            PlayerPrefs.Save();
        }
    }

    public Location GetByIndex(int index)
    {
        foreach (Location l in locations) if (l.Index == index) return l;
        return nearestLocation;
    }

    public void GetLocations(JsonData data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            Location loc = new Location(int.Parse(data[i]["index"].ToString()),data[i]["name"].ToString(), data[i]["location"].ToString(), int.Parse(data[i]["maxpoints"].ToString()));
            loc.DistanceInMeters = (int)(GPS.instance.Distance(loc.Coordinates) * 1000);
            locations.Add(loc);
            DebugModeManager.Instance.AddActionInfo(loc.Name, loc.MaxPoints, loc.Coordinates,loc.Index);
        }
    }

    public void GetNearestLocation()
    {
        Location nearest = locations[0];
        foreach(Location loc in locations)
        {
            if (GPS.instance.Distance(nearest.Coordinates) > GPS.instance.Distance(loc.Coordinates)) nearest = loc;
            print(loc.Name + "|" + GPS.instance.Distance(loc.Coordinates) + "Km");
        }
        nearestLocation = nearest;
        if (GPS.instance.Distance(nearestLocation.Coordinates) <= 0.100f) nearLocation = true;
        else nearLocation = false;
    }
}


public class Location
{
    public int Index { get; set; }
    public string Name { get; set; }
    public Position Coordinates { get; set; }
    public int MaxPoints { get; set; }

    public int DistanceInMeters { get; set; }

    public Location(int index,string name,string coordinates,int maxPoints)
    {
        Index = index;
        Name = name;
        string[] data = coordinates.Split(';');
        Coordinates = new Position(double.Parse(data[0]), double.Parse(data[1]), int.Parse(data[2]));
        MaxPoints = maxPoints;
    }
}

public class Position
{
    [SerializeField]
    public double Latitude { get; set; }
    [SerializeField]
    public double Longitude { get; set; }
    [SerializeField]
    public int Altitude { get; set; }

    public Position(double lat, double lon,int alt)
    {
        Latitude = lat;
        Longitude = lon;
        Altitude = alt;
    }

    public Vector3 ToVector()
    {
        return new Vector3((float)Longitude, (float)Latitude, Altitude);
    }
}

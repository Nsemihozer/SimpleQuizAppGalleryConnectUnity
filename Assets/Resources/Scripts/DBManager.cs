using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Huawei.Agconnect;
using Huawei.Agconnect.Auth;
using Huawei.Agconnect.CloudDB;
using Newtonsoft.Json;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager _instance;
    private AGConnectCloudDB CloudDB;
    private CloudDBZone _zone;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
            CloudDB = AGConnectCloudDB.GetInstance(AGConnectInstance.Instance,AGConnectAuth.Instance);
            InitJsonObjectTypes();
            OpenCloudZone();
        }
    }

    private void InitJsonObjectTypes()
    {
        Debug.Log("Start Create Schema");
        try
        {
            string json = Resources.Load<TextAsset>("UnityCloudDB").text;
            ObjectTypeInfo objectTypeInfo = JsonConvert.DeserializeObject<ObjectTypeInfo>(json);
            CloudDB.CreateObjectType(objectTypeInfo);
            Debug.Log("Schema Create Success");
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError("Schema Create Failed: " + ex.ErrorMessage);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Schema Create Failed: " + ex.Message);
        }
    }

    private async void OpenCloudZone()
    {
        try
        {
            Debug.Log("Try To Open Zone");
            _zone = await CloudDB.OpenCloudDBZone(new CloudDBZoneConfig("SimpleQuizDE"));
            await CloudDB.SetUserKey("123123", null, false);
            Debug.Log("Open Zone is Successful");
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError("Open Zone Failed: " + ex.ErrorMessage);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Open Zone Failed: " + ex.Message);
        }
    }

    public async Task<int> ExecuteUpsert<T>(T upsertObject) where T:CloudDBZoneObject
    {
        int result = 0;
        try
        {
             result=await _zone.ExecuteUpsert(upsertObject);
        }
        catch (Exception e)
        {
           Debug.Log(e.Message);
        }

        return result;
    }
    
    public async Task<List<T>> ExecuteQuery<T>(string fieldName,string value) where T:CloudDBZoneObject
    {
        List<T> result = new List<T>();
        try
        {
            var query = CloudDBZoneQuery<T>.Where(typeof(T)).EqualTo(fieldName, value);
            var snapshot=await _zone.ExecuteQuery(query);
            result = snapshot.GetSnapshotObjects();
            snapshot.Release();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return result;
    }
}
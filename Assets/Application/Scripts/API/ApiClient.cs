using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This is the ALE Client implementation to be used in the app for API access.
/// NOTE: This is implemented as a *PARTIAL* class. This means that it is
/// combined with the other partial definitions of this class at compile time.
/// The other implementations per type can be found in the same scripts folder
/// as this script with name AleClient-[type(s)].cs
/// </summary>
[RequireComponent(typeof(ApplicationManager))]
public partial class ApiClient : MonoBehaviour
{
    private ApplicationManager _app = null;
    private HttpClient _client;

    private const string BEARER = "bearer";
    private const string GET_SAMPLE_DATA = "/YourApi/GetSampleData";

    /// <summary>
    /// Event for notifying others the internal data structure was
    /// loaded, updated or changed.
    /// </summary>
    public delegate void DataUpdated();
    public event DataUpdated OnDataUpdated;

    /// <summary>
    /// Gets the data from memory. 
    /// 
    /// TODO:
    /// For demo purposes this is now a 
    /// list or strings, but can contain the structure you need for 
    /// your solution
    /// </summary>
    private List<string> _data = new List<string>();
    public List<string> Data { get { return _data; }  }

    /// <summary>
    /// Initialize this client for calls and load basic data for the current user.
    /// </summary>
    public async Task InitializeAsync()
    {
        _app = GetComponent<ApplicationManager>();

        _client = new HttpClient();
        _client.BaseAddress = new Uri(_app.SettingsManager.Settings.BaseEndPointUrl);
        Debug.Log($"BaseAddress: {_app.SettingsManager.Settings.BaseEndPointUrl}");

        // TODO: add other initialization here, like retrieving data
    }

    /// <summary>
    /// On destroy of the game object we're hosted in, clean up.
    /// </summary>
    private void OnDestroy()
    {
        if (_client != null)
        {
            _client.Dispose();
            _client = null;
        }
    }

    /// <summary>
    /// Get all the data.
    /// 
    /// TODO:
    /// This is now just sample code, as it's not backed by a real API.
    /// Change the type, api endpoint and such here.
    /// </summary>
    /// <param name="forced">Force load from the backend true/false. Default is false.</param>
    /// <returns>A list of <see cref="string"/> objects.</returns>
    public async Task<List<string>> GetSampleData(bool forced = false)
    {
        if (!forced && _data != null)
        {
            // if not forced and we have data, return the cache.
            return _data;
        }

        string url = GET_SAMPLE_DATA;
        string token = await _app.AuthenticationManager.GetAccessTokenAsync();

        // setup the JWT token as bearer for authenticated API access
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(BEARER, token);

        try
        {
            // get the data from the backend API
            // TODO: change for you backend and data structure.
            string json = await _client.GetStringAsync(url);
            _data = JsonConvert.DeserializeObject<List<string>>(json);

            // let others know data has changed.
            OnDataUpdated?.Invoke();

            return _data;
        }
        catch (Exception ex)
        {
            Debug.LogError($"ApiClient.GetSampleData ERROR: url={url}\n{ex}");
        }

        return null;
    }
}

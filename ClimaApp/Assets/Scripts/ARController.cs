using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using DG.Tweening;


public class ARController : MonoBehaviour
{
    [SerializeField]
    private GameObject AR;

    [SerializeField]
    private RawImage iconoClima;

    [SerializeField]
    private TextMeshProUGUI ciudad;

    [SerializeField]
    private TextMeshProUGUI pais;

    [SerializeField]
    private TextMeshProUGUI temperatura;

    [SerializeField]
    private TextMeshProUGUI clima;

    private string url_api = "http://api.weatherstack.com/current?access_key=6e18253a21a22301779783e7d9c30293&query=";
    private string ciudadActual = "SanPedroSula";
    private string url_img;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ObtenerInfo(ciudadActual));
    }

    IEnumerator ObtenerInfo(string city)
    {
        UnityWebRequest www = UnityWebRequest.Get(url_api + city);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JSONNode info = JSON.Parse(www.downloadHandler.text);

            string nombre_ciudad = info["location"]["name"];
            string nombre_pais = info["location"]["country"];
            string temp = info["current"]["temperature"];
            string url_icons = info["current"]["weather_icons"][0];
            string nombre_clima = info["current"]["weather_descriptions"][0];
            string IsDay = info["current"]["is_Day"];

            UnityWebRequest img = UnityWebRequestTexture.GetTexture(url_icons);
            yield return img.SendWebRequest();

            if(IsDay == "yes")
            {
                iconoClima.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(img);

            }
            else
            {
                iconoClima.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(img);
            }
            ciudad.text = nombre_ciudad;
            pais.text = nombre_pais;
            temperatura.text = temp + " °C";
            clima.text = nombre_clima;

            AR.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

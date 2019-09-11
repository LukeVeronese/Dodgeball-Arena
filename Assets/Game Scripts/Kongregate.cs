using UnityEngine;
using System.Collections;
 
public class Kongregate : MonoBehaviour
{
    static Kongregate instance;
 
    void Start()
    {
        if(instance == null){
            Application.ExternalEval("if(typeof(kongregateUnitySupport) != 'undefined'){kongregateUnitySupport.initAPI('" + gameObject.name + "', 'OnKongregateAPILoaded');};");
            instance = this;
        }
	}
 
    static bool isKongregate = false;
    static uint userId = 0;
	static public string username = "Guest";
    static string gameAuthToken = "";

    void OnKongregateAPILoaded(string userInfoString)
    {
        // We now know we're on Kongregate
        isKongregate = true;
        // Split the user info up into tokens
        string[] parameters = new string[3];
        parameters = userInfoString.Split("|"[0]);
        userId = uint.Parse(parameters[0]);
        username = parameters[1];
        gameAuthToken = parameters[2];

    }
	
  
    public static void SubmitStatistic(string stat, int val)
    {
        if(isKongregate)
        {Application.ExternalCall("kongregate.stats.submit",stat,val);}
    }
}

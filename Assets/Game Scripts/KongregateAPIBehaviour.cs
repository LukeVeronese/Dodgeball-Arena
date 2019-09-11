using UnityEngine;
using System.Collections;

public class KongregateAPIBehaviour : MonoBehaviour {
	
		private static KongregateAPIBehaviour instance;

	public MultiplayerManager mpManager;

		public void Start() {

				if(instance == null) {
						instance = this;
				} else if(instance != this) {
						Destroy(gameObject);
						return;
				}

				Object.DontDestroyOnLoad(gameObject);
				gameObject.name = "KongregateAPI";

				Application.ExternalEval(
						@"if(typeof(kongregateUnitySupport) != 'undefined'){
        kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');
      };"
    );

		mpManager = (MultiplayerManager)FindObjectOfType(typeof(MultiplayerManager));
		}

		public void OnKongregateAPILoaded(string userInfoString) {
				OnKongregateUserInfo(userInfoString);

			Application.ExternalEval(@"
		      kongregate.services.addEventListener('login', function(){
		        var unityObject = kongregateUnitySupport.getUnityObject();
		        var services = kongregate.services;
		        var params=[services.getUserId(), services.getUsername(), 
		                    services.getGameAuthToken()].join('|');

		        unityObject.SendMessage('KongregateAPI', 'OnKongregateUserInfo', params);
		    });"
		  );
		}  

		public void OnKongregateUserInfo(string userInfoString) {
				var info = userInfoString.Split('|');
				var userId = System.Convert.ToInt32(info[0]);
				var username = info[1];
				var gameAuthToken = info[2];
				Debug.Log("Kongregate User Info: " + username + ", userId: " + userId);

				Application.ExternalCall ("kongregate.services.getUsername()");
				mpManager.playerName.text = username;
		}
}
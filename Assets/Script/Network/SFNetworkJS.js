#pragma strict
import UnityEngine.Networking;
import UnityEngine.Networking.Match;
 
 
 
public class SFNetworkJS extends NetworkManager{
 
private var networkMatch:NetworkMatch;
 
	function Update(){
		if(networkMatch == null){
			var nm = GetComponent(NetworkMatch);
			if(nm != null){
				networkMatch = nm as NetworkMatch;
				var appid:UnityEngine.Networking.Types.AppID;
				appid= 944512;
				networkMatch.SetProgramAppID(appid);
			}
		}
	}
}
using UnityEngine;

public class NameSync : Photon.MonoBehaviour {

    //Name is sent to other clients once room is joined
    //When new client joins, other clients same name to it

	void Start () {

        photonView.RPC("SyncNames", PhotonTargets.All, transform.GetComponent<TextMesh>().text);
    }

    void OnPhotonPlayerConnected() {

        photonView.RPC("SyncNames", PhotonTargets.All, transform.GetComponent<TextMesh>().text);
    }

    [PunRPC]
    void SyncNames(string playerName, PhotonMessageInfo info)
    {
        transform.GetComponent<TextMesh>().text = playerName;
    }
}

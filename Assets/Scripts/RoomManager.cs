using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject player;
    [Space]
    public Transform[] spawnPoints;
    [Space]
    public GameObject roomcam;
    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickName = "unnamed";

    public string roomNameToJoin = "test";

    [HideInInspector]
    public int kills= 0;
    [HideInInspector]
    public int deaths= 0;
    private void Awake()
    {
        instance = this;
    }

    public void ChangeNickname(string _name)
    {
        nickName = _name;
    }

    public void JoinRoomButton()
    {
        Debug.Log("connecting");

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null); // Photon sunucusuna baðlanmasý

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }

   

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("we're connected and in a room now");
        roomcam.SetActive(false);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickName", RpcTarget.AllBuffered, nickName);
        PhotonNetwork.LocalPlayer.NickName = nickName;
    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
        catch
        {

        }
    }
}

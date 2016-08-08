using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Meg.Networking
{

    public class NetworkManager : UnityEngine.Networking.NetworkManager
    {

        // Public Methods
        // ------------------------------------------------------------

        public override void OnClientConnect(NetworkConnection conn)
        {
            Log("OnClientConnect", conn);
            base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            Log("OnClientDisconnect", conn);
            base.OnClientDisconnect(conn);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            Log("OnClientError", conn, errorCode);
            base.OnClientError(conn, errorCode);
        }

        public override void OnClientNotReady(NetworkConnection conn)
        {
            Log("OnClientNotReady", conn);
            base.OnClientNotReady(conn);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            Log("OnClientSceneChanged", conn);
            base.OnClientSceneChanged(conn);
        }

        public override void OnMatchCreate(CreateMatchResponse matchInfo)
        {
            Log("OnMatchCreate", matchInfo);
            base.OnMatchCreate(matchInfo);
        }

        public override void OnMatchList(ListMatchResponse matchList)
        {
            Log("OnMatchList", matchList);
            base.OnMatchList(matchList);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            Log("OnServerAddPlayer", conn, string.Format("playerControllerId:{0}", playerControllerId));
            base.OnServerAddPlayer(conn, playerControllerId);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,
            NetworkReader extraMessageReader)
        {
            Log("OnServerAddPlayer", conn, string.Format("playerControllerId:{0}", playerControllerId));
            base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            Log("OnServerConnect", conn);
            base.OnServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            Log("OnServerDisconnect", conn);
            base.OnServerDisconnect(conn);
        }

        public override void OnServerError(NetworkConnection conn, int errorCode)
        {
            Log("OnServerError", conn, errorCode);
            base.OnServerError(conn, errorCode);
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            Log("OnServerReady", conn);
            base.OnServerReady(conn);
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            Log("OnServerRemovePlayer", conn, string.Format("player:{0}", player));
            base.OnServerRemovePlayer(conn, player);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            Log("OnServerRemovePlayer", string.Format("sceneName:{0}", sceneName));
            base.OnServerSceneChanged(sceneName);
        }

        public override void OnStartClient(NetworkClient client)
        {
            Log("OnStartClient", string.Format("client:{0}", client));
            base.OnStartClient(client);
        }

        public override void OnStartHost()
        {
            Log("OnStartHost");
            base.OnStartHost();
        }

        public override void OnStartServer()
        {
            Log("OnStartServer");
            base.OnStartServer();
        }

        public override void OnStopClient()
        {
            Log("OnStopClient");
            base.OnStopClient();
        }

        public override void OnStopHost()
        {
            Log("OnStopHost");
            base.OnStopHost();
        }

        public override void OnStopServer()
        {
            Log("OnStopServer");
            base.OnStopServer();
        }

        public override void ServerChangeScene(string newSceneName)
        {
            Log("ServerChangeScene", string.Format("newSceneName:{0}", newSceneName));
            base.ServerChangeScene(newSceneName);
        }


        // Private Methods
        // ------------------------------------------------------------

        private static string Id
            { get { return Configuration.Instance.CurrentId; } }

        private void Log(string method)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1})", method, Id)); }

        private void Log(string method, string message)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1}): {2}", method, Id, message)); }

        private void Log(string method, NetworkConnection conn)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1}, conn:{2})", method, Id, conn)); }

        private void Log(string method, NetworkConnection conn, string message)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1}, conn:{2}): ", method, Id, message)); }

        private void Log(string method, NetworkConnection conn, int errorCode)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1}, conn:{2}, errorCode:{3})", method, Id, conn, errorCode)); }

        private void Log(string method, CreateMatchResponse matchInfo)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1}, matchInfo:{2})", method, Id, matchInfo)); }

        private void Log(string method, ListMatchResponse matchList)
            { Debug.Log(string.Format("NetworkManager.{0}(id:{1}, matchList:{2})", method, Id, matchList)); }

    }

}

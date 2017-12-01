using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Character;
using Chowizard.UnityNetwork.Client.Core;

namespace Chowizard.UnityNetwork.Client.Network
{
    public class NetworkManager : MonoBehaviour
    {
        public enum eMode
        {
            None = 0,

            Server,
            Client
        }



        public string address = "127.0.0.1";
        public ushort port = 15632;
        public bool isAtStartup = true;
        public eMode mode = eMode.None;

        [Space]
        public float enableToSyncDistance = 5.0f;
        public float checkSyncDiatanceInterval = 1.0f;

        public Dictionary<string, GameObject> spawningPrefabs = new Dictionary<string, GameObject>();

        private ConnectionConfig connectionConfiguration;

        private NetworkControllerServer serverController;
        private NetworkControllerClient clientController;

        private float elapsedTimeToCheckSyncDiatance;

        [HideInInspector]
        public string message;

        public static NetworkManager Instance
        {
            get
            {
                if(GameManager.Singleton == null)
                    return null;

                return GameManager.Singleton.networkManager;
            }
        }

        public void Terminate()
        {
            if(isAtStartup == true)
                return;

            switch(mode)
            {
            case eMode.Server:
                TerminateAtServer();
                break;

            case eMode.Client:
                TerminateAtClient();
                break;
            }

            isAtStartup = true;
        }

        // Create a server and listen on a port
        public bool StartByServer()
        {
            if(isAtStartup == false)
                return false;

            if(serverController == null)
                serverController = new NetworkControllerServer(this);

            if(serverController.Setup() == false)
                return false;

            mode = eMode.Server;
            isAtStartup = false;
            message = "Setup server.";

            return true;
        }

        // Create a client and connect to the server port
        public bool StartByClient()
        {
            if(isAtStartup == false)
                return false;

            if(clientController == null)
                clientController = new NetworkControllerClient(this);

            clientController.Setup();

            mode = eMode.Client;
            isAtStartup = false;
            message = "Setup client.";

            return true;
        }

        public CharacterEntity RegisterPlayerCharacter(int ownerNetConnectionId, short playerControllId)
        {
            return RegisterPlayerCharacter(ownerNetConnectionId, playerControllId, Vector3.zero, Quaternion.identity);
        }

        public CharacterEntity RegisterPlayerCharacter(int ownerNetConnectionId,
                                                       short playerControllId,
                                                       Vector3 position,
                                                       Quaternion rotation)
        {
            GameObject prefab = GetSpawningPrefab(Defines.SpawningPrefab.PlayerCharacter);

            CharacterEntity entity = EntityManager.Instance.CreatePlayerCharacter(prefab, ownerNetConnectionId, playerControllId);

            return entity;
        }

        public CharacterEntity RegisterNonPlayerCharacter()
        {
            return RegisterNonPlayerCharacter(Vector3.zero, Quaternion.identity);
        }

        public CharacterEntity RegisterNonPlayerCharacter(Vector3 position, Quaternion rotation)
        {
            GameObject prefab = GetSpawningPrefab(Defines.SpawningPrefab.NonPlayerCharacter);

            CharacterEntity entity = EntityManager.Instance.CreateNonPlayerCharacter(prefab, position, rotation);

            return entity;
        }

        public void UnregisterPlayerCharacter(int ownerNetConnectionId)
        {
            CharacterEntity[] myControlledEntities = EntityManager.Instance.GetMyControlledEntities(ownerNetConnectionId);
            if(myControlledEntities == null)
                return;

            foreach(CharacterEntity myEntity in myControlledEntities)
            {
                if(myEntity == null)
                    continue;

                //EntityManager.Instance.RemoveEntity(myEntity.netId.Value);
                EntityManager.Instance.DestroyEntity(myEntity);
            }
        }

        public NetworkControllerServer ServerController
        {
            get
            {
                return serverController;
            }
        }

        public NetworkControllerClient ClientController
        {
            get
            {
                return clientController;
            }
        }

        public bool IsReadyByServer
        {
            get
            {
                if(mode != eMode.Server)
                    return false;

                if(isAtStartup == true)
                    return false;

                return true;
            }
        }

        public bool IsReadyByClient
        {
            get
            {
                if(mode != eMode.Client)
                    return false;

                if(isAtStartup == true)
                    return false;

                return true;
            }
        }

        private void Start()
        {
            SetupConnectionConfiguration();
            SetupResources();
        }

        // Update is called once per frame
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                Terminate();
        }

        private void FixedUpdate()
        {
            if(mode == eMode.Server)
            {
                if(elapsedTimeToCheckSyncDiatance >= NetworkManager.Instance.checkSyncDiatanceInterval)
                {
                    UpdateSyncDistance();
                    elapsedTimeToCheckSyncDiatance = 0.0f;
                }
                else
                {
                    elapsedTimeToCheckSyncDiatance += Time.fixedDeltaTime;
                }
            }
        }

        private void SetupConnectionConfiguration()
        {
            if(connectionConfiguration == null)
                connectionConfiguration = new ConnectionConfig();

            connectionConfiguration.PingTimeout = 500;
            connectionConfiguration.DisconnectTimeout = 2000;
            //connectionConfiguration.AddChannel(QosType.Reliable);
        }

        private void SetupResources()
        {
            ClearSpawningPrefab();

            RegisterSpawningPrefab(Defines.SpawningPrefab.PlayerCharacter, "Entity/PlayerCharacter2");
            RegisterSpawningPrefab(Defines.SpawningPrefab.NonPlayerCharacter, "Entity/NonPlayerCharacter2");
            RegisterSpawningPrefab(Defines.SpawningPrefab.Bullet, "Entity/Bullet");
        }

        private void RegisterSpawningPrefab(string keyName, string resourcePath)
        {
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
            Debug.Assert(prefab != null);

            AddSpawningPrefab(keyName, prefab);
            ClientScene.RegisterPrefab(prefab);
        }

        private void UnregisterSpawningPrefab(string keyName)
        {
            GameObject prefab = GetSpawningPrefab(keyName);
            if(prefab == null)
                return;

            ClientScene.UnregisterPrefab(prefab);
            RemoveSpawningPrefab(keyName);
        }

        private void TerminateAtServer()
        {
            if(serverController != null)
            {
                serverController.Terminate();
                serverController = null;
            }

            message = "Server was terminate.";
            Debug.Log(message);
        }

        private void TerminateAtClient()
        {
            if(clientController != null)
            {
                clientController.Terminate();
                clientController = null;
            }

            message = "Client was terminate.";
            Debug.Log(message);
        }

        private void ClearSpawningPrefab()
        {
            spawningPrefabs.Clear();
        }

        private void AddSpawningPrefab(string keyName, GameObject prefab)
        {
            Debug.Assert(string.IsNullOrEmpty(keyName) == false);
            Debug.Assert(prefab != null);
            if(prefab == null)
                return;

            spawningPrefabs.Add(keyName, prefab);
        }

        private void RemoveSpawningPrefab(string keyName)
        {
            Debug.Assert(string.IsNullOrEmpty(keyName) == false);

            spawningPrefabs.Remove(keyName);
        }

        public GameObject GetSpawningPrefab(string keyName)
        {
            Debug.Assert(string.IsNullOrEmpty(keyName) == false);

            GameObject data;
            return spawningPrefabs.TryGetValue(keyName, out data) ? data : null;
        }

        private void UpdateSyncDistance()
        {
            if(EntityManager.Instance.EntityCount <= 0)
                return;

            if(EntityManager.Instance.PlayerCount <= 0)
                return;

            int syncCharactersCount = 0;
            CharacterEntity[] characters = EntityManager.Instance.Entities;
            CharacterEntity[] playerCharacters = EntityManager.Instance.Players;
            foreach(CharacterEntity character in characters)
            {
                if(character == null)
                    continue;

                if(character.property.isPlayer == true)
                    continue;

                NetworkTransformSynchronizer networkTransformSync = character.GetComponent<NetworkTransformSynchronizer>();
                if(networkTransformSync == null)
                    continue;

                bool enableToSync = false;

                foreach(CharacterEntity player in playerCharacters)
                {
                    if(player == null)
                        continue;

                    Vector3 myPosition = character.transform.position;
                    Vector3 playerPosition = player.transform.position;
                    float distanceSqr2 = Vector3.SqrMagnitude(playerPosition - myPosition);
                    float enableToSyncDistanceSqr2 = enableToSyncDistance * enableToSyncDistance;

                    if(distanceSqr2 <= enableToSyncDistanceSqr2)
                    {
                        enableToSync = true;
                        ++syncCharactersCount;
                        break;
                    }
                }

                if(networkTransformSync.enabled != enableToSync)
                    networkTransformSync.enabled = enableToSync;
            }

            Debug.Log("Synchroize transform character count : " + syncCharactersCount);
        }
    }
}


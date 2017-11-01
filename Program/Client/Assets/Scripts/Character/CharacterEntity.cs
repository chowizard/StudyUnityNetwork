using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Character;

public class CharacterEntity : NetworkBehaviour
{
    public int id;

    public CharacterEntityProperty property;

    /* 최종 목료가 될 변환 */
    public Transform destination;

    public float moveSpeed = 10.0f;

    /* 전체 컴포넌트 목록 */
    private Dictionary<System.Type, CharacterComponent> components = new Dictionary<System.Type, CharacterComponent>();

    public ClassType AddCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        ClassType component = gameObject.AddComponent<ClassType>();
        if(component == null)
            return null;

        component.owner = this;

        Debug.Assert(components.ContainsKey(typeof(ClassType)) == false);
        components.Add(typeof(ClassType), component);

        return component;
    }

    public bool RemoveCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        ClassType component = gameObject.GetComponent<ClassType>();
        Destroy(component);

        return components.Remove(typeof(ClassType));
    }

    public ClassType GetCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        CharacterComponent data;
        return components.TryGetValue(typeof(ClassType), out data) ? data as ClassType : null;
    }

    public bool ActivateCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        return false;
    }

    public bool DeactivateCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        return false;
    }

    public bool ActivateAllCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        return false;
    }

    public bool DeactivateAllCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        return false;
    }

    public CharacterComponent[] Components
    {
        get
        {
            return (components.Count > 0) ? components.Values.ToArray() : null;
        }
    }

    private void Awake()
    {
        property = new CharacterEntityProperty();
    }

    public override void OnNetworkDestroy()
    {
        Debug.Log("OnNetworkDestroy");

        base.OnNetworkDestroy();
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer");

        base.OnStartLocalPlayer();

        int playerCharacterId = NetworkManager.Instance.ClientController.NetClient.connection.connectionId;
        EntityManager.Instance.MakePlayerCharacter(this, playerCharacterId);
        EntityManager.Instance.AddEntity(playerCharacterId, this);
        EntityManager.Instance.MyCharacter = this;

        SceneMain.Singleton.mainCamera.followTarget = gameObject;
        SceneMain.Singleton.mainCamera.isFollowTarget = true;

        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public override void OnStartServer()
    {
        Debug.Log("OnStartServer");

        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");

        base.OnStartClient();
    }

    public override void PreStartClient()
    {
        Debug.Log("PreStartClient");

        base.PreStartClient();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private Dictionary<int, CharacterEntity> playerCharacters = new Dictionary<int, CharacterEntity>();

    private CharacterEntity myCharacter;

    public static EntityManager Instance
    {
        get
        {
            return SceneMain.Singleton.entityManager;
        }
    }

    public CharacterEntity CreatePlayerCharacter(GameObject prefab, int id)
    {
        return CreatePlayerCharacter(prefab, id, Vector3.zero, Quaternion.identity);
    }

    public CharacterEntity CreatePlayerCharacter(GameObject prefab,
                                                 int id,
                                                 Vector3 position,
                                                 Quaternion rotation)
    {
        GameObject myPlayerObject = Object.Instantiate<GameObject>(prefab);
        myPlayerObject.name = prefab.name;
        myPlayerObject.transform.position = prefab.transform.position;

        CharacterEntity entity = myPlayerObject.transform.GetComponent<CharacterEntity>();
        entity.transform.position = position;
        entity.transform.rotation = rotation;

        MakePlayerCharacter(entity, id);

        return entity;
    }

    public bool DestroyPlayerCharacter(CharacterEntity entity)
    {
        if(entity == null)
            return false;

        Destroy(entity);
        entity = null;

        return true;
    }

    public bool MakePlayerCharacter(CharacterEntity entity, int id)
    {
        Debug.Assert(entity != null);
        if(entity == null)
            return false;

        entity.id = id;

        entity.property.isPlayer = true;

        entity.AddCharacterComponent<CharacterComponentAction>();
        entity.AddCharacterComponent<CharacterComponentAiPlayer>();
        entity.AddCharacterComponent<CharacterComponentInputControl>();
        entity.AddCharacterComponent<CharacterComponentMove>();

        return true;
    }

    public void AddPlayerCharacter(int id, CharacterEntity entity)
    {
        if(entity == null)
            return;

        playerCharacters.Add(id, entity);

        entity.transform.parent = transform;
    }

    public CharacterEntity RemovePlayerCharacter(int id)
    {
        if(!ExistPlayer)
            return null;

        CharacterEntity player = GetPlayerCharacter(id);
        if(player == null)
            return null;

        playerCharacters.Remove(id);

        return player;
    }

    public CharacterEntity GetPlayerCharacter(int id)
    {
        CharacterEntity data;
        return playerCharacters.TryGetValue(id, out data) ? data : null;
    }

    public CharacterEntity[] Players
    {
        get
        {
            return (PlayerCount > 0) ? playerCharacters.Values.ToArray() : null;
        }
    }

    public int PlayerCount
    {
        get
        {
            return playerCharacters.Count;
        }
    }

    public bool ExistPlayer
    {
        get
        {
            return (PlayerCount > 0) ? true : false;
        }
    }

    public CharacterEntity MyCharacter
    {
        get
        {
            return myCharacter;
        }
        set
        {
            myCharacter = value;
        }
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private Dictionary<int, CharacterEntity> entities = new Dictionary<int, CharacterEntity>();

    private CharacterEntity myCharacter;

    public static EntityManager Instance
    {
        get
        {
            return SceneMain.Singleton.entityManager;
        }
    }

    public bool DestroyEntity(CharacterEntity entity)
    {
        if(entity == null)
            return false;

        Destroy(entity.gameObject);
        entity = null;

        return true;
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

    public CharacterEntity CreateNonPlayerCharacter(GameObject prefab, int id)
    {
        return null;
    }

    public bool MakeNonPlayerCharacter(CharacterEntity entity, int id)
    {
        Debug.Assert(entity != null);
        if(entity == null)
            return false;

        entity.id = id;

        entity.property.isPlayer = false;

        return true;
    }

    public void AddEntity(int id, CharacterEntity entity)
    {
        if(entity == null)
            return;

        entities.Add(id, entity);

        entity.transform.parent = transform;
    }

    public CharacterEntity RemoveEntity(int id)
    {
        if(!ExistPlayer)
            return null;

        CharacterEntity player = GetEntity(id);
        if(player == null)
            return null;

        entities.Remove(id);

        return player;
    }

    public CharacterEntity GetEntity(int id)
    {
        CharacterEntity data;
        return entities.TryGetValue(id, out data) ? data : null;
    }

    public CharacterEntity[] Entities
    {
        get
        {
            return (EntityCount > 0) ? entities.Values.ToArray() : null;
        }
    }

    public int EntityCount
    {
        get
        {
            return entities.Count;
        }
    }

    public bool ExistPlayer
    {
        get
        {
            return (EntityCount > 0) ? true : false;
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private Dictionary<int, CharacterEntity> playerCharacters = new Dictionary<int, CharacterEntity>();

    private CharacterEntity myCharacter;

    public CharacterEntity CreatePlayerCharacter(GameObject prefab)
    {
        GameObject myPlayerObject = Object.Instantiate<GameObject>(prefab);
        myPlayerObject.name = prefab.name;
        myPlayerObject.transform.position = prefab.transform.position;
        myPlayerObject.transform.parent = transform;

        CharacterEntity playerCharacter = myPlayerObject.transform.GetComponent<CharacterEntity>();

        playerCharacter.property.isPlayer = true;

        playerCharacter.AddCharacterComponent(new CharacterComponentAction());
        playerCharacter.AddCharacterComponent(new CharacterComponentAiPlayer());
        playerCharacter.AddCharacterComponent(new CharacterComponentInputControl());
        playerCharacter.AddCharacterComponent(new CharacterComponentMove());

        return playerCharacter;
    }

    public void AddPlayerCharacter(int id, CharacterEntity player)
    {
        if(player == null)
            return;

        playerCharacters.Add(id, player);
    }

    public void RemovePlayerCharacter(int id)
    {
        if(!ExistPlayer)
            return;

        CharacterEntity player = GetPlayerCharacter(id);
        if(player == null)
            return;

        Destroy(player);
        player = null;

        playerCharacters.Remove(id);
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

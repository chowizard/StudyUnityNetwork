using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

public class CharacterEntity : NetworkBehaviour
{
    /* 최종 목료가 될 변환 */
    public Transform destination;

    /* 전체 컴포넌트 목록 */
    private Dictionary<System.Type, CharacterComponent> components = new Dictionary<System.Type, CharacterComponent>();

    public void AddCharacterComponent<ClassType>(ClassType component) where ClassType : CharacterComponent
    {
        Debug.Assert(components.ContainsKey(typeof(ClassType)) == false);
        components.Add(typeof(ClassType), component);
    }

    public bool RemoveCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        return components.Remove(typeof(ClassType));
    }

    public CharacterComponent GetCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        CharacterComponent data;
        return components.TryGetValue(typeof(ClassType), out data) ? data : null;
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

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }
}

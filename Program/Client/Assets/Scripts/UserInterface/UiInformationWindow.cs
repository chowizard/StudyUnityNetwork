using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiInformationWindow : MonoBehaviour
{
    public Text uiTextMyCharacterPosition;
    private Vector3 myCharacterPosition;

    public Text uiTextObjectCount;
    private int objectCount;

    // Use this for initialization
    private void Start()
    {
        if(uiTextMyCharacterPosition != null)
            uiTextMyCharacterPosition.text = string.Format("My Character Position : {0}", myCharacterPosition);

        if(uiTextObjectCount != null)
            uiTextObjectCount.text = string.Format("Object Count : {0}", 0);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMyCharacterPosition();
        UpdateObjectCount();
    }

    private void UpdateMyCharacterPosition()
    {
        if(uiTextMyCharacterPosition == null)
            return;

        CharacterEntity myCharacter = EntityManager.Instance.MyCharacter;
        if(myCharacter == null)
            return;

        if(myCharacter.transform.position == myCharacterPosition)
            return;

        myCharacterPosition = myCharacter.transform.position;
        uiTextMyCharacterPosition.text = string.Format("My Character Position : {0}", myCharacterPosition);
    }

    private void UpdateObjectCount()
    {
        if(uiTextObjectCount == null)
            return;

        if(objectCount == EntityManager.Instance.EntityCount)
            return;

        objectCount = EntityManager.Instance.EntityCount;
        uiTextObjectCount.text = string.Format("Object Count : {0}", objectCount);
    }
}

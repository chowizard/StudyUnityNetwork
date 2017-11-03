using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiInformationWindow : UiSet
{
    public Text uiTextCurrentMode;
    private NetworkManager.eMode networkMode = NetworkManager.eMode.None;

    public Text uiTextMyCharacterPosition;
    private Vector3 myCharacterPosition;

    public Text uiTextObjectCount;
    private int objectCount;

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(uiTextCurrentMode != null);
        uiTextCurrentMode.text = string.Format("Current Mode : {0}", networkMode);

        Debug.Assert(uiTextMyCharacterPosition != null);
        uiTextMyCharacterPosition.text = string.Format("My Character Position : {0}", myCharacterPosition);

        Debug.Assert(uiTextObjectCount != null);
        uiTextObjectCount.text = string.Format("Object Count : {0}", 0);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateCurrentMode();
        UpdateMyCharacterPosition();
        UpdateObjectCount();
    }

    private void UpdateCurrentMode()
    {
        if(uiTextCurrentMode == null)
            return;

        if(networkMode == NetworkManager.Instance.mode)
            return;

        networkMode = NetworkManager.Instance.mode;
        uiTextCurrentMode.text = string.Format("Current Mode : {0}", networkMode);

        switch(networkMode)
        {
        case NetworkManager.eMode.None:
            uiTextCurrentMode.color = Color.black;
            break;

        case NetworkManager.eMode.Server:
            uiTextCurrentMode.color = new Color(0.4f, 0.25f, 0.1f);
            break;

        case NetworkManager.eMode.Client:
            uiTextCurrentMode.color = new Color(0.25f, 0.4f, 0.1f);
            break;
        }
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

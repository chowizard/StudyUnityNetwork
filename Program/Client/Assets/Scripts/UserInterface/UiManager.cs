﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UiManager : MonoBehaviour
{
    private Dictionary<string, UiFrame> uiFrames = new Dictionary<string, UiFrame>();

    public static UiManager Instance
    {
        get
        {
            return GameManager.Singleton.uiManager;
        }
    }

    public void Clear()
    {
        foreach(var pair in uiFrames)
        {
            UiFrame uiFrame = pair.Value;
            if(uiFrame == null)
                continue;

            Destroy(uiFrame);
            uiFrame = null;
        }
        uiFrames.Clear();
    }

    public void AddUiFrame(UiFrame uiFrame)
    {
        uiFrames.Add(uiFrame.name, uiFrame);
    }

    public void RemoveUiFrame(UiFrame uiFrame)
    {
        if(uiFrame == null)
            return;

        Object.Destroy(uiFrame.gameObject);
        uiFrames.Remove(uiFrame.name);
    }

    public UiFrame GetUiFrame(string name)
    {
        UiFrame data;
        return uiFrames.TryGetValue(name, out data) ? data : null;
    }

    private void SetActive(string name, bool active)
    {
        UiFrame uiFrame = GetUiFrame(name);
        SetActive(uiFrame, active);
    }

    private void SetActive(UiFrame uiFrame, bool active)
    {
        if(uiFrame == null)
            return;

        uiFrame.gameObject.SetActive(active);
    }

    public UiFrame LoadUiFrame(string name, bool activeOnLoad = true)
    {
        UiFrame uiFrame = GetUiFrame(name);
        if(uiFrame != null)
            return uiFrame;

        string path = "UserInterface/UiFrame/" + name;
        return LoadUiFrameFromFile(path, activeOnLoad);
    }

    public UiFrame LoadUiFrameFromFile(string path, bool activeOnLoad = true)
    {
        GameObject uiPrefab;
        if(!LoadUiFramePrefab(path, out uiPrefab))
            return null;

        UiFrame uiFrame = CreateUiFrame(uiPrefab, activeOnLoad);
        if(uiFrame != null)
            AddUiFrame(uiFrame);

        return uiFrame;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private bool LoadUiFramePrefab(string path, out GameObject prefab)
    {
        prefab = null;

        if(string.IsNullOrEmpty(path))
            return false;

        prefab = Resources.Load<GameObject>(path);

        return true;
    }

    private UiFrame CreateUiFrame(GameObject uiPrefab, bool activeOnLoad)
    {
        if(uiPrefab == null)
            return null;

        GameObject uiFrameObject = Object.Instantiate<GameObject>(uiPrefab);
        if(uiFrameObject == null)
            return null;

        UiFrame uiFrame = uiFrameObject.GetComponent<UiFrame>();
        if(uiFrame == null)
        {
            Destroy(uiFrameObject);
            return null;
        }

        uiFrame.name = uiPrefab.name;
        uiFrame.transform.SetParent(transform);

        uiFrame.transform.localPosition = uiPrefab.transform.localPosition;
        uiFrame.transform.localRotation = uiPrefab.transform.localRotation;
        uiFrame.transform.localScale = uiPrefab.transform.localScale;

        uiFrame.gameObject.SetActive(activeOnLoad);

        return uiFrame;
    }
}

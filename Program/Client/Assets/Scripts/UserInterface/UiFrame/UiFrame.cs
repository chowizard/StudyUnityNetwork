﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class UiFrame : MonoBehaviour
{
    public enum eAnchorType
    {
        Center = 0,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public RectTransform uiAnchorCenter;
    public RectTransform uiAnchorLeft;
    public RectTransform uiAnchorRight;
    public RectTransform uiAnchorTop;
    public RectTransform uiAnchorBottom;
    public RectTransform uiAnchorTopLeft;
    public RectTransform uiAnchorTopRight;
    public RectTransform uiAnchorBottomLeft;
    public RectTransform uiAnchorBottomRight;

    private Dictionary<eAnchorType, RectTransform> uiAnchors = new Dictionary<eAnchorType, RectTransform>();
    private Dictionary<string, UiSet> uiSets = new Dictionary<string, UiSet>();

    public void Clear()
    {
        foreach(var pair in uiSets)
        {
            UiSet uiSet = pair.Value;
            if(uiSet == null)
                continue;

            Destroy(uiSet);
            uiSet = null;
        }
        uiSets.Clear();
    }

    public void AddUiSet(UiSet uiSet)
    {
        uiSets.Add(uiSet.name, uiSet);
    }

    public void RemoveUiSet(UiSet uiSet)
    {
        if(uiSet == null)
            return;

        Object.Destroy(uiSet.gameObject);
        uiSets.Remove(uiSet.name);
    }

    public UiSet GetUiSet(string name)
    {
        UiSet data;
        return uiSets.TryGetValue(name, out data) ? data : null;
    }

    private void SetActive(string name, bool active)
    {
        UiSet uiSet = GetUiSet(name);
        SetActive(uiSet, active);
    }

    private void SetActive(UiSet uiSet, bool active)
    {
        if(uiSet == null)
            return;

        uiSet.gameObject.SetActive(active);
    }

    public UiSet LoadUiSetFromFile(string path, bool activeOnLoad = true)
    {
        GameObject uiPrefab;
        if(!LoadUiSetPrefab(path, out uiPrefab))
            return null;

        UiSet uiSet = CreateUiSet(uiPrefab, activeOnLoad);
        if(uiSet != null)
            AddUiSet(uiSet);

        return uiSet;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        RegisterAnchorPoint(eAnchorType.Center);
        RegisterAnchorPoint(eAnchorType.Left);
        RegisterAnchorPoint(eAnchorType.Right);
        RegisterAnchorPoint(eAnchorType.Top);
        RegisterAnchorPoint(eAnchorType.Bottom);
        RegisterAnchorPoint(eAnchorType.TopLeft);
        RegisterAnchorPoint(eAnchorType.TopRight);
        RegisterAnchorPoint(eAnchorType.BottomLeft);
        RegisterAnchorPoint(eAnchorType.BottomRight);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    private RectTransform RegisterAnchorPoint(eAnchorType type)
    {
        Transform findTransform = transform.Find(type.ToString());
        Debug.Assert(findTransform != null);
        if(findTransform == null)
            return null;

        RectTransform rectTransform = findTransform.GetComponent<RectTransform>();
        Debug.Assert(rectTransform != null);
        if(rectTransform == null)
            return null;

        uiAnchors[type] = rectTransform;

        return rectTransform;
    }

    private bool LoadUiSetPrefab(string path, out GameObject prefab)
    {
        prefab = null;

        if(string.IsNullOrEmpty(path))
            return false;

        prefab = Resources.Load<GameObject>(path);

        return true;
    }

    private UiSet CreateUiSet(GameObject uiPrefab, bool activeOnLoad)
    {
        if(uiPrefab == null)
            return null;

        GameObject uiSetObject = Object.Instantiate<GameObject>(uiPrefab);
        if(uiSetObject == null)
            return null;

        UiSet uiSet = uiSetObject.GetComponent<UiSet>();
        if(uiSet == null)
        {
            Destroy(uiSetObject);
            return null;
        }

        Transform anchorTransform = uiAnchors[uiSet.anchorType].transform;
        if(anchorTransform != null)
            uiSet.transform.parent = anchorTransform.GetComponent<RectTransform>();
        else
            uiSet.transform.parent = transform;

        uiSet.name = uiPrefab.name;

        uiSet.transform.localPosition = uiPrefab.transform.localPosition;
        uiSet.transform.localRotation = uiPrefab.transform.localRotation;
        uiSet.transform.localScale = uiPrefab.transform.localScale;

        uiSet.gameObject.SetActive(activeOnLoad);

        return uiSet;
    }
}
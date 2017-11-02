using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UiManager : MonoBehaviour
{
    public Canvas canvas;

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
    private void Start()
    {
        LoadUiSetFromFile("UserInterface/UiSet/ControlPanel");
        LoadUiSetFromFile("UserInterface/UiSet/InformationWindow");
        LoadUiSetFromFile("UserInterface/UiSet/LogWindow");
    }

    // Update is called once per frame
    private void Update()
    {

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

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        RectTransform prefabRectTransform = uiPrefab.GetComponent<RectTransform>();
        RectTransform targetRectTransform = uiSet.GetComponent<RectTransform>();

        if(string.IsNullOrEmpty(uiSet.targetAnchor))
        {
            targetRectTransform.parent = canvasRectTransform;
        }
        else
        {
            Transform anchorTransform = canvas.transform.Find(uiSet.targetAnchor);
            if(anchorTransform != null)
                targetRectTransform.parent = anchorTransform.GetComponent<RectTransform>();
            else
                targetRectTransform.parent = canvasRectTransform;
        }

        uiSet.name = uiPrefab.name;

        targetRectTransform.localPosition = prefabRectTransform.localPosition;
        targetRectTransform.localRotation = prefabRectTransform.localRotation;
        targetRectTransform.localScale = prefabRectTransform.localScale;

        uiSet.gameObject.SetActive(activeOnLoad);

        return uiSet;
    }
}

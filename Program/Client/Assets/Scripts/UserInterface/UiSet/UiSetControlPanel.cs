using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiSetControlPanel : UiSet
{
    public Button uiButtonStartByServer;
    public Button uiButtonStartByClient;
    public Button uiButtonTerminate;

    public void OnClickStartByServer()
    {
        if(NetworkManager.Instance.isAtStartup == true)
            NetworkManager.Instance.StartByServer();
    }

    public void OnClickStartByClient()
    {
        if(NetworkManager.Instance.isAtStartup == true)
            NetworkManager.Instance.StartByClient();
    }

    public void OnClickTerminate()
    {
        if(NetworkManager.Instance.isAtStartup == false)
            NetworkManager.Instance.Terminate();
    }

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(uiButtonStartByServer != null);
        Debug.Assert(uiButtonStartByClient != null);
        Debug.Assert(uiButtonTerminate != null);

        if(NetworkManager.Instance.isAtStartup == true)
            SetupAtStart();
        else
            SetupAtProceed();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckUpdateVisualize();
    }

    private void CheckUpdateVisualize()
    {
        if(NetworkManager.Instance.isAtStartup == true)
            SetupAtStart();
        else
            SetupAtProceed();
    }

    private void SetupAtStart()
    {
        if(NetworkManager.Instance.isAtStartup == false)
            return;

        if(uiButtonStartByServer.gameObject.activeSelf == false)
            uiButtonStartByServer.gameObject.SetActive(true);

        if(uiButtonStartByClient.gameObject.activeSelf == false)
            uiButtonStartByClient.gameObject.SetActive(true);

        if(uiButtonTerminate.gameObject.activeSelf == true)
            uiButtonTerminate.gameObject.SetActive(false);
    }

    private void SetupAtProceed()
    {
        if(NetworkManager.Instance.isAtStartup == true)
            return;

        if(uiButtonStartByServer.gameObject.activeSelf == true)
            uiButtonStartByServer.gameObject.SetActive(false);

        if(uiButtonStartByClient.gameObject.activeSelf == true)
            uiButtonStartByClient.gameObject.SetActive(false);

        if(uiButtonTerminate.gameObject.activeSelf == false)
            uiButtonTerminate.gameObject.SetActive(true);
    }
}

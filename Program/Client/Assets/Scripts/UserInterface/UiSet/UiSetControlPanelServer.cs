using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiSetControlPanelServer : UiSet
{
    public Button uiButtonTerminate;
    public Button uiButtonAddNpc;
    public Slider uiSliderAddNpcUnitCount;
    public Text uiTextAddNpcUnitCount;

    public int addNpcUnitMinimum = 1;
    public int addNpcUnitMaximum = 10;
    private int currentAddNpcUnitCount;

    public void OnClickTerminate()
    {
        NetworkManager.Instance.Terminate();
        GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Intro);
    }

    public void OnClickAddNpc()
    {
        if(GameSceneManager.Instance.currentScene == null)
            return;

        if(GameSceneManager.Instance.currentScene.sceneType != GameScene.eSceneType.GamePlay)
            return;

        if(NetworkManager.Instance.IsReadyByServer == false)
            return;

        SceneGamePlay scene = GameSceneManager.Instance.currentScene as SceneGamePlay;
        Debug.Assert(scene != null);

        scene.SpawnNonPlayerCharacters(currentAddNpcUnitCount);
    }

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(uiButtonTerminate != null);
        Debug.Assert(uiButtonAddNpc != null);
        Debug.Assert(uiSliderAddNpcUnitCount != null);
        Debug.Assert(uiTextAddNpcUnitCount != null);

        uiTextAddNpcUnitCount.text = string.Format("Add NPC Unit Count : {0}", currentAddNpcUnitCount);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateAddNpcUnitCount();
    }

    private void UpdateAddNpcUnitCount()
    {
        if(uiSliderAddNpcUnitCount.minValue != addNpcUnitMinimum)
            uiSliderAddNpcUnitCount.minValue = addNpcUnitMinimum;

        if(uiSliderAddNpcUnitCount.maxValue != addNpcUnitMaximum)
            uiSliderAddNpcUnitCount.maxValue = addNpcUnitMaximum;

        if(uiSliderAddNpcUnitCount.value != currentAddNpcUnitCount)
        {
            currentAddNpcUnitCount = (int)uiSliderAddNpcUnitCount.value;
            uiTextAddNpcUnitCount.text = string.Format("Add NPC Unit Count : {0}", currentAddNpcUnitCount);
        }
    }
}

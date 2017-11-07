using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

public class CommandLineController : MonoBehaviour
{
    public static class Keyword
    {
        public const string AutoStartMode = "-autoStartMode";
    }

    public string[] arguments;

    private bool isAtStartup = true;
    private int currentIndex = -1;

    /* 서버로서 애플리케이션을 시작한다. */
    public static void StartAsServer()
    {
    }

    private void Start()
    {
        arguments = System.Environment.GetCommandLineArgs();

        StringBuilder builder = new StringBuilder("[Arguments] : \n");
        if(arguments != null)
        {
            foreach(string argument in arguments)
                builder.AppendLine(argument);
        }
        Debug.Log(builder.ToString());
    }

    private void Update()
    {
        if(isAtStartup)
        {
            AnalyzeCommand();
            isAtStartup = false;
        }
    }

    private void AnalyzeCommand()
    {
        if(arguments == null)
            return;

        for(int index = 0; index < arguments.Length; ++index)
        {
            string argument = arguments[index];
            currentIndex = index;

            if(string.Compare(argument, Keyword.AutoStartMode, true) == 0)
                AnalyzeCommandAutoStartMode(index);
        }
    }

    private void AnalyzeCommandAutoStartMode(int argumentIndex)
    {
        if(arguments.Length < argumentIndex + 2)
        {
            string logText = string.Format("Usage : {0}  [Mode Type]", Keyword.AutoStartMode);
            Debug.LogError(logText);

            return;
        }

        string argumentValue = arguments[argumentIndex + 1];

        if(string.Compare(argumentValue, "server") == 0)
        {
            NetworkManager.Instance.StartByServer();

            if(NetworkManager.Instance.isAtStartup == false)
            {
                GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
                GameManager.Singleton.SpawnNonPlayerCharacters();
            }
        }

        if(string.Compare(argumentValue, "client") == 0)
        {
            NetworkManager.Instance.StartByClient();

            if(NetworkManager.Instance.isAtStartup == false)
                GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
        }
    }
}

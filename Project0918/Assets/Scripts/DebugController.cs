/**
* @file DebugController.cs
* @author Ryder Claus
* @brief Opens up a command window for cheats
*/

using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance { get; private set; }

    bool showConsole;
    bool helpConsole;

    public bool noclip = false;
    public bool godmode = false;

    string input;
    string response = "test";

    public static DebugCommand HELP;
    public static DebugCommand QUIT;

    
    public List<object> commandList;

    private void DebugConsole()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    Vector2 scroll;
    public void Help(ref float y)
    {
        if (helpConsole)
        {
            GUI.Box(new Rect(0, y, UnityEngine.Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, UnityEngine.Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, UnityEngine.Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 100;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Write all of the debug command implementation in here where possible
        HELP = new DebugCommand("help", "display all commands", "help", () =>
        {
            helpConsole = !helpConsole;
        });
        QUIT = new DebugCommand("quit", "quit game", "quit", () =>
        {
            Application.Quit();
        });

        commandList = new List<object>
        {
            HELP,
            QUIT
        };

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
            DebugConsole();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            OnReturn();
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0;

        Help(ref y);

        //Response box
        if (response != "")
        {
            GUI.Box(new Rect(0, y, UnityEngine.Screen.width, 30), "");
            GUI.backgroundColor = Color.gray;

            Rect labelRect = new Rect(5, y, UnityEngine.Screen.width - 30 - 100, 20);
            GUI.Label(labelRect, response);

            y += 30;
        }

        //Input box
        GUI.SetNextControlName("ConsoleInput");
        GUI.Box(new Rect(0, y, UnityEngine.Screen.width, 30), "");
        GUI.backgroundColor = Color.gray;
        input = GUI.TextField(new Rect(10f, y + 5f, UnityEngine.Screen.width - 20f, 20f), input);

        // Detect Enter key if pressed
        if (Event.current != null && Event.current.type == EventType.KeyDown)
        {
            // Some platforms give character '\n' for Enter, others '\r'. Check both.
            if (Event.current.character == '\n' || Event.current.character == '\r'
                || Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
            {
                if (GUI.GetNameOfFocusedControl() == "ConsoleInput" ||
                    GUIUtility.keyboardControl != 0) // second check as fallback
                {
                    // Send input and unfocus text field
                    OnReturn();
                    GUI.FocusControl(null);

                    // prevents newline being inserted
                    Event.current.Use();         // consume event so it doesn't insert text
                }
            }
        }
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase debugCommandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(debugCommandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    // Cast to command and invoke
                    (commandList[i] as DebugCommand).Invoke();
                    response = "Command success - " + debugCommandBase.commandId;
                }

                else if (properties.Length < 2)
                {
                    response = "Improper command signature";
                }

                else if (commandList[i] as DebugCommand<uint> != null)
                {
                    if (uint.TryParse(properties[1], out uint value))
                    {
                        (commandList[i] as DebugCommand<uint>).Invoke(value);

                        response = "Command success - " + debugCommandBase.commandId + " " + value.ToString();
                    }
                    else response = "Invalid uint signature";
                }

                else if (commandList[i] as DebugCommand<float> != null)
                {
                    if (float.TryParse(properties[1], out float value))
                    {
                        (commandList[i] as DebugCommand<float>).Invoke(value);

                        response = "Command success - " + debugCommandBase.commandId + " " + value.ToString();

                    }
                    else response = "Invalid float signature";
                }

                else if (commandList[i] as DebugCommand<string> != null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);

                    response = "Command success - " + debugCommandBase.commandId + " " + properties[1];
                }

                else if (commandList[i] as DebugCommand<(int, int)> != null)
                {
                    if (int.TryParse(properties[1], out int value) && int.TryParse(properties[2], out int value2))
                    {
                        (commandList[i] as DebugCommand<(int, int)>).Invoke((value, value2));

                        response = "Command success - " + debugCommandBase.commandId + " " + value.ToString() + " " + value2.ToString();
                    }
                    else response = "Invalid int int signature";
                }
            }
        }
    }
}

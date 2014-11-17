using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.components
{
    /// <summary>
    ///     A console to display Unity's debug logs in-game.
    /// </summary>
    public class Console : MonoBehaviour
    {
        private const int margin = 20;

		public bool Show {
			get { return show; }
			set { show = value; }
		}

        private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            {LogType.Assert, Color.white},
            {LogType.Error, Color.red},
            {LogType.Exception, Color.red},
            {LogType.Log, Color.white},
            {LogType.Warning, Color.yellow},
        };

        private readonly GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        private readonly GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

        private readonly List<Log> logs = new List<Log>();
        private readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
        private bool collapse;
        private Vector2 scrollPosition;
        private bool show;

        /// <summary>
        ///     The hotkey to show and hide the console window.
        /// </summary>
        public KeyCode toggleKey = KeyCode.BackQuote;

        private Rect windowRect = new Rect(margin, margin, Screen.width - (margin*2), Screen.height - (margin*2));

        private void OnEnable()
        {
            Application.RegisterLogCallback(HandleLog);
        }

        private void OnDisable()
        {
            Application.RegisterLogCallback(null);
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                show = !show;
			}
        }

        private void OnGUI()
        {
            if (!show)
            {
                return;
            }

            windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
        }

        /// <summary>
        ///     A window that displayss the recorded logs.
        /// </summary>
        /// <param name="windowID">Window ID.</param>
        private void ConsoleWindow(int windowID)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // Iterate through the recorded logs.
            for (int i = 0; i < logs.Count; i++)
            {
                Log log = logs[i];

                // Combine identical messages if collapse option is chosen.
                if (collapse)
                {
                    bool messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

                    if (messageSameAsPrevious)
                    {
                        continue;
                    }
                }

                GUI.contentColor = logTypeColors[log.type];
                GUILayout.Label(log.message);
            }

            GUILayout.EndScrollView();

            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(clearLabel))
            {
                logs.Clear();
            }

            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();

            // Allow the window to be dragged by its title bar.
            GUI.DragWindow(titleBarRect);
        }

        /// <summary>
        ///     Records a log from the log callback.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stackTrace">Trace of where the message came from.</param>
        /// <param name="type">Type of message (error, exception, warning, assert).</param>
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            logs.Add(new Log
            {
                message = message,
                stackTrace = stackTrace,
                type = type,
            });
        }

        private struct Log
        {
            public string message;
            public string stackTrace;
            public LogType type;
        }
    }
}
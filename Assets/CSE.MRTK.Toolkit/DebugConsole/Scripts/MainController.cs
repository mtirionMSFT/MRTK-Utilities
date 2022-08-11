using System.IO;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace CSE.MRTK.Toolkit.DebugConsole
{
    /// <summary>
    /// This is the controller for the DebugConsole prefab.
    /// Here we catch debug messages and pass them along the
    /// rest of the prefab through events.
    /// </summary>
    public class MainController : MonoBehaviour
    {
        private const string LOG_FILENAME = "log.txt";

        [SerializeField]
        [Tooltip("Indicate if you want to catch all logs, or only when the prefab is enabled.")]
        private bool _onlyLogWhenEnabled;

        [SerializeField]
        [Tooltip("Indicate if you want to add the stack trace on errors.")]
        private bool _showStackTraceOnError;

        [SerializeField]
        [Tooltip("Provide the filename of the log.")]
        private string _logFileName = LOG_FILENAME;

        /// <summary>
        /// Event fired when we want to clear the log.
        /// </summary>
        public delegate void ClearLog();
        public event ClearLog OnClearLog;

        /// <summary>
        /// Event fired when message needs to be added to the UI.
        /// </summary>
        /// <param name="message">Message.</param>
        public delegate void Message(string message);
        public event Message OnMessage;

        /// <summary>
        /// Gets the settings manager.
        /// </summary>
        private SettingsManager _manager;
        private UIController _uiController;

        private string _logPath;

        /// <summary>
        /// Start of the game object.
        /// NOTE: please make sure that the main DebugConsole game object
        /// is always ENABLED in the hierarchy. The DebugConsolePanel is the
        /// actual UI and we control that one from here.
        /// </summary>
        private void Start()
        {
            _manager = GetComponentInChildren<SettingsManager>();
            _manager.Initialize(_onlyLogWhenEnabled);

            _uiController = GetComponentInChildren<UIController>(true);

            if (_manager.Settings.ShowAtStartup)
            {
                _uiController.gameObject.SetActive(true);
            }

#if WINDOWS_UWP
            _logPath = ApplicationData.Current.LocalCacheFolder.Path;
#else
            _logPath = Application.persistentDataPath;
#endif
            _logPath = Path.Combine(_logPath, _logFileName ?? LOG_FILENAME);
            if (File.Exists(_logPath))
            {
                // clean up from last run.
                File.Delete(_logPath);
            }

            // always want to catch logs
            Application.logMessageReceived += Application_logMessageReceived;
        }

        /// <summary>
        /// Capture logs from Unity and add them to the content.
        /// Only logs, errors and exceptions are added.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        private void Application_logMessageReceived(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Log || type == LogType.Error || type == LogType.Exception)
            {
                string msg = message;
                if (_showStackTraceOnError && (type == LogType.Exception || type == LogType.Error))
                {
                    msg += $"\n{stackTrace}";
                }
                WriteMessage(msg);
            }
        }

        /// <summary>
        /// Add the message to the console or overwrite contents with it.
        /// </summary>
        /// <param name="message">Message.</param>
        public void WriteMessage(string message)
        {
            OnMessage?.Invoke(message);
            if (_manager.Settings.SaveToFile &&
                (!_onlyLogWhenEnabled || _uiController.gameObject.activeSelf))
            {
                // only save if 1) configured to do so 2) only when enabled & ui active OR always log 
                File.AppendAllText(_logPath, $"{message}\n");
            }
        }

        /// <summary>
        /// Show the debug console. We'll load the messages from the log.
        /// </summary>
        public void ShowConsole()
        {
            _uiController.gameObject.SetActive(true);
            if (File.Exists(_logPath))
            {
                // if log exists, read it and set as starting text
                string log = File.ReadAllText(_logPath);
                OnMessage?.Invoke(log);
            }
        }

        /// <summary>
        /// Clear the log and update the UI.
        /// </summary>
        public void Clear()
        {
            if (File.Exists(_logPath))
            {
                File.Delete(_logPath);
            }
            OnClearLog?.Invoke();
        }
    }
}

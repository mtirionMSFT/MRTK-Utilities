using System.IO;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace CSE.MRTK.Toolkit.DebugConsole
{
    /// <summary>
    /// This is the controller for the DebugConsole prefab.
    /// </summary>
    public class UIController : MonoBehaviour
    {
        /// <summary>
        /// Gets the settings manager.
        /// </summary>
        private SettingsManager _manager;

        /// <summary>
        /// Closing the debug console.
        /// </summary>
        public void OnClose()
        {
            if (_manager != null)
            {
                // save to state that we're not active anymore.
                _manager.Settings.ShowAtStartup = false;
                _manager.SaveSettings();
            }
            this.gameObject.SetActive(false);
        }

        public void OnClear()
        {

        }

        /// <summary>
        /// Called when the prefab is enabled.
        /// </summary>
        private void OnEnable()
        {
            if (_manager == null)
            {
                _manager = GetComponentInParent<SettingsManager>();
                if (_manager == null)
                {
                    Debug.LogError($"DebugConsole MainController ERROR: SettingsManager not found!");
                    return;
                }
            }

            // save to state that we're active
            _manager.Settings.ShowAtStartup = true;
            _manager.SaveSettings();
        }
    }
}

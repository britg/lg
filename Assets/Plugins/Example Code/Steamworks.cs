using System;
using UnityEngine;
using ManagedSteam;
using ManagedSteam.Exceptions;
using ManagedSteam.CallbackStructures;
using ManagedSteam.SteamTypes;
using ManagedSteam.Utility;
using System.Runtime.InteropServices;

/// <summary>
/// A simple example of how to start the library in Unity. 
/// Make sure that you have the steam client running and have setup everything according to the 
/// Setup Instructions in the documentation, or the library will fail to load.
/// </summary>
public class Steamworks : MonoBehaviour
{
    private static Steamworks activeInstance;

    private string status;

    /// <summary>
    /// Use this property to access the Steamworks API
    /// </summary>
    public static Steam SteamInterface { get; private set; }

    private void Awake()
    {
        // Makes sure that only one instance of this object is in use at a time
        if (SteamInterface == null)
        {
            bool error = false;
            try
            {
                // Starts the library. This will, and can, only be done once.
                SteamInterface = Steam.Initialize();
            }
            catch (AlreadyLoadedException e)
            {
                status = "The native dll is already loaded, this should not happen if ReleaseManagedResources is used and Steam.Initialize() is only called once.";
                Debug.LogError(status, this);
                Debug.LogError(e.Message, this);
                error = true;
            }
            catch (SteamInitializeFailedException e)
            {
                status = "Could not initialize the native Steamworks API. This is usually caused by a missing steam_appid.txt file or if the Steam client is not running.";
                Debug.LogError(status, this);
                Debug.LogError(e.Message, this);
                error = true;
            }
            catch (SteamInterfaceInitializeFailedException e)
            {
                status = "Could not initialize the wanted versions of the Steamworks API. Make sure that you have the correct Steamworks SDK version. See the documentation for more info.";
                Debug.LogError(status, this);
                Debug.LogError(e.Message, this);
                error = true;
            }
            catch (DllNotFoundException e)
            {
                status = "Could not load a dll file. Make sure that the steam_api.dll/libsteam_api.dylib file is placed at the correct location. See the documentation for more info.";
                Debug.LogError(status, this);
                Debug.LogError(e.Message, this);
                error = true;
            }

            if (error)
            {
                SteamInterface = null;
            }
            else
            {
                status = "Steamworks initialized and ready to use.";
                
                // Prevent destruction of this object
                GameObject.DontDestroyOnLoad(this);
                activeInstance = this;

                // An event is used to notify us about any exceptions thrown from native code.
                SteamInterface.ExceptionThrown += ExceptionThrown;

                // Listen to when the game overlay is shown or hidden
                SteamInterface.Friends.GameOverlayActivated += OverlayToggle;

                
            }
        }
        else
        {
            // Another Steamworks object is already created, destroy this one.
            Destroy(this);
        }
    }

    private void OverlayToggle(GameOverlayActivated value)
    {
        // This method is called when the game overlay is hidden or shown
        // NOTE: The overlay may not work when a game is run in the Unity editor
        // Build the game and "publish" it to a local content server and then start the game via the
        // steam client to make the overlay work.

        if (value.Active)
        {
            UnityEngine.Debug.Log("Overlay shown");
        }
        else
        {
            UnityEngine.Debug.Log("Overlay closed");
        }
    }

    private void ExceptionThrown(Exception e)
    {
        // This method is called when an exception have been thrown from native code.
        // We print the exception so we can see what went wrong.

        UnityEngine.Debug.LogError(e.GetType().Name + ": " + e.Message + "\n" + e.StackTrace);
    }

    private void Update()
    {
        if (SteamInterface != null)
        {
            // Makes sure that callbacks are sent.
            // Make sure that you call this from some other place if you use 'Time.timeScale = 0' 
            // to pause the game.
            SteamInterface.Update();
        }

        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    private void OnGUI()
    {
        GUILayout.Label(status);
        GUILayout.Label(User.Path);

        // NOTE!
        // Overlays might not work in the Unity editor, see the documentation for more information
        if (GUILayout.Button("Show overlay"))
        {
            // Will show the game overlay and show the Friends dialog. 
            SteamInterface.Friends.ActivateGameOverlay(OverlayDialog.Friends);
        }
        if (GUILayout.Button("Show overlay (webpage)"))
        {
            // Will show the game overlay and open a web page.
            SteamInterface.Friends.ActivateGameOverlayToWebPage("http://ludosity.com");
        }

    }

    private void OnDestroy()
    {
        // Only cleanup if the object being destroyed "owns" the Steam instance.
        if (activeInstance == this)
        {
            activeInstance = null;
            Cleanup();
        }
    }

    private void OnApplicationQuit()
    {
        // Always cleanup when shutting down
        Cleanup();
    }

    private void Cleanup()
    {
        if (SteamInterface != null)
        {
            if (Application.isEditor)
            {
                // Only release managed handles if we run from inside the editor. This enables us 
                // to use the library again without restarting the editor.
                SteamInterface.ReleaseManagedResources();
            }
            else
            {
                // We are running from a standalone build. Shutdown the library completely
                SteamInterface.Shutdown();
            }

            SteamInterface = null;
        }
    }

    

}

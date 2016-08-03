using UnityEngine;
using System;
using System.Runtime.InteropServices;

/** 
 * Workaround to place a game's window at a specified location on the desktop. 
 */

public class WindowManager : MonoBehaviour
{

    // Imports
    // ------------------------------------------------------------

    #if UNITY_STANDALONE_WIN &&! UNITY_EDITOR

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

    #endif


    // Static Methods
    // ------------------------------------------------------------

    /** Move the game window to a new location. */
    public static void SetPosition(int x, int y, int resX = 0, int resY = 0) 
    {
        #if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            var hWnd = GetForegroundWindow();
            Debug.Log(string.Format("WindowManager.MoveWindow({0}, {1}, {2}, {3}): hWnd = {4}", x, y, resX, resY, hWnd));
            var success = SetWindowPos(hWnd, 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
            Debug.Log(string.Format("WindowManager.MoveWindow({0}, {1}, {2}, {3}): Success: {4}", x, y, resX, resY, success));
        #endif
    }


    // Unity Methods
    // ------------------------------------------------------------

    /** Initialization. */
    private void Awake()
    {
        // Check if an explicit screen position is specified in configuration.
        var hasXPos = Configuration.Has("screen-position-x");
        var hasYPos = Configuration.Has("screen-position-y");
        if (!hasXPos && !hasYPos)
            return;

        // If so, move window to the desired position
        var x = Configuration.Get("screen-position-x", 0);
        var y = Configuration.Get("screen-position-y", 0);
        SetPosition(x, y);
    }

}

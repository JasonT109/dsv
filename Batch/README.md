# BATCH FILES FOR LAUNCHING DSV CLIENT/SERVER INSTANCES

This folder contains a set of example batch files that can be used to launch the Unity DSV interactive application in a variety of configurations.
Copy them into the folder containing dsv.exe in order to use them.


## UNITY COMMAND LINE ARGUMENTS

For a full rundown of Unity's built-in arguments, please refer to [https://docs.unity3d.com/Manual/CommandLineArguments.html](https://docs.unity3d.com/Manual/CommandLineArguments.html) (scroll down to the section entitled 'Unity Standalone Player command line arguments'.)

Some of the more useful Unity launch options:

### -show-screen-selector
Forces the screen selector dialog to be shown.

### -popupwindow
The window will be created as a a pop-up window (without a frame).

### screen-fullscreen
Overrides the default fullscreen state. This must be 0 or 1.

### -screen-width
Overrides the default screen width. This must be an integer from a supported resolution.

### -screen-height
Overrides the default screen height. This must be an integer from a supported resolution.

### -screen-quality
Overrides the default screen quality. Example usage would be: /path/to/myGame -screen-quality Beautiful


## DSV CONFIGURATION OPTIONS

The DSV client has a range of configuration options.  Please refer to the documentation in [Assets/StreamingAssets/README.md](Assets/StreamingAssets/README.md) and inspect [Assets/StreamingAssets/config.json](Assets/StreamingAssets/config.json) to get a full rundown of the supported options.

The most important commandline option is -id <configId>.  This selects which set of configuration options will be activated for the client instance being launched.

For example, 'START dsv.exe -id gliderMid' will launch an instance into the Glider screen set, and configure it as the middle glider screen.

This works by looking at the config.json file [in the StreamingAssets folder](../Assets/StreamingAssets/) and selecting the 'gliderMid' configuration group.  'gliderMid' has a parent group, 'glider', and inherits all configuration from that group.  A child's declarations take precedence over ones in the parent (same as inheritance and overriding in Java/C#).  'glider' in turn has a parent config group, 'default', which is just the baseline set of default configuration options.

If no configuration ID is specified at startup, the 'default' configuration group is used.




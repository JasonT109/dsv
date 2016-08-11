# DSV CONFIGURATION OPTIONS

The DSV application can be launched in a variety of configurations.  This capability is driven by a configuration file system.  The configuration data lives in [Assets/StreamingAssets/config.json](config.json).

In a standalone build, the configuration file will be in 'dsv_Data/StreamingAssets/config.json'.


## SELECTING A CONFIGURATION GROUP

To launch in a given configuration, use the -id <configId> commandline option.  This selects which set of configuration options will be activated for the client instance being launched.  For example, 'dsv.exe -id gliderMid' will launch into the Glider screen set, and identify itself as the middle screen.  See the [Batch folder](../../Batch) for some example batch files.

This works by looking at the config.json file (in [StreamingAssets/](.)) and selecting the 'gliderMid' configuration group.  'gliderMid' has a parent group, 'glider', and inherits all configuration from that group.  A child's declarations take precedence over ones in the parent (same as inheritance and overriding in Java/C#).  'glider' in turn has a parent config group, 'default', which is just the baseline set of default configuration options.

If no configuration ID is specified at startup, the 'default' configuration group is used.


## COMMANDLINE CONFIGURATION

You can also specify any configuration option by passing it in as a commandline parameter.  Commandline arguments always take precedence over the configuration file settings.


## CONFIGURATION REFERENCE

### save-folder
Specifies where Scene save files are located.

### auto-save-folder
Specifies where Playback autosave files are located.

### auto-save-format
Specifies a C# string formatting pattern to use when determining Playback autosave filenames.
For example, '{0}/Scene_{2:000}/{6}/{1}_{2:000}_{3:00}_{4:00}_{5:dd.MM.yyyy}_{5:HH.mm.ss.f}_{6}.json'.
- {0} is the auto-save-folder.
- {1} is the current vessel being simulated (e.g. Marco, Polo).
- {2} is the current scene.
- {3} is the current shot within the scene.
- {4} is the current take
- {5} is the current date/time, in UTC.
- {6} is a suffix such as 'Start' or 'Stop' (denotes autosaves taken at different points in playback.)

### auto-save-enabled
Whether to perform autosaving during usage of the Playback interface.

### network-role
Specifies that the instance should start up with a defined role in the network.
Valid values are "client" or "server".
If no value is specified, the instance will start up in the login screen.

### network-scene
Which screen set to start up in.  For example, "screen_subs" will launch the instance into the subs screen set.
Valid values are "screen_subs", "screen_gliders", or "screen_dcc".

### parent
Defines a parent configuration group to inherit from.  A child's declarations take precedence over ones in the parent (same as inheritance and overriding in Java/C#).  For example, 'glider' in has a parent config group 'default', which is just the baseline set of default configuration options.

### screen-mode
Specifies a screen mode to start up in.  Valid values are "16x9", "16x10", "21x9l", "21x9c" and "21x9r".
Works in conjunction with the 'screen-scaling' option.

### screen-scaling
Whether to apply scaling to warp the screen into a 16x9 output format.

### screen-initial
Which screen to start up the instance into.

For the "screen_subs" set, valid values are: "instruments", "thrusters", "navigation", "vitals", "lifesupport", "debug", "diagnostics", "sonar", "piloting", and "dome".

### screen-position-x
- Initial X position to place the window at.  Mainly useful for placing an instance on the correct screen when launching more than one instance per machine. 

### screen-position-y
- Initial Y position to place the window at.

### screen-width (Unity option)
- Initial width of the window.

### screen-height (Unity option)
- Initial height of the window.

### vessel-names
The current names of each vessel/sub. Specified as a JSON array.

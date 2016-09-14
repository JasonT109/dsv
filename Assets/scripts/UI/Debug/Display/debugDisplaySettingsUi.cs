using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class debugDisplaySettingsUi : MonoBehaviour
{

    /** DCC Configuration interface. */
    public debugDCCDisplayUi Dcc;

    /** Sub display settings. */
    public debugSubScreensUi SubSettings;

    /** Glider display settings. */
    public debugGliderScreensUi GliderSettings;


    /** Initialization. */
    private void Start()
    {
        // Activate the appropriate screen management interface.
        Dcc.gameObject.SetActive(false);
        SubSettings.gameObject.SetActive(false);
        GliderSettings.gameObject.SetActive(false);

        var scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case NetworkManagerCustom.DccScene:
                Dcc.gameObject.SetActive(true);
                break;
            case NetworkManagerCustom.BigSubScene:
                SubSettings.gameObject.SetActive(true);
                break;
            case NetworkManagerCustom.GliderScene:
                GliderSettings.gameObject.SetActive(true);
                break;
        }

    }
	
}

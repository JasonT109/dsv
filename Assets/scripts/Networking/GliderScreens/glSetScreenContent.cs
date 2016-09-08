using Meg.Networking;
using UnityEngine;

public class glSetScreenContent : MonoBehaviour
{
    /** set in inspector, relates to the screen matrix */
    public int screenMatrixID = 0;


    private void OnEnable()
        { UpdateScreenState(); }

    /** Update the local player's shared screen state to match this screen's matrix id. */
    private void UpdateScreenState()
    {
        if (!serverUtils.IsReady() || !glScreenManager.Instance)
            return;

        var state = glScreenManager.Instance.GetStateForMatrixId(screenMatrixID);
        var player = serverUtils.LocalPlayer;
        if (player && !Equals(player.ScreenState, state))
            serverUtils.PostScreenState(player.netId, state);
    }

    /** Update */
    void Update ()
    {
        /** If we have changed the current screen content update it with the GLIDER screen manager */
        if (!glScreenManager.Instance)
            return;

        glScreenManager.Instance.SetRightScreenID(screenMatrixID);
    }
}

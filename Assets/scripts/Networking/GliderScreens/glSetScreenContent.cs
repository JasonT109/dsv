using UnityEngine;

public class glSetScreenContent : MonoBehaviour
{
    /** set in inspector, relates to the screen matrix */
    public int screenMatrixID = 0;

    /** Update */
    void Update ()
    {
        /** If we have changed the current screen content update it with the GLIDER screen manager */
        if (glScreenManager.Instance)
        {
            glScreenManager.Instance.SetRightScreenID(screenMatrixID);
        }
    }
}

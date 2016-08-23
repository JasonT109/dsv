using UnityEngine;
using Meg.Networking;

/** Sets the player vessel on level start. */
public class setPlayerOnStart : MonoBehaviour
{
    public enum playerVessels
    {
        Marco,
        Polo,
        Byrd,
        Ernie
    }

    public playerVessels playerVessel = playerVessels.Marco;

	void Start ()
    {
        if (!serverUtils.IsServer())
            return;

	    switch (playerVessel)
        {
            case playerVessels.Marco:
                serverUtils.SetPlayerVessel(1);
                break;
            case playerVessels.Polo:
                serverUtils.SetPlayerVessel(2);
                break;
            case playerVessels.Byrd:
                serverUtils.SetPlayerVessel(3);
                break;
            case playerVessels.Ernie:
                serverUtils.SetPlayerVessel(4);
                break;
        }
	}
}

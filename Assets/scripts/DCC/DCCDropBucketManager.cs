using UnityEngine;

public class DCCDropBucketManager : MonoBehaviour
{
    public GameObject dropBucketGroup1;
    public GameObject dropBucketGroup2;

    private static bool HasScreen(screenData.Type type)
    {
        return DCCScreenData.GetStationHasScreen(DCCScreenData.StationId, type);
    }

    void Update ()
    {
        if (HasScreen(screenData.Type.DccScreen4))
        {
            dropBucketGroup1.SetActive(true);
            dropBucketGroup2.SetActive(false);
        }
        else
        {
            dropBucketGroup1.SetActive(false);
            dropBucketGroup2.SetActive(true);
        }
	}
}

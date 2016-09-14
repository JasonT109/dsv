using UnityEngine;

public class DCCDropBucket : MonoBehaviour
{
    public enum dropBucket
    {
        left,
        middle,
        right,
        none
    }

    public dropBucket bucket = dropBucket.left;
}


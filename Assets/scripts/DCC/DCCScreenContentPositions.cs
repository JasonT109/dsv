using UnityEngine;
using System.Collections;

public class DCCScreenContentPositions : MonoBehaviour {

    public enum positionID
    {
        topLeft,
        topRight,
        bottomLeft,
        bottomRight,
        middle,
        hidden
    }

    public Vector2[] screenPositions = new Vector2[6];
    public Vector2[] screenScales = new Vector2[6];

    public Vector2 GetScreenPosition(DCCScreenContentPositions.positionID id)
    {
        Vector2 position = Vector2.zero;

        switch (id)
            {
            case positionID.topLeft:
                position = screenPositions[0];
                break;
            case positionID.topRight:
                position = screenPositions[0];
                break;
            case positionID.bottomLeft:
                position = screenPositions[0];
                break;
            case positionID.bottomRight:
                position = screenPositions[0];
                break;
            case positionID.middle:
                position = screenPositions[0];
                break;
            case positionID.hidden:
                position = screenPositions[0];
                break;
        }

        return position;
    }

    public Vector2 GetScreenScale(DCCScreenContentPositions.positionID id)
    {
        Vector2 scale = Vector2.zero;

        switch (id)
        {
            case positionID.topLeft:
                scale = screenScales[0];
                break;
            case positionID.topRight:
                scale = screenScales[0];
                break;
            case positionID.bottomLeft:
                scale = screenScales[0];
                break;
            case positionID.bottomRight:
                scale = screenScales[0];
                break;
            case positionID.middle:
                scale = screenScales[0];
                break;
            case positionID.hidden:
                scale = screenScales[0];
                break;
        }

        return scale;
    }

    void Awake()
    {
        screenPositions[0] = new Vector2(-6f, 2.65f);
        screenPositions[1] = new Vector2(6f, 2.65f);
        screenPositions[2] = new Vector2(-6f, -2.65f);
        screenPositions[3] = new Vector2(6f, -2.65f);
        screenPositions[4] = new Vector2(0f, 0f);
        screenPositions[5] = new Vector2(0, -6f);

        screenScales[0] = new Vector2(95f, 53.4375f);
        screenScales[1] = new Vector2(95f, 53.4375f);
        screenScales[2] = new Vector2(95f, 53.4375f);
        screenScales[3] = new Vector2(95f, 53.4375f);
        screenScales[4] = new Vector2(190f, 106.875f);
        screenScales[5] = new Vector2(16, 9f);
    }
}

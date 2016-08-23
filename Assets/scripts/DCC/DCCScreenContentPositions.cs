using UnityEngine;
using System.Collections;

namespace Meg.DCC
{
    public class DCCScreenContentPositions : MonoBehaviour {

        public enum positionID
        {
            topLeft,
            topRight,
            bottomLeft,
            bottomRight,
            middle,
            hidden,
            screenPos1,
            screenPos2,
            screenPos3
        }

        public static Vector3[] screenPositions = 
        {
            new Vector3(-7.6f, 2.65f, 10f),     //0 topLeft
            new Vector3(2f, 2.65f, 10f),        //1 topRight
            new Vector3(-7.6f, -2.65f, 10f),    //2 bottomLeft
            new Vector3(2f, -2.65f, 10f),       //3 bottomRight
            new Vector3(-3.25f, 0f, 0f),        //4 middle
            new Vector3(0f, -6f, 10f),          //5 hidden
            new Vector3(-0.42f, 0.57f, 10f),    //5 screenPos1
            new Vector3(0.31f, 0.25f, 10f),     //5 screenPos2
            new Vector3(-0.5f, -0.37f, 10f)     //5 screenPos3
        };

        public static Vector2[] screenScales = 
        {
            new Vector2(95f, 53.4375f),         //0 topLeft
            new Vector2(95f, 53.4375f),         //1 topRight
            new Vector2(95f, 53.4375f),         //2 bottomLeft
            new Vector2(95f, 53.4375f),         //3 bottomRight
            new Vector2(152f, 85.5f),           //4 middle
            new Vector2(190f, 106.875f),        //5 hidden
            new Vector2(161.5f, 90.84375f),     //5 screenPos1
            new Vector2(161.5f, 90.84375f),     //5 screenPos2
            new Vector2(161.5f, 90.84375f)      //5 screenPos3
        };

        public static Vector3 GetScreenPosition(DCCScreenContentPositions.positionID id)
        {
            Vector3 position = Vector3.zero;

            switch (id)
                {
                case positionID.topLeft:
                    position = screenPositions[0];
                    break;
                case positionID.topRight:
                    position = screenPositions[1];
                    break;
                case positionID.bottomLeft:
                    position = screenPositions[2];
                    break;
                case positionID.bottomRight:
                    position = screenPositions[3];
                    break;
                case positionID.middle:
                    position = screenPositions[4];
                    break;
                case positionID.hidden:
                    position = screenPositions[5];
                    break;
                case positionID.screenPos1:
                    position = screenPositions[6];
                    break;
                case positionID.screenPos2:
                    position = screenPositions[7];
                    break;
                case positionID.screenPos3:
                    position = screenPositions[8];
                    break;
            }

            return position;
        }

        public static Vector2 GetScreenScale(DCCScreenContentPositions.positionID id)
        {
            Vector2 scale = Vector2.zero;

            switch (id)
            {
                case positionID.topLeft:
                    scale = screenScales[0];
                    break;
                case positionID.topRight:
                    scale = screenScales[1];
                    break;
                case positionID.bottomLeft:
                    scale = screenScales[2];
                    break;
                case positionID.bottomRight:
                    scale = screenScales[3];
                    break;
                case positionID.middle:
                    scale = screenScales[4];
                    break;
                case positionID.hidden:
                    scale = screenScales[5];
                    break;
                case positionID.screenPos1:
                    scale = screenScales[6];
                    break;
                case positionID.screenPos2:
                    scale = screenScales[7];
                    break;
                case positionID.screenPos3:
                    scale = screenScales[8];
                    break;
            }

            return scale;
        }

        public static void SetScreenPos(Transform window, DCCScreenContentPositions.positionID id)
        {
            Vector3 position = Vector3.zero;

            switch (id)
            {
                case positionID.topLeft:
                    position = screenPositions[0];
                    break;
                case positionID.topRight:
                    position = screenPositions[1];
                    break;
                case positionID.bottomLeft:
                    position = screenPositions[2];
                    break;
                case positionID.bottomRight:
                    position = screenPositions[3];
                    break;
                case positionID.middle:
                    position = screenPositions[4];
                    break;
                case positionID.hidden:
                    position = screenPositions[5];
                    break;
                case positionID.screenPos1:
                    position = screenPositions[6];
                    break;
                case positionID.screenPos2:
                    position = screenPositions[7];
                    break;
                case positionID.screenPos3:
                    position = screenPositions[8];
                    break;
            }

            window.localPosition = position;
        }

        public static void SetScreenScale(Transform window, DCCScreenContentPositions.positionID id)
        {
            Vector2 scale = Vector2.one;

            switch (id)
            {
                case positionID.topLeft:
                    scale = screenScales[0];
                    break;
                case positionID.topRight:
                    scale = screenScales[1];
                    break;
                case positionID.bottomLeft:
                    scale = screenScales[2];
                    break;
                case positionID.bottomRight:
                    scale = screenScales[3];
                    break;
                case positionID.middle:
                    scale = screenScales[4];
                    break;
                case positionID.hidden:
                    scale = screenScales[5];
                    break;
                case positionID.screenPos1:
                    scale = screenScales[6];
                    break;
                case positionID.screenPos2:
                    scale = screenScales[7];
                    break;
                case positionID.screenPos3:
                    scale = screenScales[8];
                    break;
            }

            window.gameObject.GetComponent<graphicsDCCWindowSize>().windowWidth = scale.x;
            window.gameObject.GetComponent<graphicsDCCWindowSize>().windowHeight = scale.y;
        }
    }
}


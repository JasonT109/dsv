using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;
using Meg.Networking;

public class graphicsPinchZoom : MonoBehaviour {

    public float scrollSpeed = 1.0f;
    public Transform[] children;
    public Vector2[] childPos;
    public Vector2 imageSize = new Vector2(10,10);
    public bool deactivateChildrenOnScroll = true;
    public float updateTick = 0.2f;

    private float nextUpdateTime;
    private float scaleDelta = 1f;
    private bool pressed = false;
    private TouchHit previousHit;
    private TouchHit currentHit;
    private Renderer r;
    private Material m;
    private bool multiTouch = false;
    private bool updatePositions = false;

    Vector2 tilingFactor;
    Vector2 newOffset;
    Vector2 touchDelta;
    Vector2 scaleFactor;

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed  += pressedHandler;
        GetComponent<ReleaseGesture>().Released += releaseHandler;
        GetComponent<TransformGesture>().Transformed += transformHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= pressedHandler;
        GetComponent<ReleaseGesture>().Released -= releaseHandler;
        GetComponent<TransformGesture>().Transformed -= transformHandler;
    }

    private void pressedHandler(object sender, EventArgs e)
    {
        var gesture = sender as PressGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);
    }

    private void transformHandler(object sender, EventArgs e)
    {
        var gesture = sender as TransformGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //make sure we are hitting this gameObject
        if (hit.Transform != gameObject.transform)
        {
            return;
        }

        //set the current hit data so we can read the position the touch is at
        currentHit = hit;

        //set pressed to true so the update will transform the texture co-ords
        pressed = true;

        //if we have more than one touch
        if (gesture.NumTouches > 1)
        {
            multiTouch = true;
            //get scale of gesture
            scaleDelta = gesture.DeltaScale;
        }
        else
        {
            scaleDelta = 1f;
            multiTouch = false;
        }

        if (deactivateChildrenOnScroll)
        {
            //set all children buttons to in active
            foreach (GameObject b in gameObject.GetComponent<buttonGroup>().buttons)
            {
                if (b.GetComponent<buttonControl>().active)
                {
                    b.GetComponent<buttonControl>().toggleButton(b);
                }
            }
        }
    }

    private void releaseHandler(object sender, EventArgs e)
    {
        var gesture = sender as ReleaseGesture;
        TouchHit hit;
        gesture.GetTargetHitResult(out hit);

        //reset previous hit
        previousHit = new TouchHit();
        scaleDelta = 1f;

        //stop the update loop from transforming the texture
        pressed = false;

    }

    void Start ()
    {
        r = gameObject.GetComponent<Renderer>();
        m = r.material;

        int childCount = transform.childCount;
        children = new Transform[childCount];
        childPos = new Vector2[childCount];
        for (int i = 0; i < childCount; i++)
        {
            children[i] = gameObject.transform.GetChild(i);
            childPos[i] = new Vector2(children[i].localPosition.x, children[i].localPosition.y);
        }

        nextUpdateTime = Time.time + updateTick;
	}
	
	void Update ()
    {
        if (Time.time > nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateTick;
            for (int i = 0; i < children.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        childPos[i] = new Vector2(serverUtils.GetServerData("v1posX"), serverUtils.GetServerData("v1posY"));
                        break;
                    case 1:
                        childPos[i] = new Vector2(serverUtils.GetServerData("v2posX"), serverUtils.GetServerData("v2posY"));
                        break;
                    case 2:
                        childPos[i] = new Vector2(serverUtils.GetServerData("v3posX"), serverUtils.GetServerData("v3posY"));
                        break;
                    case 3:
                        childPos[i] = new Vector2(serverUtils.GetServerData("v4posX"), serverUtils.GetServerData("v4posY"));
                        break;
                }
            }
        }

        if (pressed)
        {
            //get tiling factor
            tilingFactor = m.GetTextureScale("_MainTex");

            //create new offset for texture co-ords
            newOffset = m.GetTextureOffset("_MainTex");

            //get difference from this touch position and last
            touchDelta = new Vector2(previousHit.Point.x - currentHit.Point.x, previousHit.Point.y - currentHit.Point.y);

            //only get scroll input if have a valid delta and we are not scrolling
            if (previousHit.Point != Vector3.zero && !multiTouch)
            {
                //create lerp between current offset and new offset
                newOffset = Vector2.Lerp(newOffset, newOffset + touchDelta, (Time.deltaTime * scrollSpeed) * tilingFactor.x);
            }

            //clamp the new offset
            newOffset = new Vector2(Mathf.Clamp(newOffset.x, 0f, 1 - tilingFactor.x), Mathf.Clamp(newOffset.y, 0f, 1 - tilingFactor.y));

            //apply new co-ords
            m.SetTextureOffset("_MainTex", newOffset);

            //apply scale delta
            scaleFactor = m.GetTextureScale("_MainTex");
            scaleFactor /= scaleDelta;

            //clamp the scale
            scaleFactor = new Vector2( Mathf.Clamp( scaleFactor.x, 0.1f, 1.0f ), Mathf.Clamp( scaleFactor.y, 0.1f, 1.0f ));
            
            //set the scale
            m.SetTextureScale("_MainTex", scaleFactor);

            //update previous hit so we can get a delta on next frame
            previousHit = currentHit;
        }

        //transform any children
        for (int i = 0; i < children.Length; i++)
        {
            //position is inverse scale + offset
            scaleFactor = m.GetTextureScale("_MainTex");

            //get the inverse scale
            scaleFactor = new Vector2(1 / scaleFactor.x, 1 / scaleFactor.y);

            //get childs original position
            Vector2 newPos = childPos[i];

            //convert it to UV space //= (X-A)/(B-A) * (D-C) + C
            newPos.x = (newPos.x - -imageSize.x) / (imageSize.x - -imageSize.x);
            newPos.y = (newPos.y - -imageSize.y) / (imageSize.y - -imageSize.y);

            //scale the UV position
            newPos.x *= scaleFactor.x;
            newPos.y *= scaleFactor.y;

            //get the offset
            Vector2 offset = m.GetTextureOffset("_MainTex");

            //subtract the offset
            newPos.x -= offset.x * scaleFactor.x;
            newPos.y -= offset.y * scaleFactor.y;

            //convert back to object space
            newPos.x = (newPos.x - 0) / (1 - 0) * (imageSize.x - -imageSize.x) + -imageSize.x;
            newPos.y = (newPos.y - 0) / (1 - 0) * (imageSize.y - -imageSize.y) + -imageSize.y;

            //apply the new offset
            children[i].localPosition = new Vector3(Mathf.Clamp(newPos.x, -imageSize.x, imageSize.x), Mathf.Clamp(newPos.y, -imageSize.x, imageSize.x), children[i].localPosition.z);

            //0 no direction, 1 left, 2 upleft, 3 up, 4 up right, 5 right, 6 down right, 7 down, 8 down left
            int mapIconDirection = 0;

            //check to see if child is at edge of the map
            if (children[i].localPosition.x == -imageSize.x && children[i].localPosition.y == -imageSize.y)
            {
                mapIconDirection = 8;
            }
            else if (children[i].localPosition.x == -imageSize.x && children[i].localPosition.y == imageSize.y)
            {
                mapIconDirection = 2;
            }
            else if (children[i].localPosition.x == -imageSize.x)
            {
                mapIconDirection = 1;
            }

            if (children[i].localPosition.x == imageSize.x && children[i].localPosition.y == -imageSize.y)
            {
                mapIconDirection = 6;
            }
            else if (children[i].localPosition.x == imageSize.x && children[i].localPosition.y == imageSize.y)
            {
                mapIconDirection = 4;
            }
            else if (children[i].localPosition.x == imageSize.x)
            {
                mapIconDirection = 5;
            }

            if (children[i].localPosition.y == imageSize.y && children[i].localPosition.x != imageSize.x && children[i].localPosition.x != -imageSize.x)
            {
                mapIconDirection = 3;
            }
            if (children[i].localPosition.y == -imageSize.y && children[i].localPosition.x != imageSize.x && children[i].localPosition.x != -imageSize.x)
            {
                mapIconDirection = 7;
            }

            //set the orientation of the child to indicate the direction
            if (mapIconDirection != 0)
            {
                children[i].GetComponent<graphicsMapIcon>().atBounds = true;
                children[i].GetComponent<graphicsMapIcon>().direction = mapIconDirection;
            }
            else
            {
                children[i].GetComponent<graphicsMapIcon>().atBounds = false;
                children[i].GetComponent<graphicsMapIcon>().direction = mapIconDirection;
            }
        }
    }
}

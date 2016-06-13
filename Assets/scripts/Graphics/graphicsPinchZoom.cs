using UnityEngine;
using System;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Hit;

public class graphicsPinchZoom : MonoBehaviour {

    public float scrollSpeed = 1.0f;
    public Transform[] children;

    private float scaleDelta = 1f;
    private bool pressed = false;
    private TouchHit previousHit;
    private TouchHit currentHit;
    private Renderer r;
    private Material m;
    private bool multiTouch = false;

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

        //currentHit = hit;

        //pressed = true;
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
        for (int i = 0; i < childCount; i++)
        {
            children[i] = gameObject.transform.GetChild(i);
        }
	}
	
	void Update ()
    {
        if (pressed)
        {
            //get tiling factor
            Vector2 tilingFactor = m.GetTextureScale("_MainTex");

            //create new offset for texture co-ords
            Vector2 newOffset = m.GetTextureOffset("_MainTex");

            //only get scroll input if have a valid delta and we are not scrolling
            if (previousHit.Point != Vector3.zero && !multiTouch)
            {
                //get difference from this touch position and last
                Vector2 touchDelta = new Vector2(previousHit.Point.x - currentHit.Point.x, previousHit.Point.y - currentHit.Point.y);

                //create lerp between current offset and new offset
                newOffset = Vector2.Lerp(newOffset, newOffset + touchDelta, (Time.deltaTime * scrollSpeed) * tilingFactor.x);
            }

            //clamp the new offset
            newOffset = new Vector2(Mathf.Clamp(newOffset.x, 0f, 1 - tilingFactor.x), Mathf.Clamp(newOffset.y, 0f, 1 - tilingFactor.y));

            //apply new co-ords
            m.SetTextureOffset("_MainTex", newOffset);

            //apply scale delta
            Vector2 scaleFactor = m.GetTextureScale("_MainTex");
            scaleFactor /= scaleDelta;

            //clamp the scale
            scaleFactor = new Vector2( Mathf.Clamp( scaleFactor.x, 0.1f, 1.0f ), Mathf.Clamp( scaleFactor.y, 0.1f, 1.0f ));
            
            //set the scale
            m.SetTextureScale("_MainTex", scaleFactor);

            //update previous hit so we can get a delta on next frame
            previousHit = currentHit;

            //transform any children

        }
    }
}

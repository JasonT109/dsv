using UnityEngine;
using System.Collections;

public class MenuHotkeys : MonoBehaviour 
{
    public PasswordScreen LoginScreen;
    public LiveFeedInputManager LiveFeed;


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown("b"))
        {
            LoginScreen.ToggleBigSub();
        }

        if(Input.GetKeyDown("g"))
        {
            LoginScreen.ToggleGlider();
        }

        if(Input.GetKeyDown("d"))
        {
            LoginScreen.ToggleDCC();
        }

        ////////////////////////////////////

        if(Input.GetKeyDown("l"))
        {
            LiveFeed.ToggleCameras();
            LiveFeed.ToggleCameras();
        }

        if(Input.GetKeyDown("s"))
        {
            LiveFeed.StartCameras();
        }
            
        ////////////////////////////////////
       

        if(Input.GetKeyDown("c"))
        {
            LoginScreen.ToggleClient();
        }

        if(Input.GetKeyDown("h"))
        {
            LoginScreen.ToggleHost();
        }

        ////////////////////////////////////

		if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                LoginScreen.StartButton();
        
	}
        
}

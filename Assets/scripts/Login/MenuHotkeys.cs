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
		if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                LoginScreen.StartButton();
	}
        
}

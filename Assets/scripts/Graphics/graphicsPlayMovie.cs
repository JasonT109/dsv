using UnityEngine;
using System.Collections;

public class graphicsPlayMovie : MonoBehaviour {

    public MovieTexture mtex;
    private Renderer r;

	// Use this for initialization
	void Start ()
    {
        r = GetComponent<Renderer>();
        r.material.mainTexture = mtex;
    }
	
	// Update is called once per frame
	void Update ()
    {
        r = GetComponent<Renderer>();
        MovieTexture movie = (MovieTexture)r.material.mainTexture;
        movie.loop = true;

        if (movie.isPlaying)
        {
            movie.Pause();
        }
        else
        {
            movie.Play();
        }
    }
}

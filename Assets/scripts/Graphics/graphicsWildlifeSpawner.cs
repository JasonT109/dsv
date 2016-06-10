using UnityEngine;
using System.Collections;

public class graphicsWildlifeSpawner : MonoBehaviour {

    public GameObject creature;
    public string creatureTag = "Creature";
    public int minNumCreatures = 1;
    public bool spawnOnEvent = false;
    public float maxSpawnDistance = 100f;
    public float minSpawnDistance = 10f;

    private int numSpawnedCreatures = 0;
    private GameObject[] spawnedCreatures;

	// Use this for initialization
	void Start ()
    {
	    
	}

	// Update is called once per frame
	void Update ()
    {
	    if (!spawnOnEvent)
        {
            spawnedCreatures = GameObject.FindGameObjectsWithTag(creatureTag);
            numSpawnedCreatures = spawnedCreatures.Length;

            if (numSpawnedCreatures < minNumCreatures)
            {
                //spawn another one
                Vector3 spawnPos = new Vector3(Random.Range (transform.position.x - maxSpawnDistance, transform.position.x + maxSpawnDistance), transform.position.y, Random.Range(transform.position.z - maxSpawnDistance, transform.position.z + maxSpawnDistance));

                if (spawnPos.x < transform.position.x + minSpawnDistance && spawnPos.x > transform.position.x - minSpawnDistance)
                {
                    spawnPos.x = transform.position.x + minSpawnDistance;
                }
                if (spawnPos.z < transform.position.z + minSpawnDistance && spawnPos.z > transform.position.z - minSpawnDistance)
                {
                    spawnPos.z = transform.position.z + minSpawnDistance;
                }
                GameObject c = (GameObject)Instantiate(creature, spawnPos, Quaternion.identity);
                //NetworkServer.Spawn(c);
            }
        }

	}
}

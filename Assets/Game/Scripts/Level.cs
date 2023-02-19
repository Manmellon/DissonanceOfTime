using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelName;
    public Sprite thumbnailSprite;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        //Assign child entities here and cache their initial states
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitLevel()
    {
        Player.singleton.controller.enabled = false;

        Player.singleton.transform.position = spawnPoint.position;
        Player.singleton.transform.rotation = Quaternion.Euler(new Vector3(0, spawnPoint.rotation.eulerAngles.y, 0));

        Player.singleton.controller.enabled = true;
    }
}

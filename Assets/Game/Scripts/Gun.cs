using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public void Shoot()
    {
        Player.singleton.isShooting = true;
    }
}

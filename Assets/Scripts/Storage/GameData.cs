using UnityEngine;

[System.Serializable]
public class GameData
{
    public float health;
    //public float bulletsIHave;
    //public float bulletsInTheGun;
    //public float amountOfBulletsPerLoad;
    public Vector3 playerPosition;

    public GameData()
    {
        health = 100;
        //bulletsIHave = 560;
        //bulletsInTheGun = 40;
        //amountOfBulletsPerLoad = 40;
        playerPosition = new Vector3(-102.368713f, -24.4370003f, 62.3272438f);
    }
}
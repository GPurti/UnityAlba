using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;         //Tells Random to use the Unity Engine random number generator.

public class RoomBoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.


        //Assignment constructor.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;                                         //Number of columns in our room
    public int rows = 8;                                            //Number of rows in our room
    public Count foodCount = new Count(1, 5);                        //Lower and upper limit for our random number of food items per level.
    public GameObject[] foodTiles;                                    //Array of food prefabs.
    public GameObject[] enemyTiles;                                    //Array of enemy prefabs.
    public GameObject[] players;
    //public GameObject[] structureTiles;
    //public Count structureCount = new Count(1, 5);
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for(float x = transform.position.x - (float)3.5; x <= transform.position.x + (float)3.5; x++)
        {
            for(float y = transform.position.y - (float)3.5; y <= transform.position.y + (float)3.5; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Reset our list of gridpositions.
        InitialiseList();

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //LayoutObjectAtRandom(structureTiles, structureCount.minimum, structureCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        if (gameObject.tag == "CentralRoom")
        {
            LayoutObjectAtRandom(players, 1, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGameManager : MonoBehaviour
{
    private RoomBoardManager roomBoardManager;
    private int level = 3;


    public float levelStartDelay = 2f;                        //Time to wait before starting level, in seconds.
    public float turnDelay = 0.1f;                            //Delay between each Player turn.
    public int playerCoinPoints = 0;                        //Starting value for Player coin points.
    public List<string> elementsAvailable = new List<string>();
    

    [HideInInspector] public bool playersTurn = true;        //Boolean to check if it's players turn, hidden in inspector but public.


    void Awake()
    {
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        roomBoardManager = GetComponent<RoomBoardManager>();

        if (gameObject.tag == "CentralRoom")
        {
            elementsAvailable.Add("Fire");
            InitGame();
        }
    }

    //Initializes the game for each level.
    public void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        roomBoardManager.SetupScene(level);

    }
    //GameOver is called when the player reaches 0 food points
    public void GameOver()
    {

        //Enable black background image gameObject.
        //levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }
}

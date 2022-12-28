using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
	public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
	
	public int pointsPerCoin = 20;
	
	//public Text coinText;
	//public Text hpText;
	public AudioClip moveSound1;                //1 of 2 Audio clips to play when player moves.
	public AudioClip moveSound2;                //2 of 2 Audio clips to play when player moves.
	public AudioClip gameOverSound;             //Audio clip to play when player dies.

	private Animator animator;                  //Used to store a reference to the Player's animator component.
	private int coins;
	private int hp=10;

	private RoomGameManager roomGameManager;

	//Start overrides the Start function of MovingObject
	protected override void Start()
	{
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

		//Get the current food point total stored in GameManager.instance between levels.

		coins = roomGameManager.playerCoinPoints;

		//Set the foodText to reflect the current player food total.

		//coinText.text = "Money: " + coins;

		//Call the Start function of the MovingObject base class.
		base.Start();
	}


	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable()
	{
		//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
		roomGameManager.playerCoinPoints = coins;
	}


	private void Update()
	{
		//If it's not the player's turn, exit the function.
		if (!roomGameManager.playersTurn) return;

		int horizontal = 0;      //Used to store the horizontal move direction.
		int vertical = 0;        //Used to store the vertical move direction.


		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int)(Input.GetAxisRaw("Horizontal"));

		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int)(Input.GetAxisRaw("Vertical"));

		//Check if moving horizontally, if so set vertical to zero.
		if (horizontal != 0)
		{
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0)
		{
			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove<Totem>(horizontal, vertical);
		}
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D(Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
		if (other.tag == "Exit")
		{
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			Invoke("Restart", restartLevelDelay);

			//Disable the player object since level is over.
			enabled = false;
		}

		//Check if the tag of the trigger collided with is Food.
		else if (other.tag == "ElementFire")
		{

			roomGameManager.elementsAvailable.Add("Fire");

			//Disable the food object the player collided with.
			other.gameObject.SetActive(false);

			//s'haurien de desactivar els altres
		}
		else if (other.tag == "Coin")
		{
			//Add pointsPerFood to the players current food total.
			coins += pointsPerCoin;

			//Update foodText to represent current total and notify player that they gained points
			//coinText.text = "+" + pointsPerCoin + " Coins: " + coins;

			//Disable the food object the player collided with.
			other.gameObject.SetActive(false);
		}

	}
	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove<T>(xDir, yDir);

		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;

		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move(xDir, yDir, out hit))
		{

		}

		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		roomGameManager.playersTurn = false;
	}


	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove<T>(T component)
	{
		//Set hitWall to equal the component passed in as a parameter.
		Totem hitTotem = component as Totem;

		//Call the DamageWall function of the Wall we are hitting.
		hitTotem.ChoseElement();

		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		animator.SetTrigger("playerChop");
	}


	//Restart reloads the scene when called.
	private void Restart()
	{
		//Load the last scene loaded, in this case Main, the only scene in the game.
		SceneManager.LoadScene(0);
	}


	//LoseFood is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public void LoseFood(int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger("playerHit");

		//Subtract lost food points from the players total.
		hp -= loss;

		//Update the food display with the new total.
		//hpText.text = "-" + loss + " HP: " + hp;

		//Check to see if game has ended.
		CheckIfGameOver();
	}


	//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
	private void CheckIfGameOver()
	{
		//Check if food point total is less than or equal to zero.
		if (hp <= 0)
		{


			//Call the GameOver function of GameManager.
			roomGameManager.GameOver();
		}
	}
}

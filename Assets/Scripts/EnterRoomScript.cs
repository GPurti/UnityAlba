using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (this.transform.parent.gameObject.tag == "CentralRoom")
                return;
            this.transform.parent.gameObject.GetComponent<RoomGameManager>().InitGame();
        }
    }
}

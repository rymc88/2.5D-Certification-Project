using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LadderEnter")
        {
            var verticalInput = Input.GetAxisRaw("Vertical");
            
            if (verticalInput != 0)
            {
                var player = GetComponentInParent<Player>();

                player.ClimbLadder();
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Container")
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && horizontalInput !=0)
            {
                var player = GetComponentInParent<Player>();

                player.JumpOver();
            }

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LadderEnter")
        {
            var player = GetComponentInParent<Player>();

            player.LadderBottomExit();
        }
    }

}

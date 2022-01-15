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

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LadderEnter")
        {
            var player = GetComponentInParent<Player>();

            player.LadderBottomExit();
        }
    }

}

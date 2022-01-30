using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            player.Pushing();
            transform.Translate(Vector3.right * 2 * Time.deltaTime);
        }
    }
  
}

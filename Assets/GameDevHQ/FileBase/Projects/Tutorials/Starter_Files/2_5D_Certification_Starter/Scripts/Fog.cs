using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField] private float _speed;
    private ToxicityBar _toxicityBar;

    // Start is called before the first frame update
    void Start()
    {
        _toxicityBar = GameObject.Find("Toxicity_Bar").GetComponent<ToxicityBar>();

        if(_toxicityBar == null)
        {
            Debug.Log("Toxicity Bar is Null");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = new Vector3(1, 0, 0) * _speed;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            _toxicityBar.ToxicExposure(1);
        }
    }
}

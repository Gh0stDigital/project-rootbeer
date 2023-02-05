using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root_Object : MonoBehaviour
{
    public int rootHealth = 5;

    [SerializeField]
    private ParticleSystem _mineParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            rootHealth--;

            if(rootHealth == 0)
            {
                MineRoot();
            }
        }
    }

    public void MineRoot()
    {
        Instantiate(_mineParticleSystem, this.gameObject.transform.position, Quaternion.identity);
    }


}

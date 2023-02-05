using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root_Object : MonoBehaviour
{
    public int rootHealth = 5;

    [SerializeField]
    private ParticleSystem _rootParticleFX, _destroyParticleFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void HitRoot()
    {
        rootHealth--;
        Instantiate(_rootParticleFX, this.gameObject.transform.position, Quaternion.identity);

        if (rootHealth == 0)
        {
            MineRoot();
        }
    }

    public void MineRoot()
    {
        FindObjectOfType<RootRun_GameManager>().playersRoots++;
        Instantiate(_destroyParticleFX, this.gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


}

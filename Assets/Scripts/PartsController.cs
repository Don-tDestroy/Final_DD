using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsController : MonoBehaviour
{
    public GameObject part;
    public int partAmount;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        for(int i=0; i<partAmount; i++)
        {
            CreatePart();
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    private void CreatePart()
    {
        GameObject newPart = Instantiate(part, this.transform.position, Quaternion.identity);
        Rigidbody partRb = newPart.GetComponent<Rigidbody>();

        float randomX = Random.Range(-500f, 500f);
        float randomZ = Random.Range(-500f, 500f);

        partRb.AddForce(new Vector3(randomX, 700f, randomZ), ForceMode.Impulse);    
        partRb.AddTorque(new Vector3(randomX, randomX, randomX), ForceMode.Impulse);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

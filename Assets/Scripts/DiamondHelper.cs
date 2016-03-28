using UnityEngine;
using System.Collections;

public class DiamondHelper : MonoBehaviour {

	void Start()
    {
        // При появлении, подбрасываем камень вверх в случайном направления, чтобы создать эффект выпадания из паука.
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.value * 3, 3, Random.value * 3), ForceMode.Impulse);
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerHelper>())
        {
            other.gameObject.GetComponent<PlayerHelper>().score++;
            Destroy(gameObject);                                  
        }
    }
}

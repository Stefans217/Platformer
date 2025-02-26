using UnityEngine;
using UnityEngine.Events;

public class CoinCollector : MonoBehaviour
{
    public UnityEvent OnCoinCollected = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin")
        {
            Debug.Log("Coin collected");
            OnCoinCollected.Invoke();
            Destroy(other.gameObject);
        }
    }


}

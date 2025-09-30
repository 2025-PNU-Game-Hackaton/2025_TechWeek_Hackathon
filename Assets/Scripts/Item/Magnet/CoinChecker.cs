using System.Collections.Generic;
using UnityEngine;

public class CoinChecker : MonoBehaviour
{
    [SerializeField] private List<GameObject> coins = new();
    [SerializeField] private float forceValue = 10;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Coin") && !coins.Contains(other.gameObject))
        {
            coins.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Coin") && coins.Contains(other.gameObject))
        {
            coins.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// 코인 체커가 활성화 된 동안 일정 범위 안의 코인을 끌어당깁니다.
    /// </summary>
    private void Update()
    {
        foreach (GameObject coin in coins)
        {
            Vector3 direction = (transform.position - coin.transform.position).normalized;
            coin.GetComponent<Rigidbody>().AddForce(direction * forceValue);
        }
    }
}
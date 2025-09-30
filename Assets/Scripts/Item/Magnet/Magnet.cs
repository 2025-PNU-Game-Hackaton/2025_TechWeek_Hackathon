using System.Collections;
using UnityEngine;

public class Magnet : MonoBehaviour, IItem
{
    [SerializeField] private GameObject CoinCheckerPrefab; // 플레이어에게 일시적으로 추가해 줄 코인 체커 프리팹

    private WaitForSeconds magnetTime = new (5); // 5초 동안 당김

    /// <summary>
    /// 플레이어 주변의 코인을 플레이어 쪽으로 끌어들입니다.
    /// </summary>
    public void UseItem(GameObject itemEater)
    {
        // 플레이어가 자석상태인지 확인
        bool isMagnet = itemEater.transform.parent.GetComponent<PlayerController>().isMagnet;

        // 이미 자석 상태이면 처음부터 타이머를 다시 시작
        if (isMagnet)
        {
            StopAllCoroutines();            
            Destroy(itemEater.transform.Find("CoinChecker")); // 코인 체커를 제거
        }
        StartCoroutine(TimerCoroutine(itemEater));

        // 아이템을 화면에서 제거
        Destroy(gameObject);
    }

    private IEnumerator TimerCoroutine(GameObject itemEater)
    {
        // 플레이어에게 코인 체커 추가
        Instantiate(CoinCheckerPrefab, itemEater.transform);

        // 논리값도 같이 변경
        itemEater.transform.parent.GetComponent<PlayerController>().isMagnet = true;

        // 5초 대기
        yield return magnetTime;

        // 플레이어가 가진 코인 체커를 제거
        Destroy(itemEater.transform.Find("CoinChecker"));

        // 논리값도 원래대로 되돌림
        itemEater.transform.parent.GetComponent<PlayerController>().isMagnet = false;
    }
}
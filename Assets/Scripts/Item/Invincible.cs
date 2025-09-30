using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour, IItem
{
    private WaitForSeconds invincibleTime = new (5); // 5초 동안 무적

    /// <summary>
    /// 아이템 획득 시, 일정 시간동안 플레이어는 장애물로부터 피해를 받지 않습니다.
    /// 무적 상태인 도중, 무적 아이템을 또 획득하였을 경우, 무적시간이 처음 부터 다시 시작됩니다
    /// </summary>
    public void UseItem(GameObject itemEater)
    {
        // 플레이어가 무적상태인지 확인
        bool isInvincible = itemEater.transform.parent.GetComponent<PlayerController>().isInvincible;

        // 무적상태라면 코루틴을 종료 후 재실행
        if (isInvincible)
        {
            StopAllCoroutines();
        }
        StartCoroutine(TimerCoroutine(itemEater));

        // 아이템을 화면에서 제거
        Destroy(gameObject);
    }

    private IEnumerator TimerCoroutine(GameObject itemEater)
    {
        // 플레이어를 무적으로 변경
        itemEater.transform.parent.GetComponent<PlayerController>().isInvincible = true;

        // 5초 대기
        yield return invincibleTime;

        // 플레이어를 다시 원래대로 되돌림
        itemEater.transform.parent.GetComponent<PlayerController>().isInvincible = false;
    }
}
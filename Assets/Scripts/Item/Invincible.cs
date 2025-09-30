using System.Collections;
using UnityEngine;

public class Invincible : MonoBehaviour, IItem
{
    private WaitForSeconds invincibleTime = new (5); // 5초 동안 무적

    /// <summary>
    /// 아이템 획득 시, 일정 시간동안 플레이어는 장애물로부터 피해를 받지 않습니다.
    /// 무적 상태인 도중, 무적 아이템을 또 획득하였을 경우, 무적시간이 처음 부터 다시 시작됩니다
    /// </summary>
    public void UseItem(GameObject player)
    {
        // 플레이어가 무적상태인지 확인. 플레이어의 현재 상태를 저장한 곳에서 가져다 쓰는 게 좋을 듯
        bool isInvincible = false;
        // isInvincible = ???

        // 무적상태라면 코루틴을 종료 후 재실행
        if (isInvincible)
        {
            StopAllCoroutines();
        }
        StartCoroutine(TimerCoroutine());

        // 아이템을 화면에서 제거
        Destroy(gameObject);
    }

    private IEnumerator TimerCoroutine()
    {
        // 플레이어를 무적으로 변경
        // Do something

        // 논리값도 같이 변경
        // Do something

        // 5초 대기
        yield return invincibleTime;

        // 플레이어를 다시 원래대로 되돌림
        // Do something

        // 논리값도 원래대로 되돌림
        // Do something
    }
}
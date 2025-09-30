using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    /// <summary>
    /// 플레이어가 부딪히면 플레이어가 획득한 코인의 개수를 1 증가시킵니다.
    /// </summary>
    public void UseItem(GameObject player)
    {
        // 코인 1 증가(UI, global variable 수정 필요)
        // Do Something

        // 아이템을 화면에서 제거
        Destroy(gameObject);
    }
}
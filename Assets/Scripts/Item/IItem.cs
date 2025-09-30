using UnityEngine;

public interface IItem
{
    /// <summary>
    /// 플레이어가 아이템에 부딪혔을 때 호출해야 하는 함수입니다.
    /// 각 아이템마다 별도로 구현됩니다.
    /// </summary>
    public void UseItem(GameObject player);
}
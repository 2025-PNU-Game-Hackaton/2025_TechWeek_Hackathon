using System.Collections.Generic;
using UnityEngine;

public class ScrollableGround : MonoBehaviour
{
    [Header("Ground Settings")]
    public List<Transform> groundSections;   // 반복될 지형 조각
    public float scrollSpeed = 5f;

    [Header("Barrier Settings")]
    public List<GameObject> barrierPrefabs;  // 장애물 프리팹
    public int poolSizePerPrefab = 5;        // 각 프리팹별 풀 크기

    private float groundLength;

    // Object Pool
    private List<GameObject> barrierPool = new List<GameObject>();

    void Start()
    {
        if (groundSections.Count == 0) return;

        MeshRenderer mr = groundSections[0].GetComponent<MeshRenderer>();
        if (mr != null)
            groundLength = mr.bounds.size.z;
        else
            Debug.LogError("Ground section needs a MeshRenderer!");

        // 1️⃣ Object Pool 초기화
        foreach (var prefab in barrierPrefabs)
        {
            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.tag = "Barrier"; // 제거/재사용을 위해 태그 지정
                barrierPool.Add(obj);
            }
        }

        // 2️⃣ 초기 장애물 배치 (모든 섹션)
        foreach (Transform section in groundSections)
        {
            Ground groundComp = section.GetComponent<Ground>();
            if (groundComp != null)
            {
                SpawnBarrierFromPool(section, groundComp.posList1);
                SpawnBarrierFromPool(section, groundComp.posList2);
                SpawnBarrierFromPool(section, groundComp.posList3);
            }
        }
    }

    void Update()
    {
        foreach (Transform section in groundSections)
        {
            // 지형 스크롤
            section.position += Vector3.back * scrollSpeed * Time.deltaTime;

            // 섹션이 끝까지 가면 맨 뒤로 이동
            if (section.position.z <= -groundLength)
            {
                section.position += new Vector3(0, 0, groundSections.Count * groundLength);

                // 기존 섹션의 장애물 풀로 반환
                ReturnBarriersToPool(section);

                // 새 장애물 배치
                Ground groundComp = section.GetComponent<Ground>();
                if (groundComp != null)
                {
                    SpawnBarrierFromPool(section, groundComp.posList1);
                    SpawnBarrierFromPool(section, groundComp.posList2);
                    SpawnBarrierFromPool(section, groundComp.posList3);
                }
            }
        }
    }

    // 기존 섹션의 장애물을 풀로 반환
    private void ReturnBarriersToPool(Transform parentSection)
    {
        foreach (Transform child in parentSection)
        {
            if (child.CompareTag("Barrier"))
            {
                child.gameObject.SetActive(false);
                child.parent = null; // 섹션에서 분리
            }
        }
    }

    // Object Pool에서 장애물 꺼내 배치
    private void SpawnBarrierFromPool(Transform parentSection, List<Transform> spawnPoints)
    {
        if (spawnPoints == null || spawnPoints.Count == 0) return;

        GameObject barrier = barrierPool.Find(b => !b.activeSelf);
        if (barrier == null) return; // 풀에 남은 장애물이 없음

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        barrier.transform.position = spawnPoint.position;
        barrier.transform.rotation = spawnPoint.rotation;
        barrier.transform.parent = parentSection;
        barrier.SetActive(true);
    }
}

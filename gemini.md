
# Temple Run PC 해커톤 기획 기반 개발 주의사항

## 핵심 목표
- 4.5시간 내 PC용 템플런 러너 게임 플레이 가능한 프로토타입 완성
- 핵심 시스템만 구현: 플레이어 이동(Idle, Run, Jump, Slide, Death), 아이템(코인, Magnet, Invincible), 장애물 충돌/회피, UI 점수·설정·ESC 메뉴

## 필수 구현
- 캐릭터: 5가지 상태(Idle, Run, Jump, Slide, Death) 애니메이션 연동 (Mixamo 무료 소스 적극 활용)
- 아이템/장애물: Prefab으로 간략 구현. Magnet·Invincible 효과는 Coroutine으로 처리
- UI: 점수·설정·ESC·일시정지·기타 주요화면 UI, PlayerPrefs 활용한 점수 저장
- Prefab/Trigger/씬 구조 단순화: 반드시 프리팹 정리 → 1개 트리거로 장애물/아이템 관리

## 제한 및 주의 사항
- 외부 애셋 적극 활용: VARCO 3D, Mixamo, KayKit, HYPEPOLY, Unity Asset Store의 무료 러너/플랫폼 팩 우선[file:43]
- UI, 폰트 무료 소스만 사용: 155 Free Video Game Fonts 등 활용
- 오브젝트 풀링/파티클 등 고급 기능 배제, MVP 우선
- 모든 변수, Prefab 이름 한글 금지(가독성 및 협업 위해)
- UI ESC, 일시정지, 턴 회복 등 메뉴 기능 최소화
- 코드 주석: 핵심 컨트롤·효과 함수 단위로 반드시
- 빌드 오류·런타임 크래시 발상 시, 원인 로그 즉시 추가
- 반드시 PlayerPrefs로 점수 등 핵심 기록 남길 것

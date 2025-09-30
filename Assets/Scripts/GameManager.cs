using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager instance;

    [Header("Difficulty Settings")]
    public float initialSpeed = 10f;       // 초기 속도
    public float maxSpeed = 30f;           // 최대 속도
    public float speedIncreaseRate = 0.1f; // 초당 속도 증가량

    public float currentGameSpeed { get; private set; } // 현재 게임 속도 (다른 스크립트에서 읽기 전용으로 사용)

    [Header("Scoring")]
    public float scoreMultiplier = 1f; // 점수 배율

    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenu";

    private float score = 0f;
    private bool isPaused = false;
    private const string HighScoreKey = "HighScore"; // PlayerPrefs 키

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[GameManager] OnSceneLoaded: 씬 로드 감지. 게임 상태 초기화를 시작합니다.");
        // 씬이 로드될 때마다 게임 상태 초기화
        score = 0f;
        isPaused = false;
        Time.timeScale = 1f;
        currentGameSpeed = initialSpeed; // 속도 초기화

        Debug.Log("[GameManager] Score 변수가 0으로 리셋되었습니다.");

        if (UIManager.instance != null) 
        {
            Debug.Log("[GameManager] UIManager 인스턴스를 발견하여 점수 UI 업데이트를 요청합니다.");
            UIManager.instance.UpdateScoreUI((int)score);
        }
        else
        {
            Debug.LogError("[GameManager] OnSceneLoaded에서 UIManager.instance를 찾지 못했습니다!");
        }
    }

    void Start()
    {
        // Start 메서드는 이제 비워둡니다.
        // 모든 초기화는 OnSceneLoaded에서 처리됩니다.
    }

    void Update()
    {
        // ESC 키를 눌러 게임 일시정지/재개
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // 시간이 지남에 따라 속도 증가 (게임이 실행 중일 때만)
        if (!isPaused && Time.timeScale > 0)
        {
            if (currentGameSpeed < maxSpeed)
            {
                currentGameSpeed += speedIncreaseRate * Time.deltaTime;
            }
            else
            {
                currentGameSpeed = maxSpeed;
            }

            // 현재 속도에 비례하여 점수 증가
            score += currentGameSpeed * scoreMultiplier * Time.deltaTime;
            if (UIManager.instance != null) UIManager.instance.UpdateScoreUI((int)score);
        }
    }

    // 점수 추가 메서드
    public void AddScore(int newScore)
    {
        score += newScore;
        if (UIManager.instance != null) UIManager.instance.UpdateScoreUI((int)score);
    }

    // 게임 오버 처리
    public void GameOver()
    {
        Time.timeScale = 0f; // 시간 정지

        int finalScore = (int)score;

        // 현재 점수와 최고 점수 비교
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (finalScore > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, finalScore);
            currentHighScore = finalScore;
        }

        // UIManager에 게임오버 UI 표시 요청
        if (UIManager.instance != null) UIManager.instance.ShowGameOverPanel(finalScore, currentHighScore);
    }

    // 게임 일시정지
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // 게임 내 시간 흐름을 멈춤
        if (UIManager.instance != null) UIManager.instance.SetPauseMenu(true);
    }

    // 게임 재개
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 내 시간 흐름을 다시 시작
        if (UIManager.instance != null) UIManager.instance.SetPauseMenu(false);
    }

    // 자석 아이템 활성화
    public void ActivateMagnet(float duration)
    {
        if (UIManager.instance != null) UIManager.instance.ActivateMagnetIcon(duration);
        // 여기에 자석 아이템의 실제 로직을 추가하세요 (예: 코인 끌어당기기)
    }

    // 포션 아이템 활성화
    public void ActivatePotion(float duration)
    {
        if (UIManager.instance != null) UIManager.instance.ActivatePotionIcon(duration);
        // 여기에 포션 아이템의 실제 로직을 추가하세요 (예: 무적 상태)
    }

    // 게임 재시작
    public void RestartGame()
    {
        // 씬을 다시 로드하기 전에 시간 흐름을 정상으로 되돌립니다.
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임 나가기
    public void QuitGame()
    {
        Debug.Log("게임 나가기");
        Application.Quit();
    }

    // 메인 메뉴로 돌아가는 함수
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 구독 해제
        Destroy(gameObject); // DontDestroyOnLoad 속성의 GameManager 파괴
        SceneManager.LoadScene(mainMenuSceneName); // 메인 메뉴 씬 로드
    }
}
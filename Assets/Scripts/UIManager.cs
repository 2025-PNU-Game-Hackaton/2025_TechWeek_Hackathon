
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UIManager instance;

    [Header("In-Game UI")]
    public Text scoreText; // 점수를 표시할 Text 컴포넌트
    public GameObject pauseMenuPanel; // 일시정지 메뉴 패널

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // 게임오버 패널
    public Text finalScoreText; // 최종 점수 텍스트
    public Text highScoreText;  // 최고 점수 텍스트

    [Header("Power-up UI")]
    public GameObject magnetActivateImage; // 자석 아이템 활성화 이미지
    public GameObject potionActivateImage; // 포션 아이템 활성화 이미지

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // UI 초기 상태 설정
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (magnetActivateImage != null) magnetActivateImage.SetActive(false);
        if (potionActivateImage != null) potionActivateImage.SetActive(false);
    }

    // 점수 UI 업데이트
    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("[UIManager] scoreText 참조가 비어있습니다(null)! Inspector 창에서 연결이 필요합니다.");
        }
    }

    // 일시정지 메뉴 표시/숨기기
    public void SetPauseMenu(bool isActive)
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(isActive);
    }

    // 게임오버 패널 표시
    public void ShowGameOverPanel(int score, int highScore)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = "Score: " + score;
        if (highScoreText != null) highScoreText.text = "High Score: " + highScore;
    }

    // 자석 아이템 UI 활성화
    public void ActivateMagnetIcon(float duration)
    {
        StartCoroutine(MagnetRoutine(duration));
    }

    // 포션 아이템 UI 활성화
    public void ActivatePotionIcon(float duration)
    {
        StartCoroutine(PotionRoutine(duration));
    }

    private System.Collections.IEnumerator MagnetRoutine(float duration)
    {
        if (magnetActivateImage != null) magnetActivateImage.SetActive(true);
        yield return new WaitForSeconds(duration);
        if (magnetActivateImage != null) magnetActivateImage.SetActive(false);
    }

    private System.Collections.IEnumerator PotionRoutine(float duration)
    {
        if (potionActivateImage != null) potionActivateImage.SetActive(true);
        yield return new WaitForSeconds(duration);
        if (potionActivateImage != null) potionActivateImage.SetActive(false);
    }

    // --- 버튼 이벤트 호출 --- //

    public void OnResumeButtonClicked()
    {
        GameManager.instance.ResumeGame();
    }

    public void OnRestartButtonClicked()
    {
        GameManager.instance.RestartGame();
    }

    public void OnQuitButtonClicked()
    {
        GameManager.instance.QuitGame();
    }

    public void OnGoToMenuButtonClicked()
    {
        GameManager.instance.GoToMainMenu();
    }
}

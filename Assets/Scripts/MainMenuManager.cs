
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Unity Inspector에서 게임 씬의 이름을 설정할 수 있습니다.
    public string gameSceneName = "YJMScene";

    // 게임 시작 버튼을 눌렀을 때 호출될 함수
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // 게임 나가기 버튼을 눌렀을 때 호출될 함수
    public void QuitGame()
    {
        Debug.Log("게임 나가기");
        Application.Quit();
    }
}

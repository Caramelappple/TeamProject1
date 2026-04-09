using UnityEngine;
using UnityEngine.UIElements; // UI Toolkit을 쓰려면 필수! 참고로 이건 제미나이를 썼습니다

public class GameStartHandler : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startButton;

    void OnEnable()
    {
        // 1. UIDocument 컴포넌트 가져오기
        _uiDocument = GetComponent<UIDocument>();

        // 2. 툴킷 안에서 버튼 찾기 (이름이 "StartButton"이라고 가정)
        // UI Builder에서 설정한 'Name'과 똑같아야 해!
        _startButton = _uiDocument.rootVisualElement.Q<Button>("StartButton");

        if (_startButton != null)
        {
            // 3. 버튼 클릭 시 실행될 함수 연결
            _startButton.clicked += OnStartButtonClick;
        }
    }

    void OnStartButtonClick()
    {
        // 4. UI 창 전체를 비활성화 (꺼버리기)
        _uiDocument.rootVisualElement.style.display = DisplayStyle.None;

        // 5. 게임 시작 로직 (예: 일시정지 해제, 캐릭터 움직임 허용 등)
        Debug.Log("게임 시작! UI 비활성화");
        StartGame();
    }

    void StartGame()
    {
        // 여기에 실제 게임 시작 시 필요한 코드(Time.timeScale = 1 등)를 넣어줘
        Time.timeScale = 1f;
    }
}
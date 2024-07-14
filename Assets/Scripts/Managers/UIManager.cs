using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject finishPanel;


	public TMP_Text txtMatch;
	public TMP_Text txtTurn;

	public Button btnStart;
	public Button btnHome;
	public Button btnFinish;

	public Button btnSave;
	public Button btnLoad;

	public GameManager gameManager;
	

	private void Awake() {
		btnStart.onClick.AddListener(onClickedBtnStart);
		btnFinish.onClick.AddListener(onClickedFinish);
		btnHome.onClick.AddListener(onClickedFinish);
		btnSave.onClick.AddListener(gameManager.SaveGame);
		btnLoad.onClick.AddListener(gameManager.LoadGame);
	}

	// Start is called before the first frame update
	void Start()
    {
        controlAllPanel(false);
		startPanel.gameObject.SetActive(true);
	}

    private void controlAllPanel(bool isActive) {
        startPanel.gameObject.SetActive(isActive);
        gamePanel.gameObject.SetActive(isActive);
        finishPanel.gameObject.SetActive(isActive);
    }

    private void onClickedBtnStart() {
        controlAllPanel(false);
        gamePanel.gameObject.SetActive(true);
        AudioManager.Instance.PlayStartSound();
		gameManager.GenerateCards();
	}

	private void onClickedFinish() {
		controlAllPanel(false);
		startPanel.gameObject.SetActive(true);
		AudioManager.Instance.PlayStartSound();
	}

	public void GameOver() {
		controlAllPanel(false);
		finishPanel.gameObject.SetActive(true);
		AudioManager.Instance.PlayGameFinishSound();
	}
}

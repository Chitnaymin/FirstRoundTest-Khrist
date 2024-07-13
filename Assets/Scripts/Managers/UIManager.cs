using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject nextPanel;

    public Button btnStart;

	private void Awake() {
		btnStart.onClick.AddListener(onClickedBtnStart);
	}

	// Start is called before the first frame update
	void Start()
    {
        controlAllPanel(false);
		startPanel.gameObject.SetActive(true);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void controlAllPanel(bool isActive) {
        startPanel.gameObject.SetActive(isActive);
        gamePanel.gameObject.SetActive(isActive);
        nextPanel.gameObject.SetActive(isActive);
    }

    private void onClickedBtnStart() {
        controlAllPanel(false);
        gamePanel.gameObject.SetActive(true);
        AudioManager.Instance.PlayStartSound();
    }
}

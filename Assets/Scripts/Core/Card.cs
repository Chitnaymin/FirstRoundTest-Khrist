using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite frontImg;

    public Sprite backImg;

	public int ID = -1;

	private Image image;
	public bool isMatched = false;
	private GameManager gameManager;

	private void Awake() {
		Button btnCard = transform.GetComponent<Button>();
		btnCard.onClick.AddListener(onClickedCard);
	}

	void Start() {
		image = GetComponent<Image>();
		//animator = GetComponent<Animator>();
		image.sprite = backImg;
	}

	public void InitData(int _id, Sprite _frontImg, GameManager _gameManager) {
		ID = _id;
		frontImg = _frontImg;
		gameManager = _gameManager;
	}

	void onClickedCard() {
		if (!isMatched) {
			//FlipCard();
			gameManager.CardSelected(this);
		}
	}


	public void FlipCard() {
		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOScaleX(0, 0.3f).From(-1).OnComplete(() => {
			image.sprite = frontImg;
		}));

		sequence.Append(transform.DOScaleX(1, 0.3f).From(0).OnComplete(() => {
			gameManager.CardFlipped();
		}));
	}

	public void ReFlipCard() {
		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOScaleX(0, 0.3f).From(1).OnComplete(() => {
			image.sprite = backImg;
		}));

		sequence.Append(transform.DOScaleX(-1, 0.3f).From(0).OnComplete(() => {
			gameManager.CardReFlipped();
		}));
	}

	public void InitFlipCard() {
		Sequence sequence = DOTween.Sequence();
		sequence.Append(transform.DOScaleX(0, 0.3f).From(-1).OnComplete(() => {
			image.sprite = frontImg;
		}));

		sequence.Append(transform.DOScaleX(1, 1f).From(0).OnComplete(() => {
			
		}));

		sequence.Append(transform.DOScaleX(0, 1f).From(1).OnComplete(() => {
			image.sprite = backImg;
		}));

		sequence.Append(transform.DOScaleX(-1, 0.3f).From(0).OnComplete(() => {
			gameManager.isReFlipping = false;
		}));
	}

	public void SetMatched() {
		isMatched = true;
		this.GetComponent<Image>().enabled = false;
	}
}

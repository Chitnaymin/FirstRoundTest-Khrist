using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite frontImg;

    public Sprite backImg;

	private Image image;
	//private Animator animator;
	private bool isFlipped = false;
	private bool isMatched = false;

	private void Awake() {
		Button btnCard = transform.GetComponent<Button>();
		btnCard.onClick.AddListener(onClickedCard);
	}

	void Start() {
		image = GetComponent<Image>();
		//animator = GetComponent<Animator>();
		image.sprite = backImg;
	}

	void onClickedCard() {
		if (!isMatched && !isFlipped) {
			FlipCard();
			GameManager.Instance.CardSelected(this);
		}
	}

	void OnMouseDown() {
		if (!isMatched && !isFlipped) {
			FlipCard();
			GameManager.Instance.CardSelected(this);
		}
	}

	public void FlipCard() {
		isFlipped = !isFlipped;
		//animator.SetTrigger("Flip");
		transform.DOScaleX(-1, 1);
	}

	public void SetMatched() {
		isMatched = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public GameObject cardPrefab;
	public Transform cardParent;
	public int rows = 2;
	public int columns = 2;
	public List<Sprite> cardImages;
	public int score = 0;

	private List<Card> selectedCards = new List<Card>();
	private List<Card> allCards = new List<Card>();

	void Awake() {
		
	}

	void Start() {
		GenerateCards();
	}

	void GenerateCards() {
		// Ensure even number of cards
		int totalCards = rows * columns;
		if (totalCards % 2 != 0) return;

		// Duplicate and shuffle card images
		List<Sprite> images = new List<Sprite>(cardImages);
		images.AddRange(cardImages);
		images = ShuffleList(images);

		// Create card grid
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				GameObject cardObj = Instantiate(cardPrefab, cardParent);
				Card card = cardObj.GetComponent<Card>();
				card.frontImg = images[i * columns + j];
				cardObj.transform.position = new Vector3(i, j, 0);
				allCards.Add(card);
			}
		}
	}

	public void CardSelected(Card card) {
		selectedCards.Add(card);
		if (selectedCards.Count == 2) {
			StartCoroutine(CheckForMatch());
		}
	}

	IEnumerator CheckForMatch() {
		yield return new WaitForSeconds(0.5f);
		if (selectedCards[0].frontImg == selectedCards[1].frontImg) {
			selectedCards[0].SetMatched();
			selectedCards[1].SetMatched();
			score += 10; // Increment score for a match
			AudioManager.Instance.PlayMatchSound();
		} else {
			selectedCards[0].FlipCard();
			selectedCards[1].FlipCard();
			AudioManager.Instance.PlayMismatchSound();
		}
		selectedCards.Clear();
	}

	public void SaveGame() {
		// Save game state
	}

	public void LoadGame() {
		// Load game state
	}

	private List<T> ShuffleList<T>(List<T> list) {
		for (int i = 0; i < list.Count; i++) {
			T temp = list[i];
			int randomIndex = Random.Range(i, list.Count);
			list[i] = list[randomIndex];
			list[randomIndex] = temp;
		}
		return list;
	}
}

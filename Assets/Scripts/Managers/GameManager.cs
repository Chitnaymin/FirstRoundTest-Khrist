using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject cardPrefab;
	public Transform cardParent;
	public int rows = 2;
	public int columns = 2;
	public List<Sprite> cardImages;
	public int score = 0;
	public int flippedCards = 0;
	public bool isReFlipping = false;


	int matchCount = 0;
	int turnCount = 0;
	int currentCardCount = 0;


	private List<Card> selectedCards = new List<Card>();
	private List<Card> allCards = new List<Card>();

	void Awake() {
		cardParent.GetComponent<GridLayoutGroup>().constraintCount = columns;
	}

	public void GenerateCards() {
		resetAllData();
		// Ensure even number of cards
		int totalCards = rows * columns;
		// Adjust if the total number of cards is odd
        bool hasExtraCard = totalCards % 2 != 0;
        int adjustedTotalCards = hasExtraCard ? totalCards : totalCards;

        currentCardCount = adjustedTotalCards / 2;
		List<int> cardsIndexes = new List<int>();
		for (int i = 0; i < currentCardCount; i++) {
			for (int j = 0; j < 2; j++) {
				cardsIndexes.Add(i);
			}
		}
		cardsIndexes = ShuffleList(cardsIndexes);

		// Set the dynamic size for cards
		SetCardSize(totalCards);

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				int index = i * columns + j;
				if (index >= cardsIndexes.Count) {
					return;
				}

				GameObject cardObj = Instantiate(cardPrefab, cardParent);
				Card card = cardObj.GetComponent<Card>();
				card.InitData(cardsIndexes[index], cardImages[cardsIndexes[index]], this);
				cardObj.transform.position = new Vector3(i, j, 0);
				allCards.Add(card);
			}
		}

		initShow(allCards);
	}

	private void SetCardSize(int totalCards) {
		GridLayoutGroup gridLayoutGroup = cardParent.GetComponent<GridLayoutGroup>();

		// Get the dimensions of the parent container
		RectTransform parentRect = cardParent.GetComponent<RectTransform>();
		float parentWidth = parentRect.rect.width;
		float parentHeight = parentRect.rect.height;

		// Calculate the size of each card based on the grid layout
		float cellWidth = parentWidth / columns;
		float cellHeight = parentHeight / rows;

		// Set a maximum size for the cards
		float maxWidth = 300f;
		float maxHeight = 350f;

		// Adjust the cell size to fit within the parent container
		cellWidth = Mathf.Min(cellWidth, maxWidth);
		cellHeight = Mathf.Min(cellHeight, maxHeight);

		// Apply the calculated cell size to the GridLayoutGroup
		gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
	}

	void initShow(List<Card> _allCard) {
		
		for(int i = 0;i<_allCard.Count;i++) {
			_allCard[i].InitFlipCard();
		}
		isReFlipping = true;
	}

	public void CardSelected(Card card) {
		if (isReFlipping) return;
		if (selectedCards.Count >= 2) return;
		selectedCards.Add(card);
		card.FlipCard();
		AudioManager.Instance.PlayFlipSound();
	}

	public void CardFlipped() {
		flippedCards++;
		if (flippedCards == 2) {
			checkMatching();
		}
	}

	public void CardReFlipped() {
		flippedCards--;
		if (flippedCards == 0) {
			isReFlipping = false;
		}
	}

	void checkMatching() {
		bool matched = true;
		int tempID = selectedCards[0].ID;
		setTurnCount(1);
		for (int i = 1; i < selectedCards.Count; i++) {
			if (selectedCards[i].ID != tempID) {
				AudioManager.Instance.PlayMismatchSound();
				matched = false; break;

			}
		}
		if (matched) {
			AudioManager.Instance.PlayMatchSound();
			for (int i = 0; i < selectedCards.Count; i++) {
				selectedCards[i].SetMatched();
			}
			setMatchCount(1);
			
			selectedCards.Clear();
			flippedCards = 0;
			Debug.Log(currentCardCount);
			if(currentCardCount == matchCount) {
				gameFinished();
			}
		} else {
			
			for (int i = 0; i < selectedCards.Count; i++) {
				selectedCards[i].ReFlipCard();
			}
			
			selectedCards.Clear();
			isReFlipping = true;
		}
	}

	public void SaveGame() {
		// Save general game state
		PlayerPrefs.SetInt("Rows", rows);
		PlayerPrefs.SetInt("Columns", columns);
		PlayerPrefs.SetInt("Score", score);
		PlayerPrefs.SetInt("FlippedCards", flippedCards);
		PlayerPrefs.SetInt("IsReFlipping", isReFlipping ? 1 : 0);
		PlayerPrefs.SetInt("MatchCount", matchCount);
		PlayerPrefs.SetInt("TurnCount", turnCount);
		PlayerPrefs.SetInt("CurrentCardCount", currentCardCount);

		// Save state of all cards
		for (int i = 0; i < allCards.Count; i++) {
			PlayerPrefs.SetInt("Card_" + i + "_ID", allCards[i].ID);
			PlayerPrefs.SetInt("Card_" + i + "_IsMatched", allCards[i].isMatched ? 1 : 0);
		}

		PlayerPrefs.SetInt("CardCount", allCards.Count);
		PlayerPrefs.Save();
	}

	public void LoadGame() {
		if (!PlayerPrefs.HasKey("Rows")) return;
		resetAllData();

		rows = PlayerPrefs.GetInt("Rows");
		columns = PlayerPrefs.GetInt("Columns");
		score = PlayerPrefs.GetInt("Score");
		flippedCards = PlayerPrefs.GetInt("FlippedCards");
		isReFlipping = PlayerPrefs.GetInt("IsReFlipping") == 1;
		int _matchCount = PlayerPrefs.GetInt("MatchCount");
		int _turnCount = PlayerPrefs.GetInt("TurnCount");
		currentCardCount = PlayerPrefs.GetInt("CurrentCardCount");

		GenerateCards();

		for (int i = 0; i < allCards.Count; i++) {
			int cardID = PlayerPrefs.GetInt("Card_" + i + "_ID");
			bool isMatched = PlayerPrefs.GetInt("Card_" + i + "_IsMatched") == 1;
			allCards[i].ID = cardID;
			if (isMatched) {
				allCards[i].SetMatched();
			}
		}
		matchCount = _matchCount;
		turnCount = _turnCount;

		UIManager.Instance.txtMatch.text = matchCount.ToString();
		UIManager.Instance.txtTurn.text = turnCount.ToString();
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

	private void setTurnCount(int point) {
		turnCount += point;
		UIManager.Instance.txtTurn.text = turnCount.ToString();
	}

	public void setMatchCount(int point) {
		matchCount += point;
		UIManager.Instance.txtMatch.text = matchCount.ToString();
	}

	private void gameFinished() {
		UIManager.Instance.GameOver();
	}

	private void resetAllData() {
		selectedCards.Clear();
		allCards.Clear();
		foreach (Transform child in cardParent) {
			Destroy(child.gameObject);
		}
		matchCount = 0;
		turnCount = 0;
		UIManager.Instance.txtMatch.text = matchCount.ToString();
		UIManager.Instance.txtTurn.text = turnCount.ToString();
	}
}

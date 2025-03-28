using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;
    public List<Button> buttons = new List<Button>();
    [SerializeField] public GridLayoutGroup tobiField;
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    public string spriteFolder = "Memory/Resources/sprites";
    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrentGuesses;
    private int gameGuesses;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    private float _startTime;
    private int _errorCount;

    private float _firstGuessTime;
    private List<float> _reactionTimes = new List<float>();

    public GameObject GameWinPopUp;

    /*void OnValidate()
    {
        string fullPath = $"{Application.dataPath}/{spriteFolder}";
        if (!System.IO.Directory.Exists(fullPath))
        {
            return;
        }

        var folders = new string[] { $"Assets/{spriteFolder}" };
        var guids = AssetDatabase.FindAssets("t:Sprite", folders);

        var newSprites = new Sprite[guids.Length];

        bool mismatch;
        if (puzzles == null)
        {
            mismatch = true;
            puzzles = newSprites;
        }
        else
        {
            mismatch = newSprites.Length != puzzles.Length;
        }

        for (int i = 0; i < newSprites.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            newSprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            mismatch |= (i < puzzles.Length && puzzles[i] != newSprites[i]);
        }

        if (mismatch)
        {
            puzzles = newSprites;
            Debug.Log($"{name} sprite list updated.");
        }
    }
    */
    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;

        _startTime = Time.time;   // фиксируем время начала игры
        _errorCount = 0;          // обнуляем счётчик ошибок
        _reactionTimes.Clear();   // обнуляем список реакционных времен
    }


    void AddGamePuzzles()
    {
        int looper = buttons.Count;
        int index = 0;
        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < objects.Length; i++)
        {
            buttons.Add(objects[i].GetComponent<Button>());
            buttons[i].image.sprite = bgImage;
        }
    }

    public void Update()
    {
        
    }
    public void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickPuzzle(int.Parse(btn.name)));
            //btn.onClick.AddListener(PickPuzzle);
        }
    }

    public void PickPuzzle(int index)
    {

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = index;
            _firstGuessTime = Time.time;  // фиксируем время первой карточки
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            buttons[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = index;
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            buttons[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            // Вычисляем время реакции – разница между текущим временем и временем первой карточки
            float reactionTime = Time.time - _firstGuessTime;
            _reactionTimes.Add(reactionTime);
            Debug.Log("Время реакции: " + reactionTime.ToString("F2") + " сек");

            if (firstGuessPuzzle == secondGuessPuzzle)
            {
                print("Puzzle Match");
            }
            else
            {
                print("not");

                // Если выбранные карточки не совпадают, считаем это ошибкой
                _errorCount++;
            }

            StartCoroutine(CheckThePuzzleMatch());
        }
    }

    IEnumerator CheckThePuzzleMatch()
    {
        yield return new WaitForSeconds(0.5f);
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);
            buttons[firstGuessIndex].interactable = false;
            buttons[secondGuessIndex].interactable = false;
            foreach (Transform child in tobiField.transform)
            {
                GameObject g = child.gameObject;
                if (g.name == buttons[firstGuessIndex].name)
                {
                    g.GetComponent < BoxCollider>().enabled = false;
                }
            }
            foreach (Transform child in tobiField.transform)
            {
                GameObject g = child.gameObject;
                if (g.name == buttons[secondGuessIndex].name)
                {
                    g.GetComponent<BoxCollider>().enabled = false;
                }
            }
            buttons[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            buttons[secondGuessIndex].image.color = new Color(0, 0, 0, 0);
            
            CheckTheGameFinished();


        }
        else
        {
            buttons[firstGuessIndex].image.sprite = bgImage;
            buttons[secondGuessIndex].image.sprite = bgImage;
        }

        firstGuess = secondGuess = false;
    }
    void CheckTheGameFinished()
    {
        countCorrentGuesses++;

        if (countCorrentGuesses == gameGuesses)
        {
            GameWinPopUp.SetActive(true);
            UnlockNewLevel();
            
            if (SessionManager.LogIn)
            {
                int totalPairs = gamePuzzles.Count / 2;
                int currentUserId = SessionManager.UserID;
                int currentScore = countCorrentGuesses; // число угаданных пар
                int currentLevel = ButtonManager.Id;     // уровень игры, выбранный ранее
                bool victory = true;
                double timeTaken = Time.time - _startTime;
                double completionPercentage = 1  - (_errorCount / (totalPairs + _errorCount));
                int performanceRating = (int)(completionPercentage * 5); // пример: чем меньше ошибок, тем выше оценка


                // Вычисляем среднее время реакции
                double averageReactionTime = 0;
                if (_reactionTimes.Count > 0)
                {
                    averageReactionTime = _reactionTimes.Average();
                }

                // Сохраняем историю для Memory игры
                LocalDatabase.Instance.AddGameHistory(currentUserId, GameName.Memory, currentScore, currentLevel, victory,
                    timeTaken, completionPercentage, _errorCount, performanceRating, averageReactionTime);
            }
        }
    }
    
    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void UnlockNewLevel()
    {
        if (ButtonManager.Id >= LevelMenuMemory.unlockedLevel)
        {
            LevelMenuMemory.unlockedLevel += 1;
            PlayerPrefs.SetInt("unlockedlevel", ButtonManager.Id + 1);
            PlayerPrefs.Save();
        }
    }
}
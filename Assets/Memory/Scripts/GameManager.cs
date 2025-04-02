using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;
    public List<Button> buttons = new List<Button>();

    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    public string spriteFolder = "Memory/Resources/sprites";
    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrentGuesses;
    private int gameGuesses;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    public GameObject GameWinPopUp;

    // Новые переменные для отслеживания прогресса
    private float _startTime;
    private int _errorCount;

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
        StartCoroutine(ShowAllCardsForSeconds(5f));
        _errorCount = 0;
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

    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    public void PickPuzzle()
    {

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            buttons[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            buttons[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            if (firstGuessPuzzle == secondGuessPuzzle)
            {
                print("Карточки совпадают!");
            }
            else
            {
                _errorCount++;
                print("Карточки не совпадают!");
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
            Debug.Log("Игра завершилась, время сохранено!");

            if (SessionManager.LogIn)
            {
                int totalPairs = gamePuzzles.Count / 2;
                int currentUserId = SessionManager.UserID;
                int currentScore = countCorrentGuesses; // Число найденных пар.
                int currentLevel = ButtonManager.Id; // Выбранный уровень.
                bool victory = true;
                double timeTaken = Time.time - _startTime;
                double completionPercentage = (1-(double)_errorCount/(_errorCount+currentScore))*100;
                int performanceRating = 1;
                switch (currentLevel)
                {
                    case < 6:
                        performanceRating = (int)(completionPercentage / 100 * 5);
                        break;
                    case < 11:
                        performanceRating = (int)(1.2 * completionPercentage / 100 * 5);
                        break;
                    case < 16:
                        performanceRating = (int)(1.4 * completionPercentage / 100 * 5);
                        break;
                    case < 21:
                        performanceRating = (int)(1.6 * completionPercentage / 100 * 5);
                        break;
                    default:
                        performanceRating = (int)(completionPercentage / 100 * 5);
                        break;
                }
                if (performanceRating < 1)
                    performanceRating = 1;
                else if (performanceRating > 5)
                    performanceRating = 5;
                float averageReaction = 0;

                LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.Memory, currentScore, currentLevel, victory,
                    timeTaken, completionPercentage, _errorCount, performanceRating, averageReaction);
                Debug.Log($"Время прохождения: {timeTaken}");
                Debug.Log($"Счет: {currentScore}");
                Debug.Log($"Количество ошибок: {_errorCount}");
                Debug.Log($"Текущий уровень: {currentLevel}");
                Debug.Log($"Точность выполнения: {completionPercentage}");
                Debug.Log($"Рейтинг: {performanceRating}");
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

    IEnumerator ShowAllCardsForSeconds(float seconds)
    {
        // Показываем все карточки (лицевые изображения)
        for (int i = 0; i < buttons.Count; i++)
        {
            // Предполагается, что порядок кнопок соответствует порядку элементов в gamePuzzles
            buttons[i].image.sprite = gamePuzzles[i];
        }
        // Ждём заданное количество секунд
        yield return new WaitForSeconds(seconds);
        // Скрываем карточки (возвращаем задний фон)
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].image.sprite = bgImage;
        }

        _startTime = Time.time;
        Debug.Log("Игра началась, отсчет времени пошел!");
    }
}
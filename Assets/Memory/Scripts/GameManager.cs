using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
                print("Puzzle Match");
            }
            else
            {
                print("not");
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
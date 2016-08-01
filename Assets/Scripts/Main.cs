using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using ExtensionMethods;

public class Main : MonoBehaviour {

	public InputField input;
	public Button letterButton;
	public Button wordButton;
	public Text hiddenWord;
	public Text guessedLetters;

    public GameObject finalPanel;
    public Text showedWord;
    public GameObject winText;
    public GameObject loseText;
    public Button reloadButton;
    public Button quitButton;

	public GameObject heartPanel;
	public GameObject man;

	public TextAsset wordFile;
	public Sprite emptyHeart;

	char[] separator = {'\n'};
	string invalidCharacters = "0123456789-.,='()&%@$!*#";
	string[] words;
	string currentWord;
	int tries = 0;
	string guessed = "";

	void Awake ()
    {
		words = wordFile.text.Split(separator);
		bool invalid = true;
		while(invalid){
			currentWord = words[Random.Range (0, words.Length)];
			print (currentWord);
			foreach (char ic in invalidCharacters) {
				if (!currentWord.Contains (ic.ToString())) {
					invalid = false;
				} else {
					invalid = true;
					break;
				}
			}
		}
		for (int i = 0; i < currentWord.Length; i++) {
			hiddenWord.text += " _";
		}
		hiddenWord.text = hiddenWord.text.Remove(0, 1);

		letterButton.onClick.AddListener (guessLetter);
        wordButton.onClick.AddListener(guessWord);

        reloadButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(0);
        });
        quitButton.onClick.AddListener(delegate
        {
            Application.Quit();
        });
    }

    void guessLetter ()
    {
        string letter = input.text[0].ToString();
        input.text = "";
        foreach (char c in guessed)
        {
            if (letter == c.ToString())
            {
                Focus();
                return;
            }
        }
        Focus();
        if (currentWord.Contains(letter))
        {
            int[] indexes = currentWord.AllIndexesOf(letter);
            char[] hidden = hiddenWord.text.ToCharArray();
            foreach (int index in indexes)
            {
                hidden[index * 2] = currentWord[index];
            }
            hiddenWord.text = new string(hidden);
        }
        else
        {
            Fail();
        }
        guessed += letter[0];
        char[] guessed_ = guessed.ToCharArray();
        string[] _guessed = new string[guessed_.Length];
        for (int i = 0; i < guessed_.Length; i++)
        {
            _guessed[i] = guessed_[i].ToString();
        }
        guessedLetters.text = string.Join(" ", _guessed);
        if (!hiddenWord.text.Contains("_"))
        {
            Final(true);
        }
    }

    void guessWord ()
    {
        char[] hidden = hiddenWord.text.ToCharArray();
        Focus();
        if (currentWord == input.text)
        {
            for (int index = 0; index < currentWord.Length; index++)
            {
                hidden[index * 2] = currentWord[index];
            }
            hiddenWord.text = new string(hidden);
            Final(true);
        }
        else
        {
            Fail();
        }
    }

    void Focus ()
    {
        EventSystem.current.SetSelectedGameObject(input.gameObject, null);
    }

	void Fail ()
    {
        if(tries >= 5)
        {
            Final(false);
        }
        GameObject part = man.transform.FindChild("Part" + tries.ToString()).gameObject;
        GameObject heart = heartPanel.transform.FindChild("HeartImage" + tries.ToString()).gameObject;
        part.GetComponent<Rigidbody2D>().isKinematic = false;
        heart.GetComponent<Image>().sprite = emptyHeart;
        tries += 1;
    }

    void Final(bool win)
    {
        letterButton.onClick.RemoveAllListeners();
        wordButton.onClick.RemoveAllListeners();
        finalPanel.SetActive(true);
        showedWord.text = currentWord;
        if (win)
            winText.SetActive(true);
        else
            loseText.SetActive(true);
    }

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            guessLetter();
        }
    }
}

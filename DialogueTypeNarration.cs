using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems; //Untuk IPointerDownHangler
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogueTypeNarration : MonoBehaviour, IPointerDownHandler // Dari pada check tiap frame pake Update, mending ini
{
    [SerializeField] bool _playOnStart = true;
    public TextMeshProUGUI Dialogue;
    public TextMeshProUGUI Character;
    public TextMeshProUGUI ClickAnywhere;
    public Image player;
    public Image clackers;
    public Image blackBG;
    public Image bossSilhouette;
    public Image PinkPart;
    public GameObject music;
    // public string[] lines;
    [SerializeField] Lines[] lines;
    [System.Serializable] struct Lines
    {
        public Lines(string charName, string text)
        {
            CharName = charName;
            Text = text;
        }
        public string CharName;
        [TextArea]
        public string Text;
    }

    // public string CharacterName; // Not needed anymore
    public float textSpeed;
    private int index;

    //[SerializeField] AudioSource _audioSource; // GetCompoenent ngealokasi garbage. sebaiknya cache kan
    //Naming convention C# Microsoft. _privateVariable, PublicVariable, passedVariableInsideFunction
    //[SerializeField] AudioClip _typeSound; // Tapi dah ada di Singleton jadi tinggal
    AudioManager _audioManager;
    void Awake()
    {
        _audioManager = Singleton.Instance.Audio;
    }
    void Start()
    {
        // Dialogue.text = string.Empty;
        Dialogue.maxVisibleCharacters = 0; //Prevent weird shift on GUI when the text is really long. Jadi kalau nambah character pake string, nanti pas textnya masih pendek, ukuran fontnya besar. Pas sampai limit textnya, fontnya mengecil terus sampai textnya muat. Kalau pake maxVisibleCharacter, dari awal udah diset ukuran fontnya.
        // Character.text = CharacterName;
        Character.text = lines[0].CharName; // Extra
        if(_playOnStart)StartDialogue(); //Trigger langsung buka text
    }

    public void Play()
    {
        Dialogue.maxVisibleCharacters = 0;
        Character.text = lines[0].CharName;
        StartDialogue();
    }


    // void Update()
    public void OnPointerDown(PointerEventData pointerEventData) // Harus derived dari IPointerDownHandler dan MonoBehaviour. Ini dipanggil tiap mousedown tanpa cek tiap frame
    {
        // if (Input.GetMouseButtonDown(0)) //Click anywhere to continue
        // {
            // if (Dialogue.text == lines[index]) //jika teks di dialog sama dengan indeks yang ditunjuk sekarang
            if (Dialogue.maxVisibleCharacters == lines[index].Text.Length) //jika karakter text yg muncul sama dengan panjang karakter maksimalnya
            {
                NextLine(); //ganti dialog
            }
            else
            {
                StopAllCoroutines();
                // Dialogue.text = lines[index]; 
                Dialogue.maxVisibleCharacters = lines[index].Text.Length; //Prevent weird shift on GUI when the text is really long
            }
        // }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine() //efek typewriter
    {
        foreach (char c in lines[index].Text.ToCharArray())
        {
            // Dialogue.text += c;
            Dialogue.maxVisibleCharacters += 1; // Prevent weird shift on GUI when the text is really long
            // if(c != ' ')
            if(Dialogue.text[Dialogue.maxVisibleCharacters-1] != ' ') // Jika bukan spasi
            {
                // GetComponent<AudioSource>().Play(); // GetCompoenent ngealokasi garbage. sebaiknya cache kan 
                // _audioSource.PlayOneShot(_typeSound); //Play untuk music, PlayOneShot untuk sfx. Performanya beda jauh pas banyak karna playoneshot itu play once, then ignore.
                // Tapi udah ada di Singleton jadi tinggal
                _audioManager.PlaySound(4);
            }
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            // Dialogue.text = string.Empty;
            Dialogue.maxVisibleCharacters = 0; // Hide semua
            Dialogue.text = lines[index].Text; // Ganti ke text selanjutnya
            Character.text = lines[index].CharName; // Extra

            StartCoroutine(TypeLine());
        }
        else
        {
            music.SetActive(false);
            Singleton.Instance.Scene.LoadSceneWithTransition("Level"); //mungkin nanti animasi dulu baru pindah scene di bagian sini
        }
        if(index == 4)
        {
            StartCoroutine(TweenColorIMG(player, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.5f));
            StartCoroutine(TweenColorIMG(blackBG, new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 1.5f));
            StartCoroutine(TweenColorIMG(PinkPart, new Color(PinkPart.color.r, PinkPart.color.g, PinkPart.color.b, 0), new Color(PinkPart.color.r, PinkPart.color.g, PinkPart.color.b, 1), 1.5f));
        }
        if(index == 8)
        {
            StartCoroutine(TweenColorIMG(clackers, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1.5f));
        }
        if(index == 10)
        {
            StartCoroutine(TweenColorIMG(bossSilhouette, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.5f), 2.25f));
        }
        if(index == 11)
        {
            StartCoroutine(TweenColorIMG(bossSilhouette, new Color(0, 0, 0, 0.5f), new Color(0, 0, 0, 0), 2.25f));
        }
        if(index == 12)
        {
            StartCoroutine(TweenColorIMG(player, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.5f));
            StartCoroutine(TweenColorIMG(blackBG, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 1.5f));
            StartCoroutine(TweenColorIMG(PinkPart, new Color(PinkPart.color.r, PinkPart.color.g, PinkPart.color.b, 1), new Color(PinkPart.color.r, PinkPart.color.g, PinkPart.color.b, 0), 1.5f));
            StartCoroutine(TweenColorIMG(clackers, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 1.5f));
        }
        
    }

    IEnumerator TweenColorIMG(Image img, Color start, Color end, float duration)
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            img.color = Color.Lerp(start, end, Ease.InOutCubic(t));
            yield return null;
        }
        img.color = end;
    }
}

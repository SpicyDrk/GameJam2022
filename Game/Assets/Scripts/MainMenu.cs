using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] TextMeshProUGUI dialog;
    [SerializeField] Sprite catSprit, unknwonSprite;
    [SerializeField] Image image;

    private int currentText = 0;
    string[] dialogTexts = { "Wh... What happened? Where am I?", 
        "You have used your last life on earth.. It's time for your 10th life", 
        "10th what?!", 
        "Every cat must serve cat duty by protecting the scary things from entering the kitty afterlife.",
        "This sounds dumb and poorly written.",
        "Yes, it does. Your assignment is to stop some of the scariest things known to cats.. CUCUMBERS, and CORNPOPPERS",
        "Meow, sounds fun"};
    // Start is called before the first frame update
    void Start()
    {
        dialog.text = dialogTexts[currentText];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentText++;
            if (currentText == dialogTexts.Length)
            {
                SceneManager.LoadScene("GamePlay");
                return;
            }
            if (currentText % 2 == 0)
            {
                image.sprite = catSprit;
            } else
            {
                image.sprite = unknwonSprite;
            }
            dialog.text = dialogTexts[currentText];
        }        
    }

    public void Play()
    {
        SceneManager.LoadScene("GamePlay");
    }
}

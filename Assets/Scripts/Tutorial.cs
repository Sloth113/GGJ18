using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public List<Sprite> tutorialImages;
    public Image imageOutput;
    [TextArea]
    public List<string> tutorialText;

    public Text textOutput;
    public GameObject pageForward;
    public GameObject pageBack;

    int currentPage = 0;



    public void ResetMenu()
    {
        currentPage = 0;
        imageOutput.sprite = tutorialImages[currentPage];
        textOutput.text = tutorialText[currentPage];
        pageForward.SetActive(true);
        pageBack.SetActive(false);
    }

    void Start()
    {
        ResetMenu();
    }


    public void PageForward() {
        currentPage += 1;
        imageOutput.sprite = tutorialImages[currentPage];
        textOutput.text = tutorialText[currentPage];
        if (currentPage >= tutorialImages.Count - 1)
        {
            pageForward.SetActive(false);
        }
        else
        {
            pageBack.SetActive(true);
        }
    }

    public void PageBack()
    {
        currentPage -= 1;
        imageOutput.sprite = tutorialImages[currentPage];
        textOutput.text = tutorialText[currentPage];
        if (currentPage <= 0)
        {
            pageBack.SetActive(false);
        }
        else
        {
            pageForward.SetActive(true);
        }
    }



}

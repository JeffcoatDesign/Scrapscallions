using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public bool _introDone = false;
    public bool _workshopDone = false;
    public bool _shopOpened = false;
    public bool _boughtPart = false;
    public bool _workshop2Done = false;
    public bool _botCustomized = false;
    public bool _scrapyardDone = false;
    public bool _shopOpenedAfterScrapyard = false;
    public bool _arenaTried = false;
    public bool _tutorialDone = false;
    public List<bool> flags;

    public TutorialPopup introPopup;
    public TutorialPopup workshopPopup;
    public TutorialPopup workshopInteriorPopup;
    public TutorialPopup shopPopup;
    public TutorialPopup shopInteriorPopup;
    public TutorialPopup workshop2Popup;
    public TutorialPopup workshop2InteriorPopup;
    public TutorialPopup scrapyardPopup;
    public TutorialPopup shop2Popup;
    public TutorialPopup shop2InteriorPopup;
    public TutorialPopup arenaPopup;
    public TutorialPopup finishPopup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void IntroFlag()
    {
        _introDone = true;
    }
    public void WorkshopFlag()
    {
        _workshopDone = true;
    }
    public void ShopOpenFlag()
    {
        _shopOpened = true;
    }
    public void BuyPartFlag()
    {
        _boughtPart = true;
    }

}

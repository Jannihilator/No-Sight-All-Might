using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Perk : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameManager gameManager;
    [SerializeField] Text perkText;
    int perkIndex;
    string[] perks = new string[]{"Gain Torch", "Longer Dash", "Bigger Light", "Longer Trace", "Extra Life", "Super Bounce (right click)"};

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch(perkIndex){
            case 0:
                playerController.torch.SetActive(true);
                gameManager.isTorch = true;
                break;
            case 1:
                playerController.dashPower*=1.5f;
                break;
            case 2:
                playerController.killLightSize*=2f;
                break;
            case 3:
                if(gameManager.traceEmitTime<3){
                    gameManager.traceEmitTime++;
                }
                break;
            case 4:
                playerController.Heal();
                break;
            case 5:
                playerController.superBounce++;
                break;

        }
        playerController.HidePerk();
    }

    public void SetPerk(int index){
        perkText.text = perks[index];
        perkIndex = index;
    }
}

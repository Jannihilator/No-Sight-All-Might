using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Banner : MonoBehaviour, IPointerClickHandler
{
    int level;
    string[] bosses = new string[]{"Ninsharok", "Quillterror", "Nightweaver", "Vinylnight", ""};
    [SerializeField] Text bossText;
    [SerializeField] Text dialogueText;
    bool clicked=false;
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
        if(level==1){
            if(!clicked){
                dialogueText.text = "Each fallen foe bestows vitality upon its conqueror";
                clicked=true;   
            }
            else{
                gameObject.SetActive(false);
                Time.timeScale = 1;
                clicked=false;   

            }
        }
        else if(level==3){
            if(!clicked){
                dialogueText.text = "Its hidden form is its downfall; trust your instincts in the dark ...";
                clicked=true;   
            }
            else{
                gameObject.SetActive(false);
                Time.timeScale = 1;
                clicked=false;   

            }
        }
        else if(level==5){
            SceneManager.LoadScene("Menu");
        }
        else{
            gameObject.SetActive(false);
            Time.timeScale = 1;

        }
    }

    public void StartDialogue(int _level){
        level = _level;
        bossText.text = bosses[level-1];
        switch(level){
            case 1:
                dialogueText.text = "Brute force doesn't do it all ...";
                break;
            case 2:
                dialogueText.text = "Beware the unseen edges in its shadow ...";
                break;
            case 3:
                dialogueText.text = "When you think it's gone, it's merely unseen ...";
                break;
            case 4:
                dialogueText.text = "As it moves to the beat, its might magnifies ...";
                break;
            case 5:
                dialogueText.text = "In darkness you thrived. True might lies within";
                break;
        }
        Time.timeScale = 0;
    }
}

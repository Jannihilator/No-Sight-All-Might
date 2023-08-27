using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Play : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject replayBtn;
    [SerializeField] GameManager gameManager;

    public void OnPointerClick(PointerEventData eventData){
        Time.timeScale = 1;
        pauseBtn.SetActive(true);
        replayBtn.SetActive(false);
        gameObject.SetActive(false);
        gameManager.bgm.Play();
    }
}

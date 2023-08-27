using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject replayBtn;
    [SerializeField] GameManager gameManager;

    public void OnPointerClick(PointerEventData eventData){
        Time.timeScale = 0;
        playBtn.SetActive(true);
        replayBtn.SetActive(true);
        gameObject.SetActive(false);
        gameManager.bgm.Pause();

    }
}

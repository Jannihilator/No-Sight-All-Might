using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer bg;
    [SerializeField] EnemyController enemy;
    [SerializeField] BossController boss;
    [SerializeField] Boss2Controller boss2;
    [SerializeField] Boss3Controller boss3;
    [SerializeField] Boss4Controller boss4;
    [SerializeField] GameObject lightPickup;
    [SerializeField] int enemyCount;
    [SerializeField] Image replayBtn;
    [SerializeField] GameObject dashMenu;
    [SerializeField] GameObject switchMenu;
    [SerializeField] float enemyTraceTime;
    [SerializeField] Slider healthBar;
    public AudioSource bgm;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] GameObject lifePanel;
    [SerializeField] GameObject disco;
    [SerializeField] Text levelText;
    [SerializeField] Image pauseBtn;
    [SerializeField] Image playBtn;
    [SerializeField] Image replayBtn2;
    public Banner banner;



    BossController spawnedBoss;
    Boss2Controller spawnedBoss2;
    Boss3Controller spawnedBoss3;
    Boss4Controller spawnedBoss4;
    PlayerController playerController;
    public int enemyLeft;
    bool isDark = false;
    public int level;
    int switchCount;
    float traceTimer=0;
    float gainDashTimer=0;
    float gainDashTime = 3;

    bool isBoss = false;
    public bool isTorch = false;
    public bool enterBoss = false;
    public float traceEmitTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        switchCount = 0;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        SpawnEnemy(5);
        SpawnPickUp();
        // InvokeRepeating("LightControlCaller", 3.24324f, 6.48648f);
    }

    // Update is called once per frame
    void Update()
    {
        if(enterBoss){
            EnterBoss();
            enterBoss = false;
        }
        //right click
        if(Input.GetKeyDown("space")){
            if(isBoss){
                return;
            }
            if(switchCount<3 || !isDark){
                ToggleLight(false);
            }
        }
        if(isDark){
            traceTimer+=Time.deltaTime;
            gainDashTimer+=Time.deltaTime;
            if(traceTimer>enemyTraceTime){
                ShowEnemyTrace();
                traceTimer=0;
            }
            if(gainDashTimer>gainDashTime){
                if(playerController.dashLeft<3 || (playerController.dashLeft<5 && isBoss)){
                    GainDash();
                }
                gainDashTimer=0;
            }
        }
    }

    void ShowEnemyTrace(){
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemies){
            StartCoroutine(enemy.GetComponent<EnemyController>().ShowTrace(traceEmitTime));
        }
    }
    void ShowPickUp(){
        var items = GameObject.FindGameObjectsWithTag("Light");
        if(isDark){
            foreach(var item in items){
                item.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else{
            foreach(var item in items){
                item.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }



    void GainDash(){
        dashMenu.transform.GetChild(playerController.dashLeft).gameObject.SetActive(true);
        playerController.dashLeft++;
    }


    void ToggleLight(bool isAuto){
        if(isDark){
            // playerController.Blink(false);
            bg.color = Color.white;
            playerController.isDark = false;
            for(int i=0;i<5;i++){
                dashMenu.transform.GetChild(i).gameObject.SetActive(false);
            }
            for(int i=0;i<3;i++){
                switchMenu.transform.GetChild(i).GetComponent<Image>().color = Color.black;
            }
            for(int i=0;i<5;i++){
                lifePanel.transform.GetChild(i).GetComponent<Image>().color = Color.black;
            }
            levelText.color = Color.black;
            pauseBtn.color = Color.black;
            playBtn.color = Color.black;
            replayBtn2.color = Color.black;
            isDark = false;
            if(isAuto){
                return;
            }
            switchMenu.transform.GetChild(switchCount).gameObject.SetActive(false);
            switchCount++;
        }
        else{
            // playerController.Blink(true);
            playerController.isDark = true;
            playerController.dashLeft = 3;
            for(int i=0;i<3;i++){
                dashMenu.transform.GetChild(i).gameObject.SetActive(true);
            }
            playerController.combo = 0;
            bg.color = Color.black;
            
            for(int i=0;i<3;i++){
                switchMenu.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            for(int i=0;i<5;i++){
                lifePanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            levelText.color = Color.white;
            pauseBtn.color = Color.white;
            playBtn.color = Color.white;
            replayBtn2.color = Color.white;
            isDark = true;
        }
        ShowPickUp();
    }


    void SpawnEnemy(int amount){
        enemyLeft = amount;
        for(int i=0;i<amount;i++){
            float x=Random.Range(-8.5f,8.5f);
            float y=Random.Range(-4.5f,4.5f);
            Instantiate(enemy, new Vector3(x,y,0), Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)))).Init((level-1)/2 + 1, this, playerController);
        }
    }
    
    void SpawnPickUp(){
        float x=Random.Range(-8.5f,8.5f);
        float y=Random.Range(-4.5f,4.5f);
        Instantiate(lightPickup, new Vector3(x,y,0), Quaternion.identity);
    }




    public IEnumerator SceneFlash(int time){
        if(level==4&&isBoss){
            
            for(int i=0;i<time;i++){
                disco.GetComponent<SpriteRenderer>().color = Color.black;
                yield return new WaitForSeconds(0.08f);
                
                disco.GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.08f);
            
        }   
        }
        else{
            for(int i=0;i<time;i++){
                bg.color = Color.white;
                yield return new WaitForSeconds(0.08f);
                bg.color = Color.black;
                yield return new WaitForSeconds(0.08f);
            }
        }
        if(!isDark){
            bg.color = Color.white;
        }
        
    }

    public void PlayerDied(){
        Time.timeScale = 0;
        replayBtn.gameObject.SetActive(true);
        if(isDark && !isBoss){
            ToggleLight(true);
        }
        bgm.Pause();
        menuMusic.Play();
        if(isBoss){
            GameObject.FindWithTag("Boss").GetComponent<AudioSource>().Pause();
        }
    }

    public void GameEnd(){
        banner.gameObject.SetActive(true);
        banner.StartDialogue(5);
    }

    public void NextLevel(){
        playerController.GetComponent<SpriteRenderer>().color = Color.black;
        playerController.Heal();
        if(isTorch){
            playerController.torch.SetActive(true);
        }
        level++;
        switchCount=0;
        gainDashTime = 3;
        if(level%2==1){
            SpawnEnemy(5);
        }
        else{
            SpawnEnemy(10);
        }
        SpawnPickUp();
        for(int i=0;i<3;i++){
            switchMenu.transform.GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(playerController.TurnInvicibleFor(1.0f));
        if(isDark){
            ToggleLight(true);
        }
        var items = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(var item in items){
            Destroy(item);
        }
        playerController.isBoss = false;
        bgm.Play();
        levelText.text = "LV" + level.ToString();
        isBoss = false;
    }

    public void UpdateDashMenu(int index){
        if(index < 5){
            dashMenu.transform.GetChild(index).gameObject.SetActive(false);
            if(!isBoss){
                gainDashTimer=0;
            }
        }
    }
    public void UpdateLifePanel(int index, bool heal){
        if(heal){
            lifePanel.transform.GetChild(index).gameObject.SetActive(true);
        }
        else{
            lifePanel.transform.GetChild(index).gameObject.SetActive(false);
        }
    }

    public void EnterBoss(){
        if(isTorch){    
            playerController.torch.SetActive(false);
        }
        GameObject lastEnemy = GameObject.FindWithTag("Enemy");
        if(!playerController.isChoosing){    
            banner.gameObject.SetActive(true);
            banner.StartDialogue(level);
        }
        else{
            playerController.isDialogue = true;
        }
        switch(level){
            case 1:
                spawnedBoss = Instantiate(boss, lastEnemy.transform.position, lastEnemy.transform.rotation);
                spawnedBoss.Init(this, healthBar);
                playerController.GetComponent<SpriteRenderer>().color = Color.white;

                break;
            case 2:
                spawnedBoss2 = Instantiate(boss2, lastEnemy.transform.position, lastEnemy.transform.rotation);
                spawnedBoss2.Init(this, healthBar);
                playerController.GetComponent<SpriteRenderer>().color = Color.white;

                break;
            case 3:
                spawnedBoss3 = Instantiate(boss3, lastEnemy.transform.position, lastEnemy.transform.rotation);
                spawnedBoss3.Init(this, healthBar);
                playerController.GetComponent<SpriteRenderer>().color = Color.white;

                break;
            case 4:
                spawnedBoss4 = Instantiate(boss4, lastEnemy.transform.position, lastEnemy.transform.rotation);
                spawnedBoss4.Init(this, healthBar, disco);
                playerController.GetComponent<SpriteRenderer>().color = Color.black;

                break;
        }
        gainDashTime = 3;
        Destroy(lastEnemy);
        playerController.dashLeft = 5;
        isBoss=true;
        for(int i=0;i<3;i++){
            switchMenu.transform.GetChild(i).gameObject.SetActive(false);
        }
        for(int i=0;i<5;i++){
            dashMenu.transform.GetChild(i).gameObject.SetActive(true);
        }
        bgm.Pause();
    }
}

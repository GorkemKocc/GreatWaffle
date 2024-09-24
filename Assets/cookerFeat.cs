using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cookerFeat : MonoBehaviour
{
    public float cookingTime = 0;
    //bool isCooked = false;
    public GameObject wafflePrefab;
    public GameObject waffleBatterPrefab;
    public strawberryBinControl strawberryBin;
    public chocolateBinControl chocolateBin;

    //public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GamePlay.isCookerEmpty)
        {
            cookingTime += Time.deltaTime;

            if (cookingTime > 3 && cookingTime < 8)
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
            }
            if (cookingTime > 8)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            }
        }
    }
    public void cooking()
    {
        if (!GamePlay.waffle)
        {
            /*if (!GamePlay.isCookerEmpty)
            {
                GamePlay.waffle = true;
                GamePlay.isCookerEmpty = true;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

                Vector3 spawnPosition = new Vector3(-0.33f, -1.05f, -3);
                GameObject spawnedWaffle = Instantiate(wafflePrefab, spawnPosition, Quaternion.identity);
                
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
                GamePlay.isCookerEmpty = false;
            }*/
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
            GamePlay.isCookerEmpty = false;
        }
    }

    void OnMouseDown()
    {
        if (!GamePlay.waffle)
        {
            if (!GamePlay.isCookerEmpty)
            {
                GamePlay.waffle = true;
                GamePlay.isCookerEmpty = true;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

                Vector3 spawnPosition = new Vector3(-0.33f, -1.05f, -3);
                GameObject spawnedWaffle = Instantiate(wafflePrefab, spawnPosition, Quaternion.identity);
                strawberryBin.setWaffleFeat(spawnedWaffle);
                chocolateBin.setWaffleFeat(spawnedWaffle);
                customerFeat.setWaffleFeat(spawnedWaffle);

                cookingTime = 0;
            }
            /*else
            {
                GamePlay.isCookerEmpty = false;
            }*/
        }
    }


}

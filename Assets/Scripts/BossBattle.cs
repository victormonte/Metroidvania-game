using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    public int threshold1, threshold2;

    public float activeTime, fadeoutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    public Animator anim;

    public Transform theBoss;

    public float timeBetweenShots1, timeBetweenShots2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    // Start is called before the first frame update
    void Start()
    {
        // Disable camera to be able to control it when boss time
        theCam = FindObjectOfType<CameraController>();
        theCam.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;
    }

    // Update is called once per frame
    void Update()
    {
        // Fix camera in Boss Battle
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        if (BossHealthController.instance.currentHealth > threshold1)
        {
            FirstPhase();

        }
        else
        {
            SecondPhase();
        }
    }

    private void FirstPhase()
    {
        if (activeCounter > 0)
        {
            activeCounter -= Time.deltaTime;
            if (activeCounter <= 0)
            {
                fadeCounter = fadeoutTime;
                anim.SetTrigger("vanish");
            }

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots1;

                Instantiate(bullet, shotPoint.position, Quaternion.identity);
            }
        }
        else if (fadeCounter > 0)
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0)
            {
                theBoss.gameObject.SetActive(false);
                inactiveCounter = inactiveTime;
            }
        }
        else if (inactiveCounter > 0)
        {
            inactiveCounter -= Time.deltaTime;
            if (inactiveCounter <= 0)
            {
                theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                theBoss.gameObject.SetActive(true);

                activeCounter = activeTime;

                shotCounter = timeBetweenShots1;
            }
        }
    }

    private void SecondPhase()
    {
        if (targetPoint == null)
        {
            targetPoint = theBoss;
            fadeCounter = fadeoutTime;
            anim.SetTrigger("vanish");
        }
        else
        {
            if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f)
            {
                theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(theBoss.position, targetPoint.position) <= .02f)
                {
                    fadeCounter = fadeoutTime;
                    anim.SetTrigger("vanish");
                }

                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    if(PlayerHealthController.instance.currentHealth > threshold2)
                    {
                        shotCounter = timeBetweenShots1;
                    }
                    else
                    {
                        shotCounter = timeBetweenShots2;
                    }

                    Instantiate(bullet, shotPoint.position, Quaternion.identity);
                }
            }
            else if (fadeCounter > 0)
            {
                fadeCounter -= Time.deltaTime;
                if (fadeCounter <= 0)
                {
                    theBoss.gameObject.SetActive(false);
                    inactiveCounter = inactiveTime;
                }
            }
            else if (inactiveCounter > 0)
            {
                inactiveCounter -= Time.deltaTime;
                if (inactiveCounter <= 0)
                {
                    theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                    // Loops are problematic for games, might cause bugs since its dynamic
                    int whileBreaker = 0;
                    do
                    {
                        if (whileBreaker >= 5) break;

                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        whileBreaker++;
                    } while (theBoss.position == targetPoint.position);

                    theBoss.gameObject.SetActive(true);

                    if (PlayerHealthController.instance.currentHealth > threshold2)
                    {
                        shotCounter = timeBetweenShots1;
                    }
                    else
                    {
                        shotCounter = timeBetweenShots2;
                    }
                }
            }
        }
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;

    private GameManager gameManager;
    public int pointValue;

    public ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(),
        RandomTorque(), ForceMode.Impulse);
        transform.position = new Vector3(Random.Range(-4, 4), -6);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            
            if (gameObject.CompareTag("Good"))
            {
                if (gameManager.pointOverride == 0)
                {
                    gameManager.UpdateScore(pointValue);
                }
                else
                {
                    gameManager.UpdateScore(30);
                }
            }
            
            if (gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }

            if (gameObject.CompareTag("Powerup"))
            {
                StartCoroutine(TemporarilyIncreasePointValue());
            }
        }
    }

    private void OnTriggerEnter()
    {
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad") || gameObject.CompareTag("Powerup"))
        {
            gameManager.GameOver();
        }
    }

    IEnumerator TemporarilyIncreasePointValue()
    {
        gameManager.pointOverride = 30;
        yield return new WaitForSeconds(5);
        gameManager.pointOverride = 0;
    }
}
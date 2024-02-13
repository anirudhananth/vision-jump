using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float spawnRate = 1f;
    private float enemySpeed = 7f;
    private bool canSpawn = true;
    private float cameraWidth, cameraHeight;
    private float multiplierX, multiplierY, x, y;

    [SerializeField] private GameObject enemyPrefab;

    private List<GameObject> spawnedEnemies;
    // Start is called before the first frame update
    void Start() {
        spawnedEnemies = new List<GameObject>();
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnTopDownEnemy());
    }

    private IEnumerator SpawnEnemy() {
        // WaitForSeconds seconds = new WaitForSeconds(spawnRate);

        while(canSpawn) {
            yield return new WaitForSeconds(Random.Range(2, 5) * 0.5f);

            multiplierX = Mathf.Pow(-1, Random.Range(0, 2));
            multiplierY = Mathf.Pow(-1, Random.Range(0, 2));
            x = Camera.main.transform.position.x + multiplierX * cameraWidth / 2f;
            y = Camera.main.transform.position.y + multiplierY * cameraHeight / 4f;
            GameObject enemy = Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity);

            Vector2 direction;  
            if(multiplierX == -1) {
                direction = (Camera.main.transform.position - enemy.transform.position + new Vector3(Random.Range(0,5), 3, 0)).normalized;
            } else {
                direction = (enemy.transform.position - Camera.main.transform.position - new Vector3(Random.Range(0,5), 3, 0)).normalized;
            }
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * enemySpeed * multiplierX * -1f, ForceMode2D.Impulse);
            spawnedEnemies.Add(enemy);
        }
    }

    private IEnumerator SpawnTopDownEnemy() {
        while(canSpawn) {
            yield return new WaitForSeconds(Random.Range(2, 4));

            float multX = Mathf.Pow(-1, Random.Range(0, 2));
            x = Camera.main.transform.position.x + multX * Random.Range(0, cameraWidth / 2f);
            y = Camera.main.transform.position.y + cameraHeight / 2f;
            GameObject enemy = Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity);

            Vector2 direction = Vector2.down + new Vector2(Random.Range(0, 2) * multX, Random.Range(0, 2) * -1f);
            direction.Normalize();
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * 5f, ForceMode2D.Impulse);
            spawnedEnemies.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update() {
        if(spawnedEnemies.Count > 0) CheckAndDestroyEnemies();
    }

    void CheckAndDestroyEnemies() {
        List<GameObject> enemiesToDestroy = new List<GameObject>();
        foreach (GameObject enemy in spawnedEnemies) {
            if(!enemy) continue;
            Vector2 direction = enemy.GetComponent<Rigidbody2D>().velocity.normalized;
            if(direction.x > 0 && CrossedRightSide(enemy)) {
                enemiesToDestroy.Add(enemy);
                Destroy(enemy);
            }

            if(direction.x < 0 && CrossedLeftSide(enemy)) {
                enemiesToDestroy.Add(enemy);
                Destroy(enemy);
            }

            if(direction.y < 0 && CrossedBottomSide(enemy)) {
                enemiesToDestroy.Add(enemy);
                Destroy(enemy);
            }

            if(!enemy) enemiesToDestroy.Add(enemy);
        }

        spawnedEnemies.RemoveAll(e => enemiesToDestroy.Contains(e));
    }

    bool IsInsideCameraView() {
        Camera mainCamera = Camera.main;
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    bool CrossedRightSide(GameObject go) {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        if(go.transform.position.x - renderer.bounds.size.x > Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x) {
            return true;
        }
        return false;
    }

    bool CrossedLeftSide(GameObject go) {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        if(go.transform.position.x + renderer.bounds.size.x < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x) {
            return true;
        }
        return false;
    }

    bool CrossedBottomSide(GameObject go) {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        if(go.transform.position.y + renderer.bounds.size.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y) {
            return true;
        }
        return false;
    }
}

using UnityEngine;
using DG.Tweening;

public class SpawnEnemyMove : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform targetPoint;
    [SerializeField] float moveTime = 3f;

    private void Start()
    {
        Game.Instance.onAllCarsMove += SpawnAndMove;
    }

    private void OnDisable()
    {
        Game.Instance.onAllCarsMove -= SpawnAndMove;
    }

    void SpawnAndMove()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        enemy.transform.DOLookAt(targetPoint.position, .1f)
            .SetLink(enemy);

        Tween moveTween = enemy.transform
            .DOMove(targetPoint.position, moveTime)
            .SetEase(Ease.Linear)
            .SetAutoKill(false)
            .SetLink(enemy);

        enemy.GetComponent<EnemyController>()
             .SetMoveTween(moveTween);
    }
}

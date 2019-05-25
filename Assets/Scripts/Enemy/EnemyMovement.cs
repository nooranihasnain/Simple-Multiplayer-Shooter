using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    UnityEngine.AI.NavMeshAgent nav;
    float enemyHealth = 100;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }


    void Update ()
    {
        if(enemyHealth > 0)
        {
            nav.SetDestination (player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }

    public void EnemyDamage(float Damage)
    {
        if(enemyHealth > 0)
        {
            enemyHealth -= Damage;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision Col)
    {
        if(Col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Singleplayer");
        }
    }
}

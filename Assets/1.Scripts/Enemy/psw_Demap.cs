using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psw_Demap : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPosition;
    private GameObject Player;
    CharacterController cc;
    BoxCollider boxCollider;

    void Start()
    {
        Player = GameObject.Find("Player");
        cc = Player.GetComponent<CharacterController>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {

    }

    IEnumerator SpawnObject(float delay)
    {
        yield return new WaitForSeconds(delay + 3f);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //&& cc.isGrounded == false)
        {
            StartCoroutine(Delay());
            StartCoroutine(HideRendererAfterDelay(4.5f));
            StartCoroutine(SpawnObject(4.5f));
        }
    }

    IEnumerator HideRendererAfterDelay(float delay)
    {
        yield return new WaitForSeconds(2f);
        this.GetComponent<MeshRenderer>().enabled = false;
        boxCollider.enabled = false;
    }
}

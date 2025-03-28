using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private int _respawnTimer;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BoxCollider _collider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ItemManager>().GetItem();
            StartCoroutine(RespawnBox());
        }
    }

    IEnumerator RespawnBox()
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;
        yield return new WaitForSeconds(_respawnTimer);
        _meshRenderer.enabled = true;
        _collider.enabled = true;
    }
}

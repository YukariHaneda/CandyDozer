using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyDestroyer : MonoBehaviour
{
    public CandyManager candyManager;
    public int reward;
    public GameObject effectPrefab;
    public Vector3 effectRotation;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Candy") {
            //指定数だけCandyストックを増やす
            candyManager.AddCandy(reward);
            
            //オブジェクトを削除
            Destroy(other.gameObject);

            if (effectPrefab != null) {
                //Candyのポジションにエフェクトを生成
                Instantiate (
                    effectPrefab,
                    other.transform.position,
                    Quaternion.Euler(effectRotation)
                );
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

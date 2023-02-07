using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject[] candyPrefabs; //キャンディプロパティの配列化
    public Transform candyParentTransform;
    public float shotForce;
    public float shotTorque;
    public float baseWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) Shot();
    }

    //キャンディプレハブからランダムに1つを選ぶ
    GameObject SampleCandy() {
        int index = Random.Range(0, candyPrefabs.Length);
        return candyPrefabs[index];
    }

    //発射位置の計算
    Vector3 GetInstantiatePosition() {
        //画面のサイズとInputの割合からキャンディ生成のポジションを計算
        float x = baseWidth * (Input.mousePosition.x / Screen.width) - (baseWidth / 2);
        return transform.position + new Vector3(x, 0, 0);
    }

    public void Shot() {
        //プレハブからCandyオブジェクトを生成
        GameObject candy = (GameObject)Instantiate (
            SampleCandy(),
            GetInstantiatePosition(),
            Quaternion.identity
            );

        //生成したCandyオブジェクトの親をcandyParentTransformに設定する
        candy.transform.parent = candyParentTransform;

        //candyオブジェクトのRigidbodyを取得し力と回転を加える
        Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();
        candyRigidBody.AddForce(transform.forward * shotForce);
        candyRigidBody.AddTorque(new Vector3(0, shotTorque, 0));
    }
}

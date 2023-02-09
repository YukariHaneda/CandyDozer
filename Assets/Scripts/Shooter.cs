using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    const int MaxShotPower = 5;
    const int RecoverySeconds = 3;

    int shotPower = MaxShotPower;
    AudioSource shotSound;
    
    public GameObject[] candyPrefabs; //キャンディプロパティの配列化
    public Transform candyParentTransform; //Candiesオブジェクトの子要素に配置するため親要素を準備
    public CandyManager candyManager;
    public float shotForce;
    public float shotTorque;
    public float baseWidth; //台の幅
    
    // Start is called before the first frame update
    void Start()
    {
        shotSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) Shot();
    }

    //キャンディプレハブからランダムに1つを選ぶ
    GameObject SampleCandy() {
        int index = Random.Range(0, candyPrefabs.Length); //Rangeは以上・未満
        return candyPrefabs[index];
    }

    //発射位置の計算
    Vector3 GetInstantiatePosition() {
        //画面のサイズとInputの割合からキャンディ生成のポジションを計算
        float x = baseWidth * (Input.mousePosition.x / Screen.width) - (baseWidth / 2); //-2.5~2.5
        //Input.mousePosition.x:クリックしたx座標
        //Screen.width

        // 画面に関する情報を試しに取得
        // Debug.Log(Input.mousePosition.x + ":" + Input.mousePosition.y);
        // Debug.Log(Screen.width);
        // Debug.Break();
        
        //左端からのクリックや右端からのクリックなど場所によって発射位置を算出
        return transform.position + new Vector3(x, 0, 0); //shooterのtransform.positionは真ん中
    }

    public void Shot() {
        //キャンディを生成できる条件外ならばShotしない
        if (candyManager.GetCandyAmount() <= 0) return;
        if ( shotPower <= 0) return;

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

        //Candyのストックを消費
        candyManager.ConsumeCandy();
        //ShotPowerを消費
        ConsumePower();

        //サウンドを再生
        shotSound.Play();
    }

    void OnGUI() {
        GUI.color = Color.black;

        //ShotPowerの残数を＋の数で表示
        string label = "";
        for ( int i = 0; i < shotPower; i++) label = label + "+";

        GUI.Label(new Rect(50, 65, 100, 30), label);
    }

    void ConsumePower() {
        //ShotPowerを消費すると同時に回復のカウントをスタート
        shotPower--;
        StartCoroutine(RecoverPower());
    }

    IEnumerator RecoverPower() {
        //一定秒数待った後にshotPowerを回復
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}

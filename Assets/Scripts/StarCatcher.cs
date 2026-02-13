using UnityEngine;
using System.Collections.Generic;

public class StarCatcher : MonoBehaviour
{
    private float spawnTimer = 0f;
    private int score = 0;
    private List<GameObject> stars = new List<GameObject>();
    
    public GameObject starPrefab;      // 星星预制体

    void Start()
    {
        Debug.Log("[星空捕手] 游戏开始！点击下落的星星得分");
        Debug.Log("[得分] 当前得分: 0");
    }

    void Update()
    {
        // 生成星星
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= 1f)
        {
            SpawnStar();
            spawnTimer = 0f;
        }

        // 移动星星
        MoveStars();

        // 检测点击
        CheckClick();
    }

    void SpawnStar()
    {
        // 在摄像机视野内随机生成（2D，Z=0）
        Vector3 viewportPos = new Vector3(
            Random.Range(0.2f, 0.8f),
            1.1f,
            0f
        );

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0f;  // 确保在2D平面

        // 从预制体创建星星
        GameObject star = Instantiate(starPrefab, worldPos, Quaternion.identity);
        star.name = "Star";

        stars.Add(star);
    }

    void MoveStars()
    {
        for (int i = stars.Count - 1; i >= 0; i--)
        {
            GameObject star = stars[i];

                        // 向下移动
            star.transform.position += Vector3.down * 3f * Time.deltaTime;

            // 旋转（绕Y轴）
            star.transform.Rotate(0, 100f * Time.deltaTime, 0);

            // 检测是否掉出屏幕
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(star.transform.position);

            if (viewportPos.y < -0.1f)
            {
                Destroy(star);
                stars.RemoveAt(i);
            }
        }
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = 0f;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = 0f;

            bool hit = false;

            for (int i = stars.Count - 1; i >= 0; i--)
            {
                GameObject star = stars[i];
                float distance = Vector3.Distance(mouseWorld, star.transform.position);

                if (distance < 0.5f)
                {
                    score++;
                    Debug.Log($"[捕获] +1分 当前得分: {score}");

                    Destroy(star);
                    stars.RemoveAt(i);
                    hit = true;
                    break;
                }
            }

            if (!hit)
            {
                score--;
                Debug.Log($"[点空] -1分 当前得分: {score}");
            }
        }
    }
}
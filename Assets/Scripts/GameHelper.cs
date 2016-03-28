using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHelper : MonoBehaviour {

    public Toggle toggleBob;
    public Toggle toggleSam;
    public Toggle toggleJohn;

    private GameObject player;

    public Text ScoreText;

    void Update()
    {
        if (player != null)
        ScoreText.text = "Score: " + player.GetComponent<PlayerHelper>().score;
    }

    public void Begin()
    {
        if (toggleBob.isOn)
            player = Instantiate(Resources.Load<GameObject>("Bob"), new Vector3(0, 0, -4), Quaternion.identity) as GameObject;
        else if (toggleSam.isOn)
            player = Instantiate(Resources.Load<GameObject>("Sam"), new Vector3(0, 0, -4), Quaternion.identity) as GameObject;
        else
            player = Instantiate(Resources.Load<GameObject>("John"), new Vector3(0, 0, -4), Quaternion.identity) as GameObject;

        
        transform.position = new Vector3(0.0f, 2.5f, -6.0f);            // Ставим камеру в нужную позицию.
        transform.LookAt(player.transform.position + transform.up);     // Методрм LookAt поворачиваем камеру к игроку.
        transform.SetParent(player.transform);  // Устанавливаем transform игрока родителем для transform'а камеры, xтобы она двигалась за ним.
        InvokeRepeating("CheckSpiders", 1.0f, Random.Range(4.0f, 7.0f));
    }


    /// Метод, отвечающий за spawn пауков.
    void CheckSpiders()
    {
        if (FindObjectsOfType<EnemyHelper>().Length < 8)
        {
            // Создаем паука в случайном месте на игровом поле.
            Instantiate(Resources.Load<GameObject>("Enemy"), new Vector3(Random.Range(-4, 5), 0, Random.Range(-4, 5)), Quaternion.identity);
        }
    }
}

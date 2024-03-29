using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuBullet : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector2(transform.position.x + Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).z) * Time.deltaTime * 15, transform.position.y + Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).z) * Time.deltaTime * 15);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("startgame"))
        {
            //SceneManager.LoadScene("waitingRoom");
            Object.FindObjectOfType<respawnPoint>().GetComponent<Animator>().SetBool("Slide", true);
            Destroy(gameObject);
        }
        if (collision.CompareTag("settings"))
        {

        }
        if (collision.CompareTag("quit"))
        {
            Application.Quit();
        }
    }
}

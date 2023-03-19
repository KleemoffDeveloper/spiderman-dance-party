using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public GameObject text;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);

        text.SetActive(false);

        yield return new WaitForSeconds (1);

        SceneManager.LoadScene(1);
    }
}

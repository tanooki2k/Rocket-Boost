using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Everything is looking good!");
                break;
            case "Finish":
                Debug.Log("You're all done, welcome to our country!");
                break;
            case "Fuel":
                Debug.Log("Why did you pick me up? I'm not in this game!");
                break;
            default:
                ReloadLevel();
                break;
        }
    }

    private static void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}

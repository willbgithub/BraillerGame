using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnBraillerTutorialButtonPress()
    {

    }
    public void OnPaperButtonPress()
    {
        SceneManager.LoadScene("PaperTest");
    }
    public void OnCopyGameButtonPress()
    {
        SceneManager.LoadScene("ReactionGame");
    }
}

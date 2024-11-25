using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class SceneSwitcher : MonoBehaviour
{
    public PlayableDirector playableDirector; // Timeline 控制器
    public string nextSceneName = "Editor";  // 要切換的場景名稱

    void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnTimelineFinished;
            playableDirector.Play();
        }
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnTimelineFinished;
        }
    }
}

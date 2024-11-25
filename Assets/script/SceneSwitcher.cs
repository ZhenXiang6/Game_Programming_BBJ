using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class SceneSwitcher : MonoBehaviour
{
    public PlayableDirector playableDirector; // Timeline ���
    public string nextSceneName = "Editor";  // �n�����������W��

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextScene;

    private void Awake()
    {
        Camera.main.aspect = 16f / 9f;
    }

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();

    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        vp.loopPointReached -= OnVideoEnd;
        SceneManager.LoadScene(nextScene);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Collider2D ChangeScene;
    public string NextStageScene;
    public Button StartScene;
    public string SceneA;
    public Button GameScene;
    public string SceneB;

    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private Slider _slider;

    public void MoveA()
    {
        SceneManager.LoadScene(SceneA);
    }

    public void MoveLoadNextSceneB()
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneA);
        AsyncOperation async1 = SceneManager.LoadSceneAsync(SceneB);
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
        while (!async1.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }

    public void MoveB()
    {
        SceneManager.LoadScene(SceneB);
    }

    ////�R���C�_�\�ɓ�������scene�ؑ�
    public void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneA);
    }
    public void MoveLoadNextSceneC()
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadNextScene());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�ȂǁA����̃^�O�����I�u�W�F�N�g���������ꍇ�ɃV�[�����ڍs
        if (other.CompareTag("Player"))  // "Player"�^�O�������I�u�W�F�N�g�Ŕ���
        {
            Debug.Log("Player entered trigger, switching scene.");
            SceneManager.LoadScene(NextStageScene);  // �V�[����؂�ւ���
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter");

        SceneManager.LoadScene(NextStageScene);
    }
}

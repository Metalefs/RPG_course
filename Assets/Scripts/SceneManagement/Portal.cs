using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            //yield return new WaitForSeconds(1f);
            //SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            //wrapper.Save();
            //SceneManager.LoadScene(sceneToLoad);
            //wrapper.Load();
            Destroy(gameObject);
        }
    }
}

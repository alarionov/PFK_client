using UnityEngine;
using UnityEngine.SceneManagement;

namespace WordBearers.Shared
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private string _scene;

        public void Load()
        {
            SceneManager.LoadScene(_scene, LoadSceneMode.Single);
        }
    }
}

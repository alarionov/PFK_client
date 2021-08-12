using UnityEngine;
using WordBearers.ProfileScene.Map;

namespace WordBearers.ProfileScene
{
    public class Preview : MonoBehaviour
    {
        [SerializeField] private Level[] _levels;
        [SerializeField] private GameObject[] _info;

        public void SetLevel(int index)
        {
            if (index > _levels.Length) return;

            for (int i = 0; i < _info.Length; ++i)
            {
                _info[i].SetActive(i == index);
            }

            gameObject.SetActive(true);
            transform.position = _levels[index].gameObject.transform.position;
        }
    }
}

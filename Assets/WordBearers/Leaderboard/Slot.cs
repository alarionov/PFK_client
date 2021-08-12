using UnityEngine;
using UnityEngine.UI;

namespace WordBearers.Leaderboard
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Text _positionText;
        [SerializeField] private Text _walletText;
        [SerializeField] private Text _scoreText;
        private void OnValidate()
        {
            _positionText.text = (transform.GetSiblingIndex() + 1 ).ToString();
        }

        public void SetValues(Record statsRecord)
        {
            _walletText.text = statsRecord.Player;
            _scoreText.text = statsRecord.Score.ToString();
        }
    }
}

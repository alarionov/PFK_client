using PFK;
using TMPro;
using UnityEngine;

namespace WordBearers.ProfileScene
{
    public class Wallet : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TMP_Text>()?.SetText($"Wallet: {PlayerState.GetInstance().Wallet}");
        }
    }
}

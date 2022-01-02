using UnityEngine;

namespace PFK.Acts.Act001
{
    public class StorylineProgress : MonoBehaviour
    {
        public void SetProgress(int progress)
        {
            PlayerState.GetInstance().UpdateProgress(progress);
        }
    }
}

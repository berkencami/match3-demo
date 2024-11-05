using _Dev.Scripts.Block;
using _Dev.Scripts.Data;
using _Dev.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Dev.Scripts.Goal
{
    public class GoalUI : MonoBehaviour
    {
        [SerializeField] private Image _goalImage;
        [SerializeField] private TextMeshProUGUI _goalCountText;
        [SerializeField] private GameObject _completeMark;
        [SerializeField] private BlockType _blockType;
        private int _goalCount;
        private Vector3 _initScale;

        public BlockType BlockType => _blockType;

        private void Awake()
        {
            _initScale = _goalImage.transform.localScale;
        }

        public void InitializeGoal(Sprite sprite, int count, BlockType type)
        {
            _goalImage.sprite = sprite;
            _goalCount = count;
            _goalCountText.text = "x" + _goalCount;
            _blockType = type;
            _completeMark.gameObject.SetActive(false);
        }

        public void DecreaseGoal()
        {
            if (_completeMark.activeSelf) return;
            _goalCount--;
            if (_goalCount <= 0)
            {
                _goalCountText.gameObject.SetActive(false);
                _completeMark.gameObject.SetActive(true);
            }

            _goalCountText.text = "x" + _goalCount;
            FXManager.Instance.PlayParticle(ParticleType.StarExplode, transform.position, Quaternion.identity);
            PunchTransformAnim(_initScale, -0.15f, 0.3f);
        }

        private void PunchTransformAnim(Vector3 initScale, float punchForce, float duration)
        {
            if (DOTween.IsTweening(_goalImage.transform))
                DOTween.Kill(_goalImage.transform);

            _goalImage.transform.localScale = initScale;
            _goalImage.transform.DOPunchScale(initScale * punchForce, duration, 1, 0f);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GoalUI : MonoBehaviour
{
    [SerializeField] private Image goalImage;
    [SerializeField] private TextMeshProUGUI goalCountText;
    [SerializeField] private GameObject completeMark;
    public BlockType blockType;
    private int goalCount;

    private Vector3 initScale;

    private void Awake()
    {
        initScale = goalImage.transform.localScale;
    }

    public void InitializeGoal(Sprite sprite, int count,BlockType type)
    {
        goalImage.sprite = sprite;
        goalCount = count;
        goalCountText.text = "x" + goalCount;
        blockType = type;
        completeMark.gameObject.SetActive(false);
    }

    public void DecreaseGoal()
    {
        if(completeMark.activeSelf) return;
        goalCount--;
        if (goalCount <= 0)
        {
            goalCountText.gameObject.SetActive(false);
            completeMark.gameObject.SetActive(true);
        }
        goalCountText.text =  "x" + goalCount;
        FXManager.instance.PlayParticle(ParticleType.StarExplode, transform.position, Quaternion.identity);
        PunchTransformAnim(initScale, -0.15f, 0.3f);
    }
    
    private void PunchTransformAnim( Vector3 initScale, float punchForce, float duration)
    {
        if (DOTween.IsTweening(goalImage.transform))
            DOTween.Kill(goalImage.transform);

        goalImage.transform.localScale = initScale;
        goalImage.transform.DOPunchScale(initScale * punchForce, duration, 1, 0f);
    }

}

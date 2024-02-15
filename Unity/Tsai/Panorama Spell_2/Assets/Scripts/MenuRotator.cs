using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuRotator : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform referceTransform;
    public float fixAngle = 20.0f;
    public float fixDistance = 0.5f;
    bool isMoving = false;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAngleShouldChange();
        CheckPositionShouldChange();
    }

    void CheckAngleShouldChange()
    {
        if (Mathf.Abs(this.transform.rotation.eulerAngles.y - referceTransform.rotation.eulerAngles.y) >= fixAngle)
        {
            Vector3 newDir = new Vector3(this.transform.rotation.eulerAngles.x, referceTransform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            this.transform.DORotate(newDir, 1.5f, RotateMode.Fast);
        }
        else
        {

        }
    }

    void CheckPositionShouldChange()
    {
        float dis = Vector2.Distance(new Vector2(referceTransform.position.x, referceTransform.position.z), new Vector2(this.rectTransform.position.x, this.rectTransform.position.z));
        if (dis > fixDistance && !isMoving)
        {
            Debug.LogWarning("ssdsd");
            Vector3 newPos = new Vector3(referceTransform.position.x, transform.position.y, referceTransform.position.z);
            isMoving = true;
            this.transform.DOMove(newPos, 1.5f).SetEase(Ease.InOutCubic).OnComplete(() => { isMoving = false; });

        }
    }
}

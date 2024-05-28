using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target_Trigger : MonoBehaviour
{
    public GameObject targetTriggerPopup; // 타겟 주변에 들어가면 뜨는 스낵바

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") {
            StartCoroutine(OpenPartSnackbar());
        }
    }
    private IEnumerator OpenPartSnackbar()
    {
        targetTriggerPopup.SetActive(true);
        targetTriggerPopup.GetComponent<FadeInOut>().FadeInAll();

        yield return new WaitForSeconds(2f);

        targetTriggerPopup.SetActive(false);
        targetTriggerPopup.GetComponent<FadeInOut>().FadeOutAll();
    }
}

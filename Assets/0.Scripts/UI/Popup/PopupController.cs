using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] ToastPopup toastPopup;
    [SerializeField] ListPopup ListPopup;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            toastPopup.PopupText("빛나는 방어구 AA를 습득하였습니다");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ListPopup.PopupText("빛나는 방어구 AA 1개");
        }
    }
}

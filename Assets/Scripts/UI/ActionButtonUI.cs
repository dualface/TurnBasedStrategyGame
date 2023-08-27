using TMPro;
using UnitAction;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private GameObject selectedBorder;

        public Button Button { get; private set; }

        public BaseAction Action { get; private set; }

        private void Awake()
        {
            selectedBorder.SetActive(false);
            Button = GetComponent<Button>();
        }

        public void SetAction(BaseAction action)
        {
            Action = action;
            text.text = action.ActionName.ToUpper();
        }

        public void UpdateSelectedVisual()
        {
            selectedBorder.SetActive(Action.IsSelected);
        }
    }
}

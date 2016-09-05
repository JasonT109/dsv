using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
    public enum NavigationMode { Auto = 0, Manual = 1 };
    [RequireComponent(typeof(EventSystem))]
    [AddComponentMenu("Event/Extensions/Tab Navigation")]

    public class TabNavigation : MonoBehaviour
    {
        private EventSystem _system;

        [Tooltip("The path to take when user is tabbing through ui components.")]
        public Selectable[] NavigationPath;

        [Tooltip("Use the default Unity navigation system or a manual fixed order using Navigation Path")]
        public NavigationMode NavigationMode;

        void Start()
        {
            _system = GetComponent<EventSystem>();
            if (_system == null)
            {
                Debug.LogError("Needs to be attached to the Event System component in the scene");
            }
        }

        public void Update()
        {
            var current = _system.currentSelectedGameObject;
            Selectable next = null;

            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
            {
                if (current != null)
                {
                    if (IsTabIgnored(current))
                        return;

                    next = current.GetComponent<Selectable>().FindSelectableOnUp();
                }
                else if (_system.firstSelectedGameObject != null)
                {
                    next = _system.firstSelectedGameObject.GetComponent<Selectable>();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (current != null)
                {
                    if (IsTabIgnored(current))
                        return;

                    next = current.GetComponent<Selectable>().FindSelectableOnDown();
                }
                else if (_system.firstSelectedGameObject != null)
                {
                    next = _system.firstSelectedGameObject.GetComponent<Selectable>();
                }
            }
            else if (NavigationMode == NavigationMode.Manual)
            {
                for (var i = 0; i < NavigationPath.Length; i++)
                {
                    if (current != NavigationPath[i].gameObject) continue;


                    next = i == (NavigationPath.Length - 1) ? NavigationPath[0] : NavigationPath[i + 1];

                    break;
                }
            }
            else if (current == null && _system.firstSelectedGameObject != null)
            {
                next = _system.firstSelectedGameObject.GetComponent<Selectable>();
            }

            selectGameObject(next);
        }

        private bool IsTabIgnored(GameObject go)
        {
            var behaviour = _system.currentSelectedGameObject.GetComponent<TabNavigationOptions>();
            return (behaviour && behaviour.Behaviour == TabNavigationOptions.TabBehaviour.IgnoreTab);
        }

        private void selectGameObject(Selectable selectable)
        {
            if (selectable != null)
            {
                InputField inputfield = selectable.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(_system));  //if it's an input field, also set the text caret

                _system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(_system));
            }
        }
    }
}

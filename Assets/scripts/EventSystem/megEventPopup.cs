using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Meg.Networking;

namespace Meg.EventSystem
{

    [System.Serializable]
    public class megEventPopup : megEvent
    {

        // Constants
        // ------------------------------------------------------------

        /** Default error message. */
        public const string DefaultMessage = "";


        // Properties
        // ------------------------------------------------------------

        public popupData.Type Type = popupData.Type.Info;
        public string Title = "";
        public string Message = DefaultMessage;
        public string Target = "";
        public Vector3 Position = Vector3.zero;
        public Vector2 Size = Vector2.zero;
        public popupData.Icon Icon = popupData.Icon.Exclamation;
        public Color Color = Color.white;

        /** Popup configuration data. */
        public popupData.Popup Popup
        {
            get
            {
                return new popupData.Popup
                {
                    Type = Type,
                    Title = Title,
                    Message = Message,
                    Target = Target,
                    Position = Position,
                    Size = Size,
                    Icon = Icon,
                    Color = Color
                };
            }
        }

        /** Determines if a popup is active for this event. */
        public bool HasPopup
            { get { return serverUtils.PopupData.IsPopupActive(Popup); } }


        // Lifecycle
        // ------------------------------------------------------------

        /** Constructor for a sonar event. */
        public megEventPopup(megEventGroup group = null)
            : base(megEventType.Popup, group) { }


        // Public Methods
        // ------------------------------------------------------------

        /** Execute this event's effect, regardless of timing. */
        public override void Execute()
        {
            base.Execute();
            file.PostTogglePopup(Popup);
        }

        /** Execute trigger effect. */
        public override void TriggerExecute()
        {
            // Toggle popups when triggered.
            file.PostTogglePopup(Popup);
        }


        // Load / Save
        // ------------------------------------------------------------

        /** Save state to JSON. */
        public override JSONObject Save()
        {
            var json = base.Save();
            json.AddField("Type", (int) Type);
            json.AddField("Title", Title);
            json.AddField("Message", Message);
            json.AddField("Target", Target);
            json.AddField("Position", Position);
            json.AddField("Size", Size);
            json.AddField("Icon", (int) Icon);
            json.AddField("Color", Color);

            return json;
        }

        /** Load movement state from JSON. */
        public override void Load(JSONObject json)
        {
            base.Load(json);

            var type = 0;
            json.GetField(ref type, "Type");
            Type = (popupData.Type) type;

            json.GetField(ref Title, "Title");
            json.GetField(ref Message, "Message");
            json.GetField(ref Target, "Target");
            json.GetField(ref Position, "Position");
            json.GetField(ref Size, "Size");
            json.GetField(ref Color, "Color");

            var icon = 0;
            json.GetField(ref icon, "Icon");
            Icon = (popupData.Icon) icon;
        }

        /** String representation. */
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(triggerLabel))
                return triggerLabel;

            var typeName = Enum.GetName(typeof(popupData.Type), Type);
            var result = string.Format("Popup {0}", typeName);
            if (!string.IsNullOrEmpty(Title))
                result += string.Format(" '{0}'", Title);
            if (!string.IsNullOrEmpty(Target))
                result += string.Format(" @ '{0}'", Target);

            return result;
        }


        // Protected Methods
        // ------------------------------------------------------------

        /** Start this event. */
        protected override void Start()
        {
            file.PostAddPopup(Popup);
        }

        /** Update this event internally. */
        protected override void Update(float t, float dt)
        {
            // Check if final value has been reached.
            if (timeFraction < 1 || completed)
                return;

            // Event is complete.
            completed = true;

            // If event has a duration, hide the popup when it completes.
            if (completeTime > 0)
                file.PostRemovePopup(Popup);
        }

        /** Rewind this event. */
        protected override void Rewind()
        {
        }

        /** Stop this event. */
        protected override void Stop()
        {
        }

    }

}

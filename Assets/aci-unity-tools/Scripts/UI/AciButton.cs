// <copyright file=AciButton.cs/>
// <copyright>
//   Copyright (c) 2018, Affective & Cognitive Institute
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software andassociated documentation files
//   (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify,
//   merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//   OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//   LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
//   IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// </copyright>
// <license>MIT License</license>
// <main contributors>
//   Moritz Umfahrer
// </main contributors>
// <co-contributors/>
// <patent information/>
// <date>07/12/2018 05:59</date>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aci.Unity.UI
{
    public class AciButton : MonoBehaviour
    {
        private RectTransform buttonlayoutControl;

        public bool FixedWidth;

        public Image IconContainer;

        private RectTransform   iconLayoutControl;
        public  TextMeshProUGUI TextContainer;
        private RectTransform   textLayoutControl;

        public Sprite Icon
        {
            get { return IconContainer?.sprite; }
            set
            {
                if (IconContainer == null)
                    return;
                IconContainer.sprite = value;
                RecalculateLayout();
            }
        }

        public string Text
        {
            get { return TextContainer?.text; }
            set
            {
                if (TextContainer == null)
                    return;
                TextContainer.text = value;
                RecalculateLayout();
            }
        }

        // Use this for initialization
        private void Awake()
        {
            iconLayoutControl = IconContainer?.GetComponent<RectTransform>();
            textLayoutControl = TextContainer?.GetComponent<RectTransform>();
            buttonlayoutControl = transform as RectTransform;
            if (iconLayoutControl == null || iconLayoutControl == null || buttonlayoutControl == null)
                return;
            RecalculateLayout();
        }

        /// <summary>
        ///     Sets display states of the button.
        /// </summary>
        /// <param name="displayIcon">True if icon should be displayed, False otherwise.</param>
        /// <param name="displayText">True if text should be displayed, False otherwise.</param>
        public void SetElementStates(bool displayIcon, bool displayText)
        {
            IconContainer?.gameObject.SetActive(displayIcon);
            TextContainer?.gameObject.SetActive(displayText);
        }

        /// <summary>
        ///     Calculates the button size and layout
        /// </summary>
        public void RecalculateLayout()
        {
            //check if everything is set
            if (iconLayoutControl == null || iconLayoutControl == null || buttonlayoutControl == null)
                return;
            //check if fixed size
            if (!FixedWidth)
            {
                textLayoutControl.sizeDelta = new Vector2(TextContainer.preferredWidth, textLayoutControl.sizeDelta.y);
                iconLayoutControl.sizeDelta = new Vector2(IconContainer?.sprite == null ? 0.0f : 30f, 30f);
                buttonlayoutControl.sizeDelta =
                    new Vector2(40 + iconLayoutControl.rect.width + textLayoutControl.rect.width,
                                buttonlayoutControl.rect.height);
                TextContainer.enableAutoSizing = false;
            }
            else
            {
                //get total width - margins
                float width = -30 + buttonlayoutControl.rect.width;
                //set icon layout and subtract from remaining width
                iconLayoutControl.sizeDelta =
                    new Vector2(IconContainer?.sprite == null ? 0 : iconLayoutControl.sizeDelta.y,
                                iconLayoutControl.sizeDelta.y);
                width -= iconLayoutControl.rect.width;
                //set text width to remaining width
                textLayoutControl.sizeDelta = new Vector2(width, textLayoutControl.sizeDelta.y);
                TextContainer.enableAutoSizing = true;
                TextContainer.enableWordWrapping = true;
            }

            //check if icon or text only
            if (!IconContainer.IsActive())
            {
                textLayoutControl.anchorMin = new Vector2(0.5f, 0.5f);
                textLayoutControl.anchorMax = new Vector2(0.5f, 0.5f);
                textLayoutControl.pivot = new Vector2(0.5f, 0.5f);
                textLayoutControl.anchoredPosition = new Vector2(0, 0);
                TextContainer.alignment = TextAlignmentOptions.Center;
            }
            else
            {
                textLayoutControl.anchorMin = new Vector2(1, 0.5f);
                textLayoutControl.anchorMax = new Vector2(1, 0.5f);
                textLayoutControl.pivot = new Vector2(1, 0.5f);
                textLayoutControl.anchoredPosition = new Vector2(-10, 0);
                TextContainer.alignment = TextAlignmentOptions.Right;
            }

            if (!TextContainer.IsActive())
            {
                iconLayoutControl.anchorMin = new Vector2(0.5f, 0.5f);
                iconLayoutControl.anchorMax = new Vector2(0.5f, 0.5f);
                iconLayoutControl.pivot = new Vector2(0.5f, 0.5f);
                iconLayoutControl.anchoredPosition = new Vector2(0, 0);
            }
            else
            {
                iconLayoutControl.anchorMin = new Vector2(0, 0.5f);
                iconLayoutControl.anchorMax = new Vector2(0, 0.5f);
                iconLayoutControl.pivot = new Vector2(0, 0.5f);
                iconLayoutControl.anchoredPosition = new Vector2(0, 0);
            }
        }
    }
} // namespace Aci.Unity.UserInterface
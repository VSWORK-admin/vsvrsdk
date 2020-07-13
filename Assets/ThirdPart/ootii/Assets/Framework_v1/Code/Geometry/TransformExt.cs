﻿using System.Collections.Generic;
using UnityEngine;

namespace com.ootii.Geometry
{
    /// <summary>
    /// Extension for the standard Transform that allows us to add functions
    /// </summary>
    public static class TransformExt
    {
        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static Transform FindTransform(this Transform rThis, HumanBodyBones rBone)
        {
            Animator lAnimator = rThis.gameObject.GetComponent<Animator>();
            if (lAnimator != null) { return lAnimator.GetBoneTransform(rBone); }

            return null;
        }

        /// <summary>
        /// Recursively searches for a bone given the name and returns it if found
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns>Transform of the bone or null</returns>
        public static Transform FindTransform(this Transform rThis, string rName)
        {
            return FindChildTransform(rThis, rName);
        }

        /// <summary>
        /// Recursively search for a bone that matches the specifie name
        /// </summary>
        /// <param name="rParent">Parent to search through</param>
        /// <param name="rBoneName">Bone to find</param>
        /// <returns></returns>
        public static Transform FindChildTransform(Transform rParent, string rName)
        {
            string lParentName = rParent.name;

            // We found it. Get out fast
            if (string.Compare(lParentName, rName, true) == 0) { return rParent; }

            // Handle the case where the bone name is nested in a namespace
            int lIndex = lParentName.IndexOf(':');
            if (lIndex >= 0)
            {
                lParentName = lParentName.Substring(lIndex + 1);
                if (string.Compare(lParentName, rName, true) == 0) { return rParent; }
            }

            // Since we didn't find it, check the children
            for (int i = 0; i < rParent.transform.childCount; i++)
            {
                Transform lTransform = FindChildTransform(rParent.transform.GetChild(i), rName);
                if (lTransform != null) { return lTransform; }
            }

            // Return nothing
            return null;
        }

        /// <summary>
        /// Retrieves the chain of transforms that start at the name and goes down the first child
        /// </summary>
        /// <param name="rParent"></param>
        /// <param name="rName"></param>
        /// <param name="rList"></param>
        public static void FindTransformChain(Transform rParent, string rName, ref List<Transform> rList)
        {
            Transform lTransform = rParent.FindTransform(rName);

            rList.Clear();
            while (lTransform != null)
            {
                rList.Add(lTransform);
                if (lTransform.childCount > 0)
                {
                    lTransform = lTransform.GetChild(0);
                }
                else
                {
                    lTransform = null;
                }
            }
        }

        // CDL 07/06/2018 - added methods for resetting a Transform or RectTransform
        /// <summary>
        /// Reset the a Transform's local position, rotation, and scale to default values (0 for position and rotation; 1 for scale)
        /// </summary>
        /// <param name="rTransform"></param>
        public static void Reset(this Transform rTransform)
        {
            if (rTransform == null) { return; }

            rTransform.localPosition = Vector3.zero;
            rTransform.localRotation = Quaternion.identity;
            rTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// Reset a RectTransform's anchor and local position, rotation, and scale. This is usually required when
        /// adding UI elements as children of a RectTransform at runtime.
        /// </summary>
        /// <param name="rTransform"></param>
        public static void ResetRect(this Transform rTransform)
        {
            if (rTransform == null) { return; }

            rTransform.localPosition = Vector3.zero;
            rTransform.localRotation = Quaternion.identity;
            rTransform.localScale = Vector3.one;

            RectTransform lRectTransform = rTransform.GetComponent<RectTransform>();
            if (lRectTransform != null)
            {
                lRectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}

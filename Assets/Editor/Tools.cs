using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/*To create a hotkey you can use the following special characters: % (ctrl on Windows, cmd on OS X), # (shift), & (alt). 
If no special modifier key combinations are required the key can be given after an underscore. For example to create a menu with hotkey shift-alt-g use
"MyMenu/Do Something #&g". To create a menu with hotkey g and no key modifiers pressed use "MyMenu/Do Something _g".
Some special keyboard keys are supported as hotkeys, for example "#LEFT" would map to shift-left.The keys supported 
like this are: LEFT, RIGHT, UP, DOWN, F1..F12, HOME, END, PGUP, PGDN.
*/

[ExecuteInEditMode]
public static class Tools
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    public enum AnchorType
    {
        Strech,
        Top,
        Middle,
        Bottom,
        Left,
        Center,
        Right,
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public static float moveFactor = 0.5f;
    private static int waitCounter = 5;
    private static Direction selectedDirection;

    #region CREATE

    [MenuItem("Tools/Create/SpriteRenderer Child %`", false)]
    public static void CreateSpriteRendererChild()
    {
        SpriteRenderer image = CreateObjectToSelected<SpriteRenderer>();
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/SpriteRenderer Sibling %&`", false)]
    public static void CreateSpriteRendererSibling()
    {
        SpriteRenderer image = CreateObjectToSelected<SpriteRenderer>(true);
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/Image Child %1", false)]
    public static void CreateChildImage()
    {
        Image image = CreateObjectToSelected<Image>();
        image.raycastTarget = false;
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/Image Sibling %F1", false)]
    public static void CreateSiblingImage()
    {
        Image image = CreateObjectToSelected<Image>(true);
        image.raycastTarget = false;
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/RawImage Child %2", false)]
    public static void CreateChildRawImage()
    {
        RawImage image = CreateObjectToSelected<RawImage>();
        image.raycastTarget = false;
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/RawImage Sibling %F2", false)]
    public static void CreateSiblingRawImage()
    {
        RawImage rawImage = CreateObjectToSelected<RawImage>(true);
        rawImage.raycastTarget = false;
        Selection.activeGameObject = rawImage.gameObject;
    }

    [MenuItem("Tools/Create/Text Child %3", false)]
    public static void CreateChildText()
    {
        Text text = CreateObjectToSelected<Text>();
        text.raycastTarget = false;
        text.fontSize = 18;
        text.resizeTextMinSize = 12;
        text.resizeTextMaxSize = text.fontSize;
        text.rectTransform.sizeDelta = new Vector2(50, text.fontSize);
        text.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/Roboto-Regular.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.resizeTextForBestFit = true;
        text.supportRichText = false;
        Selection.activeGameObject = text.gameObject;
    }

    [MenuItem("Tools/Create/Text Sibling %F3", false)]
    public static void CreateSiblingText()
    {
        Text text = CreateObjectToSelected<Text>(true);
        text.raycastTarget = false;
        text.fontSize = 18;
        text.resizeTextMinSize = 12;
        text.resizeTextMaxSize = text.fontSize;
        text.rectTransform.sizeDelta = new Vector2(50, text.fontSize);
        text.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/Roboto-Regular.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.resizeTextForBestFit = true;
        text.supportRichText = false;
        Selection.activeGameObject = text.gameObject;
    }

    [MenuItem("Tools/Create/Button Child %4", false)]
    public static void CreateChildButton()
    {
        Image image = CreateObjectToSelected<Image>();
        image.gameObject.name = "Button";
        Button button = image.gameObject.AddMissingComponent<Button>();
        button.interactable = true;
        button.transition = Selectable.Transition.None;
        Navigation navigation = new Navigation
        {
            mode = Navigation.Mode.None
        };

        button.navigation = navigation;

        image.raycastTarget = true;
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/Button Sibling %F4", false)]
    public static void CreateSiblingButton()
    {
        Image image = CreateObjectToSelected<Image>(true);
        image.gameObject.name = "Button";
        Button button = image.gameObject.AddMissingComponent<Button>();
        button.interactable = true;
        button.transition = Selectable.Transition.None;
        Navigation navigation = new Navigation
        {
            mode = Navigation.Mode.None
        };

        button.navigation = navigation;

        image.raycastTarget = true;
        Selection.activeGameObject = image.gameObject;
    }

    [MenuItem("Tools/Create/Scroll Masked Rect Child %5", false)]
    public static void CreateChildScrollRect()
    {
        ScrollRect view = CreateObjectToSelected<ScrollRect>();
        CreateViewItems(view);
    }


    [MenuItem("Tools/Create/Scroll Masked Rect Sibling %F5", false)]
    public static void CreateSiblingScrollRect()
    {
        ScrollRect view = CreateObjectToSelected<ScrollRect>(true);
        CreateViewItems(view);
    }

    [MenuItem("Tools/Create/Grid Child %6", false)]
    public static void CreateChildGrid()
    {
        VerticalLayoutGroup grid = CreateObjectToSelected<VerticalLayoutGroup>();
        grid.gameObject.AddMissingComponent<ContentSizeFitter>();
        Selection.activeGameObject = grid.gameObject;
        LayoutElement child = CreateObjectToSelected<LayoutElement>();
        Selection.activeGameObject = child.gameObject;
    }

    [MenuItem("Tools/Create/Grid Sibling %F6", false)]
    public static void CreateSiblingGrid()
    {
        ScrollRect grid = CreateObjectToSelected<ScrollRect>(true);
        grid.gameObject.AddMissingComponent<ContentSizeFitter>();
        Selection.activeGameObject = grid.gameObject;
        LayoutElement child = CreateObjectToSelected<LayoutElement>();
        Selection.activeGameObject = child.gameObject;
    }

    private static T CreateObjectToSelected<T>(bool wantSibling = false) where T : Component
    {
        GameObject child = new GameObject(typeof(T).Name);
        Undo.RegisterCreatedObjectUndo(child, "Create GameObject");

        T component = child.AddMissingComponent<T>();
        child.transform.parent = wantSibling ? Selection.activeGameObject.transform.parent : Selection.activeGameObject.transform;
        RectTransform rect = child.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.localPosition = Vector3.zero;
            rect.localRotation = Quaternion.Euler(Vector3.zero);
            rect.localScale = Vector3.one;
        }

        return component;
    }

    #endregion

    #region UI

    [MenuItem("Tools/UI/Image Native Size %&io")]
    public static void SetNativeSizeImage()
    {
        GameObject[] objects = TakeSelectionGameObjects(Selection.gameObjects);
        for (int i = 0; i < objects.Length; i++)
        {
            Image image = objects[i].GetComponent<Image>();
            image.SetNativeSize();
        }
    }

    [MenuItem("Tools/UI/Disable Raycast Without Button")]
    public static void SetRectTransform()
    {
        GameObject[] objects = TakeSelectionGameObjects(Selection.gameObjects);
        for (int i = 0; i < objects.Length; i++)
        {
            Button button = objects[i].GetComponent<Button>();
            MaskableGraphic mask = objects[i].GetComponent<MaskableGraphic>();

            if (mask != null)
            {
                mask.raycastTarget = button != null;
            }
        }
    }

    [MenuItem("Tools/UI/Аdd Button to existing Image %&ib")]
    public static void AddButtonAndImage()
    {
        GameObject[] objects = TakeSelectionGameObjects(Selection.gameObjects);

        for (int i = 0; i < objects.Length; i++)
        {
            Image image = AddMissingComponent<Image>(objects[i]);
            Button button = AddMissingComponent<Button>(objects[i]);

            button.interactable = true;
            button.transition = Selectable.Transition.None;
            Navigation navigation = new Navigation
            {
                mode = Navigation.Mode.None
            };

            button.navigation = navigation;

            image.raycastTarget = true;
        }
    }

    [MenuItem("Tools/UI/Anchors/Strech %[]")]
    static void FixAnchorsStrech()
    {
        GameObject o = Selection.activeGameObject;
        Undo.RecordObject(o, "Anchor " + o);

        if (o != null && o.GetComponent<RectTransform>() != null)
        {
            RectTransform r = o.GetComponent<RectTransform>();
            RectTransform p = o.transform.parent.GetComponent<RectTransform>();

            Vector2 offsetMin = r.offsetMin;
            Vector2 offsetMax = r.offsetMax;
            Vector2 _anchorMin = r.anchorMin;
            Vector2 _anchorMax = r.anchorMax;

            float parent_width = p.rect.width;
            float parent_height = p.rect.height;

            Vector2 anchorMin = new Vector2(_anchorMin.x + offsetMin.x / parent_width,
                                        _anchorMin.y + (offsetMin.y / parent_height));
            Vector2 anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
                                        _anchorMax.y + (offsetMax.y / parent_height));

            r.anchorMin = anchorMin;
            r.anchorMax = anchorMax;

            r.offsetMin = Vector2.zero;
            r.offsetMax = Vector2.one;
            r.pivot = new Vector2(0.5f, 0.5f);
        }
    }

    [MenuItem("Tools/UI/Anchors/Top")]
    static void FixAnchorsTop()
    {
        FixAnchors(AnchorType.Top);
    }

    [MenuItem("Tools/UI/Anchors/Middle")]
    static void FixAnchorsMiddle()
    {
        FixAnchors(AnchorType.Middle);
    }

    [MenuItem("Tools/UI/Anchors/Bottom")]
    static void FixAnchorsBottom()
    {
        FixAnchors(AnchorType.Bottom);
    }

    [MenuItem("Tools/UI/Anchors/Left")]
    static void FixAnchorsLeft()
    {
        FixAnchors(AnchorType.Left);
    }

    [MenuItem("Tools/UI/Anchors/Center")]
    static void FixAnchorsCenter()
    {
        FixAnchors(AnchorType.Center);
    }

    [MenuItem("Tools/UI/Anchors/Right")]
    static void FixAnchorsRight()
    {
        FixAnchors(AnchorType.Right);
    }

    [MenuItem("Tools/UI/Anchors/Top Left")]
    static void FixAnchorsTopLeft()
    {
        FixAnchors(AnchorType.TopLeft);
    }

    [MenuItem("Tools/UI/Anchors/Top Center")]
    static void FixAnchorsTopCenter()
    {
        FixAnchors(AnchorType.TopCenter);
    }

    [MenuItem("Tools/UI/Anchors/Top Right")]
    static void FixAnchorsTopRight()
    {
        FixAnchors(AnchorType.TopRight);
    }

    [MenuItem("Tools/UI/Anchors/Middle Left")]
    static void FixAnchorsMiddleLeft()
    {
        FixAnchors(AnchorType.MiddleLeft);
    }

    [MenuItem("Tools/UI/Anchors/Middle Center")]
    static void FixAnchorsMiddleCenter()
    {
        FixAnchors(AnchorType.MiddleCenter);
    }

    [MenuItem("Tools/UI/Anchors/Middle Right")]
    static void FixAnchorsMiddleRight()
    {
        FixAnchors(AnchorType.MiddleRight);
    }

    [MenuItem("Tools/UI/Anchors/Bottom Left")]
    static void FixAnchorsBottomLeft()
    {
        FixAnchors(AnchorType.BottomLeft);
    }

    [MenuItem("Tools/UI/Anchors/Bottom Center")]
    static void FixAnchorsBottomCenter()
    {
        FixAnchors(AnchorType.BottomCenter);
    }

    [MenuItem("Tools/UI/Anchors/Bottom Right")]
    static void FixAnchorsBottomRight()
    {
        FixAnchors(AnchorType.BottomRight);
    }

    static void FixAnchors(AnchorType type, GameObject[] gameObjs = null)
    {
        Undo.RecordObjects(gameObjs, "Anchors objects");

        if (gameObjs == null || gameObjs.Length == 0)
        {
            gameObjs = Selection.gameObjects;
        }

        foreach (GameObject go in gameObjs)
        {
            RectTransform rectTransform = go.GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                continue;
            }

            Vector3[] corners = new Vector3[4];
            Vector3[] parentCorners = new Vector3[4];

            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            if (parentRectTransform == null)
            {
                Debug.Log(string.Format("Selected element {0} has no RectTransform Component.GameObject is skipped", go.name));
                continue;
            }

            bool hasAspectRatio = false;
            if (go.GetComponent<AspectRatioFitter>() != null && go.GetComponent<AspectRatioFitter>().enabled == true)
            {
                hasAspectRatio = true;
                go.GetComponent<AspectRatioFitter>().enabled = false;
            }

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();

            rectTransform.GetWorldCorners(corners);
            parentRectTransform.GetWorldCorners(parentCorners);

            float rectUIWidth = canvas.transform.localScale.x * rectTransform.rect.width;
            float rectUIHeight = canvas.transform.localScale.y * rectTransform.rect.height;
            float parentUIWidth = parentRectTransform.rect.width * canvas.transform.localScale.x;
            float parentUIHeight = parentRectTransform.rect.height * canvas.transform.localScale.y;

            Vector2 parentZeroCorner = parentCorners[0];
            float parent_x = parentZeroCorner.x + ((parentCorners[3].x - parentZeroCorner.x) / 2);
            float parent_y = parentZeroCorner.y + ((parentCorners[1].y - parentZeroCorner.y) / 2);
            float parentXMin = parent_x - (parentUIWidth / 2);
            float parentYMin = parent_y - (parentUIHeight / 2);

            float minX = corners[0].x;
            float maxY = corners[3].y;

            Vector2 anchorMinPoint = new Vector2(minX - parentXMin, maxY - parentYMin);
            Vector2 anchorMaxPoint = Vector2.zero;
            Vector2 offsetMin = Vector2.zero;
            Vector2 offsetMax = Vector2.zero;

            switch (type)
            {
                case AnchorType.Strech:
                    anchorMaxPoint.x = anchorMinPoint.x + rectUIWidth;
                    anchorMaxPoint.y = anchorMinPoint.y + rectUIHeight;
                    break;

                case AnchorType.Top:
                    anchorMinPoint.y += rectUIHeight;
                    anchorMaxPoint.x = anchorMinPoint.x + rectUIWidth;
                    anchorMaxPoint.y = anchorMinPoint.y;
                    offsetMax = Vector2.zero;
                    offsetMin = new Vector2(0f, -rectTransform.rect.height);
                    break;

                case AnchorType.Middle:
                    anchorMinPoint.y += rectUIHeight / 2;
                    anchorMaxPoint.x = anchorMinPoint.x + rectUIWidth;
                    anchorMaxPoint.y = anchorMinPoint.y;
                    offsetMax = new Vector2(0f, +rectTransform.rect.height / 2);
                    offsetMin = new Vector2(0f, -rectTransform.rect.height / 2);
                    break;

                case AnchorType.Bottom:
                    anchorMaxPoint.x = anchorMinPoint.x + rectUIWidth;
                    anchorMaxPoint.y = anchorMinPoint.y;
                    offsetMax = new Vector2(0f, rectTransform.rect.height);
                    offsetMin = Vector2.zero;
                    break;

                case AnchorType.Left:
                    anchorMaxPoint.x = anchorMinPoint.x;
                    anchorMaxPoint.y = anchorMinPoint.y + rectUIHeight;
                    offsetMax = new Vector2(rectTransform.rect.width, 0f);
                    offsetMin = Vector2.zero;
                    break;

                case AnchorType.Center:
                    anchorMinPoint.x += rectUIWidth / 2;
                    anchorMaxPoint.x = anchorMinPoint.x;
                    anchorMaxPoint.y = anchorMinPoint.y + rectUIHeight;
                    offsetMax = new Vector2(rectTransform.rect.width / 2, 0f);
                    offsetMin = new Vector2(-rectTransform.rect.width / 2, 0f);
                    break;

                case AnchorType.Right:
                    anchorMinPoint.x += rectUIWidth;
                    anchorMaxPoint.x = anchorMinPoint.x;
                    anchorMaxPoint.y = anchorMinPoint.y + rectUIHeight;
                    offsetMax = Vector2.zero;
                    offsetMin = new Vector2(-rectTransform.rect.width, 0f);
                    break;

                case AnchorType.TopLeft:
                    anchorMinPoint.y += rectUIHeight;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(rectTransform.rect.width, 0f);
                    offsetMin = new Vector2(0f, -rectTransform.rect.height);
                    break;

                case AnchorType.TopCenter:
                    anchorMinPoint.x += rectUIWidth / 2;
                    anchorMinPoint.y += rectUIHeight;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(rectTransform.rect.width / 2, 0f);
                    offsetMin = new Vector2(-rectTransform.rect.width / 2, -rectTransform.rect.height);
                    break;

                case AnchorType.TopRight:
                    anchorMinPoint.x += rectUIWidth;
                    anchorMinPoint.y += rectUIHeight;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = Vector2.zero;
                    offsetMin = new Vector2(-rectTransform.rect.width, -rectTransform.rect.height);
                    break;

                case AnchorType.MiddleLeft:
                    anchorMinPoint.y += rectUIHeight / 2;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(rectTransform.rect.width, rectTransform.rect.height / 2);
                    offsetMin = new Vector2(0f, -rectTransform.rect.height / 2);
                    break;

                case AnchorType.MiddleCenter:
                    anchorMinPoint.x += rectUIWidth / 2;
                    anchorMinPoint.y += rectUIHeight / 2;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);
                    offsetMin = new Vector2(-rectTransform.rect.width / 2, -rectTransform.rect.height / 2);
                    break;

                case AnchorType.MiddleRight:
                    anchorMinPoint.x += rectUIWidth;
                    anchorMinPoint.y += rectUIHeight / 2;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(0f, rectTransform.rect.height / 2);
                    offsetMin = new Vector2(-rectTransform.rect.width, -rectTransform.rect.height / 2);
                    break;

                case AnchorType.BottomLeft:
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
                    offsetMin = Vector2.zero;
                    break;

                case AnchorType.BottomCenter:
                    anchorMinPoint.x += rectUIWidth / 2;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height);
                    offsetMin = new Vector2(-rectTransform.rect.width / 2, 0f);
                    break;

                case AnchorType.BottomRight:
                    anchorMinPoint.x += rectUIWidth;
                    anchorMaxPoint = anchorMinPoint;
                    offsetMax = new Vector2(0f, rectTransform.rect.height);
                    offsetMin = new Vector2(-rectTransform.rect.width, 0f);
                    break;
            }

            //Convert to relative anchor coordinates
            anchorMinPoint.x /= parentUIWidth;
            anchorMinPoint.y /= parentUIHeight;

            anchorMaxPoint.x /= parentUIWidth;
            anchorMaxPoint.y /= parentUIHeight;


            //apply new anchors
            rectTransform.anchorMin = anchorMinPoint;
            rectTransform.anchorMax = anchorMaxPoint;
            rectTransform.offsetMax = offsetMax;
            rectTransform.offsetMin = offsetMin;

            if (hasAspectRatio)
            {
                go.GetComponent<AspectRatioFitter>().enabled = true;
            }
        }
    }

    #endregion

    #region HELP

    [MenuItem("Help/Atributes")]
    public static void LogAtributes()
    {
        Debug.Log(
            "Editor:\n [HideInInspector]\n  [ExecuteInEditMode]\n  [ContextMenu()]\n  [CanEditMultipleObjects]\n  " +
            "[MenuItem()]\n  [Range()]\n  [Tooltip()]\n  [Header]\n  [Space()]\n  [RequireComponent()]\n  [Serializable]\n  [SerializeField]\n  [TextArea()]"
            );
    }

    [MenuItem("Help/Microsoft CSharp Conventions")]
    public static void OpenURLConventions()
    {
        Application.OpenURL("https://msdn.microsoft.com/en-us/library/ms229042.aspx");
    }

    [MenuItem("Help/APK's Folder in Dropbox")]
    public static void OpenDropBox()
    {
        Application.OpenURL("https://www.dropbox.com/home/LP%20Mobile/APKs");
    }

    [MenuItem("Help/Update Unity 3D")]
    public static void UpdateUnity3D()
    {
        Application.OpenURL("https://unity3d.com/get-unity/update");
    }

    #endregion

    #region MOVE

    //[MenuItem("Tools/Transform/Move/ ↑ %UP")]
    //public static void MoveTransformUp()
    //{
    //    MoveSelected(Direction.Up);
    //}

    //[MenuItem("Tools/Transform/Move/ ↓ %DOWN")]
    //public static void MoveTransformDown()
    //{
    //    MoveSelected(Direction.Down);
    //}

    //[MenuItem("Tools/Transform/Move/ ←| %LEFT")]
    //public static void MoveTransformLeft()
    //{
    //    MoveSelected(Direction.Left);
    //}

    //[MenuItem("Tools/Transform/Move/ |→ %RIGHT")]
    //public static void MoveTransformRight()
    //{
    //    MoveSelected(Direction.Right);
    //}

    public static void MoveSelected(Direction direction)
    {
        Transform transform = Selection.activeGameObject.transform;
        Undo.RecordObject(transform, "Move " + direction);
        AdjustMoveSpeedOnHold(direction);
        switch (direction)
        {
            case Direction.Up:
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y + moveFactor,
                    transform.localPosition.z);
                break;
            case Direction.Down:
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y - moveFactor,
                    transform.localPosition.z);
                break;
            case Direction.Left:
                transform.localPosition = new Vector3(
                    transform.localPosition.x - moveFactor,
                    transform.localPosition.y,
                    transform.localPosition.z);
                break;
            case Direction.Right:
                transform.localPosition = new Vector3(
                    transform.localPosition.x + moveFactor,
                    transform.localPosition.y,
                    transform.localPosition.z);
                break;
        }
    }

    private static void AdjustMoveSpeedOnHold(Direction direction)
    {
        if (selectedDirection == direction)
        {
            waitCounter++;
            if (waitCounter == 30)
            {
                moveFactor += 1;
            }
        }
        else
        {
            moveFactor = 0.5f;
            waitCounter = 0;
            selectedDirection = direction;
        }
    }

    #endregion

    [MenuItem("Tools/Redo #%z")]
    public static void RedoVS()
    {
        Undo.PerformRedo();
    }

    [MenuItem("Tools/UI/Move Sibling Up %&UP")]
    public static void MoveSiblingUp()
    {
        Transform transform = Selection.activeGameObject.transform;
        transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    }

    [MenuItem("Tools/UI/Move Sibling Down %&DOWN")]
    public static void MoveSiblingDown()
    {
        Transform transform = Selection.activeGameObject.transform;
        transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
    }

    [MenuItem("Tools/UI/Move Sibling Out %&LEFT")]
    public static void MoveSiblingOut()
    {
        Transform transform = Selection.activeGameObject.transform;
        int parentLocation = transform.parent.GetSiblingIndex();
        transform.parent = transform.parent.parent;
        transform.SetSiblingIndex(parentLocation);
    }

    [MenuItem("Tools/UI/Move Sibling In %&RIGHT")]
    public static void MoveSiblingIn()
    {
        Transform transform = Selection.activeGameObject.transform;
        bool reachedTarget = false;

        if (transform.parent == null)
        {
            foreach (GameObject go in SceneRoots())
            {
                if (reachedTarget)
                {
                    transform.transform.SetParent(go.transform);
                    SetExpandedRecursive(go, true);
                    for (int i = 0; i < go.transform.childCount; i++)
                    {
                        SetExpandedRecursive(go.transform.GetChild(i).gameObject, false);
                    }
                    Selection.activeGameObject = transform.gameObject;
                    transform.SetSiblingIndex(0);

                    break;
                }
                if (go == transform.gameObject)
                {
                    reachedTarget = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (reachedTarget)
                {
                    GameObject go = transform.parent.GetChild(i).gameObject;
                    transform.transform.SetParent(go.transform);
                    SetExpandedRecursive(go, true);
                    for (int j = 0; j < go.transform.childCount; j++)
                    {
                        SetExpandedRecursive(go.transform.GetChild(j).gameObject, false);
                    }
                    Selection.activeGameObject = transform.gameObject;
                    transform.SetSiblingIndex(0);
                    break;
                }
                if (transform == transform.parent.GetChild(i))
                {
                    reachedTarget = true;
                }
            }
        }
    }

    [MenuItem("Tools/Log Transform Dimension")]
    public static void LogTransformInformation()
    {
        Transform[] transforms = TakeSelectionTransforms(Selection.transforms);

        foreach (Transform transform in transforms)
        {
            Debug.Log(string.Format("NAME:\"{0}\"" + "\n|POS X:{1,6:###.###}| Y:{2,6:###.###}| Z:{3,6:###.###}|" + "\n|ROT X:{4,6:###.###}| Y:{5,6:###.###}| Z:{6,6:###.###}|" + "\n|SCA X:{7,6:###.###}| Y:{8,6:###.###}| Z:{9,6:###.###}|", transform.name, transform.localPosition.x, transform.localPosition.y, transform.localPosition.z, transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localScale.x, transform.localScale.y, transform.localScale.z));

            Debug.DrawLine(Vector3.left, Vector3.right);
        }
    }

    [MenuItem("Tools/Find unused sprites from directory")]
    public static void LogUsedImages()
    {
        throw new NotImplementedException("Need to add logic for window. Take path and display how many items used,unused, and total is in resouses and on hierarchy");
        //GameObject[] objArr = TakeSelectionGameObjects(Selection.gameObjects);
        //SmallList<string> usedSpriteNames = new SmallList<string>();
        //SmallList<string> unusedSpriteNames = new SmallList<string>();
        //SmallList<string> allSpriteNames = new SmallList<string>();

        //AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Fonts/Roboto-Regular.");

        //for (int i = 0; i < objArr.Length; i++)
        //{
        //    Image image = objArr[i].GetComponent<Image>();
        //    if (image != null)
        //    {

        //    }

        //    RawImage raw = objArr[i].GetComponent<RawImage>();
        //}
    }

    private static GameObject[] TakeSelectionGameObjects(GameObject[] gameObjects)
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            gameObjects = Selection.gameObjects;
        }

        return gameObjects;
    }

    private static Transform[] TakeSelectionTransforms(Transform[] transforms)
    {
        if (transforms == null || transforms.Length == 0)
        {
            transforms = Selection.transforms;
        }

        Undo.RecordObjects(transforms, "Disable Raycast to All");
        return transforms;
    }

    public static IEnumerable<GameObject> SceneRoots()
    {
        HierarchyProperty prop = new HierarchyProperty(HierarchyType.GameObjects);
        int[] expanded = new int[0];
        while (prop.Next(expanded))
        {
            yield return prop.pptrValue as GameObject;
        }
    }

    public static void SetExpandedRecursive(GameObject go, bool expand)
    {
        Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        MethodInfo methodInfo = type.GetMethod("SetExpandedRecursive");

        EditorApplication.ExecuteMenuItem("Window/Hierarchy");
        EditorWindow window = EditorWindow.focusedWindow;

        methodInfo.Invoke(window, new object[] { go.GetInstanceID(), expand });
    }

    private static void CreateViewItems(ScrollRect view)
    {
        Image img = view.gameObject.AddMissingComponent<Image>();
        GameObject gridObject = new GameObject("Grid", typeof(GridLayoutGroup), typeof(ContentSizeFitter));
        GridLayoutGroup grid = gridObject.GetComponent<GridLayoutGroup>();
        ContentSizeFitter fitter = gridObject.GetComponent<ContentSizeFitter>();
        GameObject gridChild = new GameObject("Button", typeof(Image), typeof(Button));
        Button button = gridChild.GetComponent<Button>();

        gridObject.transform.parent = view.transform;
        button.transform.parent = gridObject.transform;

        fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;

        img.color = new Color32(255, 255, 255, 0);

        view.gameObject.AddMissingComponent<RectMask2D>();
        view.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 768f);
        view.content = gridObject.GetComponent<RectTransform>();
        view.vertical = true;
        view.horizontal = false;
        view.movementType = ScrollRect.MovementType.Elastic;
        view.elasticity = 0.09f;
        view.inertia = true;
        view.decelerationRate = 0.003f;

        grid.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f);
        grid.cellSize = new Vector2(200f, 120f);
        grid.spacing = new Vector2(0f, 2f);
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Vertical;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 1;

        button.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 120f);
        button.interactable = true;
        button.transition = Selectable.Transition.None;
        Navigation navigation = new Navigation
        {
            mode = Navigation.Mode.None
        };

        button.navigation = navigation;

        button.GetComponent<Image>().raycastTarget = true;
        button.GetComponent<Image>().color = new Color32(255, 255, 255, 0);


        Selection.activeGameObject = button.gameObject;
    }

    private static T AddMissingComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
        {
            component = go.AddComponent<T>();
        }

        return component;
    }
}
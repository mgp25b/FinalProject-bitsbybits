
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public class MalibuMenuBuilder
{
    private static string menuFolder = "Assets/MalibuMotorcycleMenu";
    private static string artFolder = "Assets/MalibuMotorcycleMenu/Art";
    private static string sceneFolder = "Assets/Scenes";

    [MenuItem("Tools/Create Malibu Motorcycle Menu")]
    public static void CreateStartMenu()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Directory.CreateDirectory(sceneFolder);

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.DefaultGameObjects,
            NewSceneMode.Single
        );

        GameObject canvasObject = new GameObject("Malibu Menu Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1648, 928);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Sprite backgroundSprite = FindArtwork();

        GameObject background = new GameObject("Background");
        background.transform.SetParent(canvasObject.transform, false);

        Image backgroundImage = background.AddComponent<Image>();
        backgroundImage.sprite = backgroundSprite;
        backgroundImage.preserveAspect = false;
        backgroundImage.raycastTarget = false;

        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        GameObject instructionsPanel = CreateInstructionsPanel(
            canvasObject.transform, font
        );

        GameObject controllerObject = new GameObject("Malibu Menu Controller");
        MalibuMenuController controller =
            controllerObject.AddComponent<MalibuMenuController>();

        controller.instructionsPanel = instructionsPanel;
        instructionsPanel.SetActive(false);

        Button playButton = CreateHitArea(
            canvasObject.transform,
            "Play Hit Area",
            new Vector2(515, -140),
            new Vector2(490, 105)
        );

        Button howToButton = CreateHitArea(
            canvasObject.transform,
            "Instructions Hit Area",
            new Vector2(515, -232),
            new Vector2(490, 88)
        );

        Button quitButton = CreateHitArea(
            canvasObject.transform,
            "Quit Hit Area",
            new Vector2(515, -326),
            new Vector2(490, 88)
        );

        Button closeButton = CreateHitArea(
            instructionsPanel.transform,
            "Close Instructions Button",
            new Vector2(0, -180),
            new Vector2(250, 70)
        );

        // Save button actions permanently in the scene.
        UnityEventTools.AddPersistentListener(
            playButton.onClick, controller.PlayGame
        );

        UnityEventTools.AddPersistentListener(
            howToButton.onClick, controller.ShowInstructions
        );

        UnityEventTools.AddPersistentListener(
            quitButton.onClick, controller.QuitGame
        );

        UnityEventTools.AddPersistentListener(
            closeButton.onClick, controller.HideInstructions
        );

        // Create an EventSystem for UI interaction.
        if (Object.FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        EditorSceneManager.SaveScene(
            scene,
            sceneFolder + "/MalibuStartMenu.unity"
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Malibu Menu Created",
            "Your Malibu start menu has been created at Assets/Scenes/MalibuStartMenu.unity",
            "OK"
        );
    }

    private static Sprite FindArtwork()
    {
        string[] artworkNames =
        {
            "Malibu Motorcycle Menu Artwork",
            "Malibu Coastal Background"
        };

        foreach (string artworkName in artworkNames)
        {
            string[] results = AssetDatabase.FindAssets(
                artworkName + " t:Sprite",
                new[] { artFolder }
            );

            if (results.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(results[0]);
                return AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
        }

        Debug.LogError(
            "Artwork not found. Make sure the images are inside " + artFolder +
            " and their Texture Type is set to Sprite (2D and UI)."
        );

        return null;
    }

    private static Button CreateHitArea(
        Transform parent,
        string objectName,
        Vector2 position,
        Vector2 size
    )
    {
        GameObject buttonObject = new GameObject(objectName);
        buttonObject.transform.SetParent(parent, false);

        RectTransform rect = buttonObject.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(1, 1, 1, 0.01f);
        image.raycastTarget = true;

        Button button = buttonObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.interactable = true;

        return button;
    }

    private static GameObject CreateInstructionsPanel(
        Transform parent,
        Font font
    )
    {
        GameObject panel = new GameObject("Instructions Panel");
        panel.transform.SetParent(parent, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(700, 500);

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.16f, 0.05f, 0.22f, 0.96f);

        GameObject heading = CreateText(
            panel.transform,
            "Instructions Heading",
            "HOW TO PLAY",
            font,
            42,
            new Vector2(0, 150),
            new Vector2(600, 80)
        );

        GameObject instructions = CreateText(
            panel.transform,
            "Instructions Text",
            "Use the arrow keys to steer your motorcycle.\n\nAvoid obstacles and stay on the road!\n\nEnjoy your Malibu ride!",
            font,
            25,
            new Vector2(0, 10),
            new Vector2(600, 220)
        );

        return panel;
    }

    private static GameObject CreateText(
        Transform parent,
        string objectName,
        string message,
        Font font,
        int fontSize,
        Vector2 position,
        Vector2 size
    )
    {
        GameObject textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);

        RectTransform rect = textObject.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Text text = textObject.AddComponent<Text>();
        text.text = message;
        text.font = font;
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        return textObject;
    }
}


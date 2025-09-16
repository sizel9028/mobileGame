using UnityEditor;
using UnityEngine;

public class ForceFullRectImporter : AssetPostprocessor
{
    private const int PixelsPerUnit = 128;

    void OnPreprocessTexture()
    {
        // Resources 폴더 안의 PNG만 적용
        if (!assetPath.StartsWith("Assets/Resources/")) return;

        var importer = (TextureImporter)assetImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = PixelsPerUnit;
        importer.spriteImportMode = SpriteImportMode.Single;

        // 항상 원본 크기 유지 (투명 영역 무시하지 않음)
        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        importer.SetTextureSettings(settings);
    }

    [MenuItem("Tools/Sprites/Reimport All Resources Sprites As FullRect")]
    private static void ReimportAll()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Resources" });
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        Debug.Log("[ForceFullRectImporter] Resources 폴더 내 모든 PNG를 128x128 FullRect로 강제 세팅 완료!");
    }
}

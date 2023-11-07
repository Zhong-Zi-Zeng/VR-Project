using UnityEngine;
using UnityEditor;
using System.IO;

class Change2DtoCube : AssetPostprocessor
{
    public void OnPreprocessTexture()
    {
        if (assetPath.Contains("panoramawithmask"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureShape = TextureImporterShape.TextureCube;
            textureImporter.SaveAndReimport();
        }
    }
}
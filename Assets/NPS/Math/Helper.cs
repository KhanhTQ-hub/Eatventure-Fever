using UnityEngine;

public static class Helper
{
    #region Convert

    /// <summary>
    /// Chuyển String về Int
    /// </summary>
    public static int ParseInt(this string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }

        return result;
    }

    /// <summary>
    /// Chuyển Char về Int
    /// </summary>
    public static int ParseInt(this char value)
    {
        int result = 0;
        result = 10 * result + (value - 48);
        return result;
    }

    #endregion

    /// Decimal to Price
    /// </summary>
    public static string PriceShow(this decimal price)
    {
        var isDecimalInt = price % 1 == 0 && price >= int.MinValue && price <= int.MaxValue;
        return string.Format(isDecimalInt ? "{0:N0}" : "{0:N}", price);
    }

    #region Layer

    /// <summary>
    /// Check if gameobject has layer with name
    /// </summary>
    public static bool CompareLayer(this GameObject go, string layerName)
    {
        return go.layer == LayerMask.NameToLayer(layerName);
    }

    /// <summary>
    /// Set layer for gameobject and its children
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void SetLayerRecursively(this GameObject go, int layer)
    {
        foreach (Transform trans in go.transform.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }

    #endregion
}
using UnityEngine;
using System.Collections;

public class HFTColorUtils {

  public static float Max(Color c) {
    float max = c.r > c.g ? c.r : c.g;
    return max > c.b ? max : c.b;
  }

  public static float Min(Color c) {
    float min = c.r < c.g ? c.r : c.g;
    return min < c.b ? min : c.b;
  }

  public static float Fract(float v) {
    float r = v % 1.0f;
    return r < 0 ? 1.0f - r : r;
  }

  public static float MinMax(float v, float min, float max) {
    return Mathf.Min(Mathf.Max(v, min), max);
  }

  public static float Saturate(float v) {
    return MinMax(v, 0.0f, 1.0f);
  }

  /// <summary>
  /// Converts an RGBA color value to HSVA. Conversion formula
  /// adapted from http://en.wikipedia.org/wiki/HSV_color_space.
  /// Assumes r, g, and b are contained in the set [0, 1] and
  /// returns h, s, and v in the set [0, 1].
  /// </summary>
  /// <param name="c">Color to convert</param>
  /// <returns>HSVA representation</returns>
  static public Vector4 ColorToHSVA(Color c) {
    float max = Max(c);
    float min = Min(c);
    float h;
    float s;
    float v = max;
    float r = c.r;
    float g = c.g;
    float b = c.b;

    float d = max - min;
    s = max == 0 ? 0 : d / max;

    if (max == min) {
      h = 0; // achromatic
    } else {
      if (max == r) {
        h = (g - b) / d + (g < b ? 6.0f : 0);
      } else if (max == g) {
        h = (b - r) / d + 2.0f;
      } else {
        h = (r - g) / d + 4.0f;
      }
      h /= 6.0f;
    }

    return new Vector4(h, s, v, c.a);
  }

  /// <summary>
  /// Converts a HSVA color to a RGBA Color
  ///
  /// adapted from http://en.wikipedia.org/wiki/HSV_color_space.
  /// Assumes h, s, and v are contained in the set [0, 1] and
  /// returns r, g, and b in the set [0, 1].
  /// </summary>
  /// <param name="hsva">Vector4 with h, s, v, a</param>
  /// <returns>an RGBA color</returns>
  static public Color HSVAToColor(Vector4 hsva) {
    float r = 0.0f;
    float g = 0.0f;
    float b = 0.0f;
    float h = Fract(hsva.x);
    float s = Saturate(hsva.y);
    float v = Saturate(hsva.z);

    float i = Mathf.Floor(h * 6.0f);
    float f = h * 6.0f - i;
    float p = v * (1.0f - s);
    float q = v * (1.0f - f * s);
    float t = v * (1.0f - (1.0f - f) * s);

    switch((int)i % 6) {
      case 0: r = v; g = t; b = p; break;
      case 1: r = q; g = v; b = p; break;
      case 2: r = p; g = v; b = t; break;
      case 3: r = p; g = q; b = v; break;
      case 4: r = t; g = p; b = v; break;
      case 5: r = v; g = p; b = q; break;
    }

    return new Color(r, g, b, hsva.w);
  }

}

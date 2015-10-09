using HappyFunTimes;
using HFTSounds;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class HFTGlobalSoundHelper : MonoBehaviour {

  public static Sounds GetSounds() {
    return s_sounds;
  }

  void Awake()
  {
      if (s_sounds == null)
      {
        InitSounds();
      }
  }

  void InitSounds()
  {
    s_sounds = new Sounds();
    string baseFolder = Path.Combine(Path.Combine(Application.dataPath, "WebPlayerTemplates"), "HappyFunTimes");
    string soundFolder = Path.Combine(baseFolder, "sounds");
    if (Directory.Exists(soundFolder))
    {
      AddSoundFiles(baseFolder, Directory.GetFiles(soundFolder, "*.mp3"));
      AddSoundFiles(baseFolder, Directory.GetFiles(soundFolder, "*.wav"));
      AddJSFXSounds(Directory.GetFiles(soundFolder, "*.jsfx.txt"));
    }
  }

  void AddJSFXSounds(string[] filenames)
  {
    foreach(string filename in filenames)
    {
      string content = System.IO.File.ReadAllText(filename);
      string[] lines = content.Split(s_lineDelims, System.StringSplitOptions.None);
      int lineNo = 0;
      foreach (string lineStr in lines)
      {
        ++lineNo;
        string line = lineStr.Split('#')[0].Split('/')[0].Split(';')[0].Trim();
        if (line.Length == 0)
        {
          continue;
        }

        // TODO remove comments
        Match m = s_jsfxRE.Match(line);
        if (!m.Success)
        {
          Debug.LogError(filename + " line: " + lineNo + " could not parse line");
          continue;
        }
        string name = m.Groups[1].Value;
        string generator = m.Groups[2].Value;
        string numbersString = m.Groups[3].Value;
        string[] numberStrings = numbersString.Split(',');
        if (numberStrings.Length != 27)
        {
          Debug.LogError(filename + " line:" + lineNo + " expected 27 values found " + numberStrings.Length);
          continue;
        }

        float[] parameters = new float[27];
        int i = 0;
        bool error = false;
        foreach (string numstr in numberStrings)
        {
          try
          {
            parameters[i] = float.Parse(numstr, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
          }
          catch (System.Exception)
          {
            Debug.LogError(filename + " line:" + lineNo + " could not parse number " + numstr);
            error = true;
          }
          ++i;
        }

        if (error)
        {
          continue;
        }

        s_sounds[name] = new SoundJSFX(generator, parameters);
      }
    }
  }

  void AddSoundFiles(string baseFolder, string[] filenames)
  {
    foreach(string filename in filenames)
    {
      string filepath = filename.Substring(baseFolder.Length + 1).Replace("\\", "/");
      s_sounds[Path.GetFileNameWithoutExtension(filename)] = new SoundFile(filepath);
    }
  }

  private static Sounds s_sounds = null;
  private static Regex s_jsfxRE = new Regex(@"(\w+)\s*?\[""(\w+)""\s*?,(.*?)\]");
  private static string[] s_lineDelims = new string[] { "\r\n", "\n" };
};


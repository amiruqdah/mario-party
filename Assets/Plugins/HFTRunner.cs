/*
 * Copyright 2014, Gregg Tavares.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 *     * Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above
 * copyright notice, this list of conditions and the following disclaimer
 * in the documentation and/or other materials provided with the
 * distribution.
 *     * Neither the name of Gregg Tavares. nor the names of its
 * contributors may be used to endorse or promote products derived from
 * this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using UnityEngine;
using HappyFunTimes;
using System.Collections;
using System.IO;

namespace HappyFunTimes
{
    public class HFTRunner : MonoBehaviour {

        private const string kExeKey = "HappyFunTimesExecutable";
        private GameServer m_gameServer;

        public void HFTInitializeRunner(GameServer gs)
        {
            m_gameServer = gs;
            m_gameServer.OnConnectFailure += Run;
        }

        public void HFTNeedNewHFT(GameServer gs)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog(
                "HappyFunTimes",
                "This game needs a newer version of HappyFunTimes",
                "Ok");
            #endif
        }

        void Run(object sender, System.EventArgs args)
        {
            #if UNITY_EDITOR
                RunHFT();
            #endif
        }

        #if UNITY_EDITOR
        void RunHFT()
        {
            string exePath = UnityEditor.EditorPrefs.GetString(kExeKey);

            #if UNITY_EDITOR_OSX
                if (System.String.IsNullOrEmpty(exePath) || (!Directory.Exists(exePath) && !(File.Exists(exePath)))) {
                    exePath = "/Applications/HappyFunTimes.app";
                    if (!Directory.Exists(exePath) && !File.Exists(exePath)) {
                        AskAboutHappyFunTimesApp();
                        return;
                    }
                }
                // If it's a directory (.app) launch it with open.
                // If it's start.js launch it with node
                if (exePath.EndsWith("start.js")) {
                    System.Diagnostics.Process.Start("osascript", @"-e '
tell application ""Terminal""
  tell window 1
    do script ""hft start""
  end tell
end tell
'");
                } else {
                    System.Diagnostics.Process.Start("open", "-a \"" + exePath + "\"");
                }
            #elif UNITY_EDITOR_WIN
                // See if we can find HappyFunTimes
                // Use registry!!!
                bool ask = false;
                if (System.String.IsNullOrEmpty(exePath) || !File.Exists(exePath)) {
                    ask = true;
                    object loc = LocalRegistryGetKey(
                        "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Greggman HappyFunTimes",
                        "InstallLocation");
                    if (loc != null && !System.String.IsNullOrEmpty((string)loc)) {
                        exePath = System.IO.Path.Combine(StripQuotes((string)loc), "node.exe");
                        ask = !File.Exists(exePath);
                    }
                }
                if (ask) {
                    AskAboutHappyFunTimesApp();
                    return;
                }
                string nodePath;
                string startPath;
                if (exePath.EndsWith("start.js")) {
                    startPath = exePath;
                    nodePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(exePath), "node.exe");
                } else {
                    nodePath = exePath;
                    startPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(exePath), "start.js");
                }
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = "cmd.exe";
                psi.Arguments = "/c \"\"" + nodePath + "\" \"" + startPath + "\"\"";
                psi.LoadUserProfile = true;
                System.Diagnostics.Process.Start(psi);
            #endif
            // Try for 5 seconds?
            for (int count = 0; count < 10; ++count) {
                if (CheckForHFT()) {
                    m_gameServer.Connect();
                    return;
                }
                System.Threading.Thread.Sleep(1000);
            }

            UnityEditor.EditorUtility.DisplayDialog(
                "HappyFunTimes",
                "Could not find HappyFunTimes",
                "Ok");
            Application.Quit();
        }

        bool CheckForHFT()
        {
            bool success = false;
            try {
                System.Net.WebRequest request = System.Net.WebRequest.Create(m_gameServer.GetBaseHttpUrl());
                request.Method = "POST";
                string postData = "{\"cmd\":\"happyFunTimesPing\"}";
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes (postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                System.Net.WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                success = responseFromServer.Contains("HappyFunTimes");
                if (success) {
                    Debug.Log("HappyFunTimes found");
                }
            } catch (System.Exception) {
                Debug.Log("waiting for happyfuntimes...");
            }
            return success;
        }

        void Install()
        {
            Application.OpenURL("http://docs.happyfuntimes.net/docs/unity/install.html");
            Application.Quit();
        }

        void FindIt()
        {
            #if UNITY_EDITOR_OSX
            string path = UnityEditor.EditorUtility.OpenFilePanel(
                "Select HappyFunTimes",
                "/Applications",
                "");
            if (!System.String.IsNullOrEmpty(path)) {
                UnityEditor.EditorPrefs.SetString(kExeKey, path);
                RunHFT();
            }
            #elif UNITY_EDITOR_WIN
            string path = UnityEditor.EditorUtility.OpenFilePanel(
                "Select HappyFunTimes start.js",
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles),
                ".js");
            if (!System.String.IsNullOrEmpty(path)) {
                UnityEditor.EditorPrefs.SetString(kExeKey, path);
                RunHFT();
            }
            #endif
        }

        void Quit()
        {
            Application.Quit();
        }

        void AskAboutHappyFunTimesApp()
        {
            int result = UnityEditor.EditorUtility.DisplayDialogComplex(
                "HappyFunTimes",
                "Could not find HappyFunTimes",
                "Install It",
                "Tell Me Where It's Installed",
                "Quit");
            Debug.Log("result: " + result);
            switch (result) {
                case 0:  // Install It
                    Install();
                    break;
                case 1:  // Tell me where
                    FindIt();
                    break;
                case 2:  // Quit
                    Quit();
                    break;
                default:
                    Debug.LogError("Something went wrong :(");
                    break;
            }
        }

        #if UNITY_EDITOR_WIN
        object LocalRegistryGetKey(string path, string sub)
        {
            object v = null;
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path);
                if (key != null)
                {
                    v = key.GetValue(sub);
                }
            }
            catch (System.Exception ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                Debug.LogException(ex);
            }
            return v;
        }

        string StripQuotes(string s)
        {
            if (s.StartsWith("\""))
            {
                s = s.Substring(1);
            }
            if (s.EndsWith("\""))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }
        #endif  // UNITY_EDITOR_WIN

        #endif  // UNITY_EDITOR
    }
}


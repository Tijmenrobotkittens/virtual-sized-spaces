using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
namespace RobotKittens
{

    public class TextureVaultItem
    {
        public string ID;
        public string directory = "";
        public TextureVault.SuccessHandler onSuccess;
        public TextureVault.ErrorHandler onError;

        public TextureVaultItem(string ID, TextureVault.SuccessHandler onSuccess, TextureVault.ErrorHandler onError, string directory = "")
        {
            this.ID = ID;
            this.onSuccess = onSuccess;
            this.onError = onError;
            this.directory = directory;
        }
    }

    public class ThrottledTextureVault
    {
        private Queue<TextureVaultItem> queue;

        public ThrottledTextureVault(MonoBehaviour context)
        {
            this.queue = new Queue<TextureVaultItem>(context, this.OnAdvanceQueueAction, 5);
        }

        public void LoadTexture(string ID, TextureVault.SuccessHandler onSuccess, TextureVault.ErrorHandler onError, string directory = "")
        {
            TextureVaultItem textureVaultItem = queue.GetQueuedRequests().Where(item => item.ID == ID).First();
            if(textureVaultItem != null)
            {
                queue.DeleteRequest(textureVaultItem);
            }

            this.queue.Process(new TextureVaultItem(ID, onSuccess, onError, directory));
        }

        private void OnAdvanceQueueAction(TextureVaultItem item)
        {
            TextureVault.LoadTextureItem(item.ID, item.onSuccess, item.onError, item.directory);
        }
    }

    internal class TextureVaultHistory
    {
        public string id;
        public DateTime lastTouched;
        public Texture2D texture;

        public TextureVaultHistory(string id, Texture2D texture)
        {
            this.id = id;
            this.texture = texture;
            this.lastTouched = DateTime.Now;
        }
    }

    public class TextureVault
    {
        public enum SAVE_TYPES { PNG, EXR, JPG };
        private static string vaultLocation = Application.persistentDataPath;
        private static bool obfuscateID = false;
        private static string defaultVaultDirectory = "vault";

        public delegate void SuccessHandler(Texture2D item);
        public delegate void ErrorHandler();

        private static Dictionary<string, TextureVaultHistory> textureCache = new Dictionary<string, TextureVaultHistory>();

        public static bool Get(string ID, string directory = "")
        {
            ID = TextureVault.GenerateIdentifier(ID);
            directory = TextureVault.GetDirectory(directory);

            string fileName = TextureVault.GetFilename(directory, ID);
            if (textureCache.ContainsKey(fileName))
            {
                return true;
            }

            if (TextureVault.DirectoryExists(directory))
            {
                return File.Exists(fileName);
            }
            return false;
        }

        private static void Touch(string fileName)
        {
            if (textureCache.ContainsKey(fileName))
            {
                textureCache[fileName].lastTouched = DateTime.Now;
            }
            CleanupCache();
        }

        private static void CleanupCache()
        {
            int textureCacheLimit = 50;
            int maxNumMinutesBeforeRemoved = 1;
            textureCache = textureCache
                .ToList()
                .OrderBy(pair => pair.Value.lastTouched)
                .Where(pair => pair.Value.lastTouched.CompareTo(DateTime.Now.AddMinutes(-maxNumMinutesBeforeRemoved)) > -1)
                .Take(textureCacheLimit)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static void LoadTextureItem(string ID, SuccessHandler onSuccess, ErrorHandler onError, string directory = "")
        {
            if (TextureVault.Get(ID, directory))
            {
                ID = TextureVault.GenerateIdentifier(ID);
                directory = TextureVault.GetDirectory(directory);

                string fileName = TextureVault.GetFilename(directory, ID);


                if(textureCache.ContainsKey(fileName))
                {
                    Touch(fileName);
                    Debug.Log("From Cache " + fileName + " " + textureCache.Count);
                    onSuccess(textureCache[fileName].texture);
                    return;
                }

                byte[] data = File.ReadAllBytes(fileName);

                Texture2D texture2D = new Texture2D(2, 2);
                if (texture2D.LoadImage(data, true))
                {
                    textureCache.Add(fileName, new TextureVaultHistory(fileName, texture2D));
                    CleanupCache();
                    onSuccess(texture2D);
                    return;
                }
            }
            onError();
        }

        public static void Add(string ID, Texture2D texture, string directory = "", SAVE_TYPES saveType = SAVE_TYPES.PNG)
        {
            if(!texture) { return; }
            byte[] bytes = texture.EncodeToPNG();

            ID = TextureVault.GenerateIdentifier(ID);
            directory = TextureVault.GetDirectory(directory);
            string fileName = TextureVault.GetFilename(directory, ID);

            if (File.Exists(fileName)) {
                //Debug.LogFormat("[{0}] {1}", "TextureVault", "File exists in vault, halting " + fileName);
                return;
            }

            if (!TextureVault.DirectoryExists(directory)) {
                //Debug.LogFormat("[{0}] {1} {2}", "TextureVault", "Directory does not exists create", directory);
                Directory.CreateDirectory(directory);
            }

            //Debug.LogFormat("[{0}] {1} {2} {3}", "TextureVault", "Saving to vault", fileName, texture);
            File.WriteAllBytes(fileName, bytes);
        }

        private static bool DirectoryExists(string directory)
        {
            try {
                return Directory.Exists(directory);
            } catch( DirectoryNotFoundException exception ) {
                //
            }
            return false;
        }

        private static string GetDirectory(string directory = "")
        {
            if(directory != "") {
                directory = string.Format("{0}/{1}", TextureVault.defaultVaultDirectory, directory);
            } else {
                directory = TextureVault.defaultVaultDirectory;
            }
            return string.Format("{0}/{1}", TextureVault.vaultLocation, directory);
        }

        private static string GetFilename(string directory, string ID)
        {
            return string.Format("{0}/{1}.png", directory, ID);
        }

        private static string GenerateIdentifier(string path) 
        {
            string fileName = obfuscateID ? "" : Path.GetFileNameWithoutExtension(path);
            return string.Format("{0}-{1}", fileName, Animator.StringToHash(path));
        }

        public static void Clean()
        {
            string baseDirectory = TextureVault.GetDirectory();
            if (!TextureVault.DirectoryExists(baseDirectory)) return;

            string[] directories = Directory.GetDirectories(baseDirectory);
            foreach (string directory in directories)
            {
                //Debug.LogFormat("[{0}] {1}", "TextureVault", "Directory:" + directory);
                string[] files = Directory.GetFiles(directory);
                foreach (string file in files)
                {
                    // Everything a day old.
                    System.DateTime expiredTime = System.DateTime.Now;
                    expiredTime = expiredTime.AddDays(-7.0);

                    System.DateTime lastAccessTime = File.GetLastAccessTime(file);
                    if(expiredTime > lastAccessTime) {
                        File.Delete(file);
                        Debug.LogFormat("[{0}] {1}", "TextureVault", "File is expired " + file + " (" + expiredTime + "/" + lastAccessTime + ")");
                    }
                }
            }
        }

    }
}

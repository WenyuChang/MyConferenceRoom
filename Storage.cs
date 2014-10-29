using System;
using System.IO;
using System.IO.IsolatedStorage;

using Matrix.Xml;

namespace SilverlightMuc
{
    /// <summary>
    /// class which handles the isolated storage
    /// </summary>
    public class Storage
    {
        const string SETTINGS_FILENAME  = "settings.xml";
        const string AVATAR_FOLDER      = "avatar";

        private static Settings _Settings;

        public static Settings Settings
        {
            get { return _Settings; }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var isfs = new IsolatedStorageFileStream(SETTINGS_FILENAME, FileMode.Create, isf))
                    {
                        using (var sw = new StreamWriter(isfs))
                        {
                            sw.Write(_Settings.ToString());
                            sw.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public static void LoadSettings()
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(SETTINGS_FILENAME))
                    {
                        using (var isfs = new IsolatedStorageFileStream(SETTINGS_FILENAME, FileMode.Open, isf))
                        {
                            var sr = new StreamReader(isfs);
                            string xml = sr.ReadToEnd();
                            XmppXElement set = XmppXElement.LoadXml(xml);

                            if (set is Settings)
                                _Settings = set as Settings;
                        }
                    }
                    else
                        _Settings = new Settings();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Loads an avatar with the given hash.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static byte[] LoadAvatar(string fileName)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(AVATAR_FOLDER))
                {
                    string filePath = Path.Combine(AVATAR_FOLDER, fileName);
                    using (var isfs = new IsolatedStorageFileStream(filePath, FileMode.Open, isf))
                    {
                        var b = new byte[isfs.Length];
                        isfs.Read(b, 0, b.Length);
                        return b;
                    }

                }
                return null;
            }
        }

        /// <summary>
        /// Saves an avatar.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="image">The image.</param>
        public static void SaveAvatar(string fileName, byte[] image)
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.DirectoryExists(AVATAR_FOLDER))
                        isf.CreateDirectory(AVATAR_FOLDER);

                    var filePath = Path.Combine(AVATAR_FOLDER, fileName);
                    using (var isfs = new IsolatedStorageFileStream(filePath, FileMode.Create, isf))
                    {
                        isfs.Write(image, 0, image.Length);
                        isfs.Flush();
                        isfs.Close();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Chechs if an avatar exists in the cache
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static bool AvatarExists(string filename)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.DirectoryExists(AVATAR_FOLDER))
                {
                    string filePath = Path.Combine(AVATAR_FOLDER, filename);
                    return isf.FileExists(filePath);
                }
                return false;
            }
        }
    }
}
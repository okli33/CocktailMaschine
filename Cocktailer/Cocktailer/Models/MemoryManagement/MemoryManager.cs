﻿using Cocktailer.Models.DataManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cocktailer.Models.MemoryManagement { 
    public class MemoryManager<T> where T : IAmSaveable
    {
        string AppPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string subFolder;

        public MemoryManager()
        {
            if (typeof(T).Equals(typeof(Configuration)))
            {
                subFolder = "Configurations";
            }
            else if (typeof(T).Equals(typeof(Drink)))
            {
                subFolder = "Drinks";
            }
            else if (typeof(T).Equals(typeof(Ingredient)))
            {
                subFolder = "Ingredients";
            }
            else if (typeof(T).Equals(typeof(Recipe)))
            {
                subFolder = "Recipes";
            }
        }
        public void Save(T obj, string name)
        {
            string fileName = GetFileName(name);
            using (StreamWriter stream = new StreamWriter(fileName))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(stream, obj);
            }
        }

        public T Load(string name, bool pathIncluded = false)
        {
            var serializer = new JsonSerializer();
            if (!pathIncluded)
            {
                using (JsonReader reader = new JsonTextReader(new StringReader(GetFileName(name))))
                {
                    var Config = serializer.Deserialize<T>(reader);
                    return Config;
                }
            }
            using (JsonReader reader = new JsonTextReader(new StringReader(name)))
            {
                var Config = serializer.Deserialize<T>(reader);
                return Config;
            }
        }

        public List<T> GetAvailable()
        {
            List<T> objects = new List<T>();
            var objectNames = Directory.GetFiles(Path.Combine(AppPath, subFolder));
            foreach (var obj in objectNames)
            {
                objects.Add(Load(obj, true));
            }
            return objects;
        }
        private string GetFileName(string name) => Path.Combine(AppPath, subFolder, name, ".json");
    }
}

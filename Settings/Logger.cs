using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SVMC.Structure;

namespace SVMC.Settings
{
    public static class ModuleLogger
    {
        public static void Save(InsertedModules newEntry, string filePath)
        {
            List<InsertedModules> allEntries = new List<InsertedModules>();

            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                try
                {
                    allEntries = JsonConvert.DeserializeObject<List<InsertedModules>>(existingJson)
                                 ?? new List<InsertedModules>();
                }
                catch
                {
                    allEntries = new List<InsertedModules>(); 
                }
            }


            allEntries.Add(newEntry);


            string updatedJson = JsonConvert.SerializeObject(allEntries, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }
    }

    public static class PanelLogger
    {
        public static void Save(InsertedPanels entry, string filePath)
        {
            List<InsertedPanels> all = new List<InsertedPanels>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                try
                {
                    all = JsonConvert.DeserializeObject<List<InsertedPanels>>(json) ?? new List<InsertedPanels>();
                }
                catch
                {
                    all = new List<InsertedPanels>();
                }
            }

            all.Add(entry);

            string updated = JsonConvert.SerializeObject(all, Formatting.Indented);
            File.WriteAllText(filePath, updated);
        }
    }
}
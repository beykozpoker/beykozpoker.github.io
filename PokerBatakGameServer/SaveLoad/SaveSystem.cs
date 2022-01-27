using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PokerBatakGameServer.SaveLoad
{
    public static class SaveSystem
    {
        public static void SaveData(Dictionary<string, Players> _Players)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = "./PokerGamePlayersDatas";
            
            FileStream stream = new FileStream(path, FileMode.Create);

            DataManager dataManager = new DataManager(_Players);

            formatter.Serialize(stream, dataManager);
            stream.Close();
        }

        public static DataManager LoadData()
        {
            string path = "./PokerGamePlayersDatas";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                DataManager loadData = formatter.Deserialize(stream) as DataManager;
                stream.Close();

                return loadData;
            }
            else
            {
                Console.WriteLine("No Data Found");
                return null;
            }
        }
    }
}

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Reflection;

namespace StarboundApiDocViewer {
  [DataContract]
  class Config {
    private Config() {}

    [DataMember]
    public double WinPosX = 0;

    [DataMember]
    public double WinPosY = 0;

    [DataMember]
    public double WinWidth = 0;

    [DataMember]
    public double WinHeight = 0;

    [DataMember]
    public double SplitterPosition = 0;

    [DataMember]
    public bool WinMaximized = false;

    [DataMember]
    public string StarboundFolder = null;

    public void Save() {
      var ser = new DataContractJsonSerializer(typeof(Config));
      var fs = new FileStream(ConfigFile, FileMode.Create);
      ser.WriteObject(fs, this);
      fs.Close(); 
    }

    public static Config Load() {
      if (!File.Exists(ConfigFile))
        return new Config();

      var ser = new DataContractJsonSerializer(typeof(Config));
      var fs = new FileStream(ConfigFile, FileMode.Open);
      var o = ser.ReadObject(fs);
      fs.Close();

      return o as Config;
    }

    private static string ConfigFile {
      get { return Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "config"); }
    }
  }
}

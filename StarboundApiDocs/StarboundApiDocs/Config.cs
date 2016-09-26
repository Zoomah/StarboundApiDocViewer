using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Reflection;

namespace StarboundApiDocViewer {
	/// <summary>
	/// Represents a configuration structure
	/// </summary>
	[DataContract]
  class Config {
		/// <summary>
		/// Private constructor:
		/// only callable from within <see cref="Config"/>
		/// </summary>
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

		/// <summary>
		/// Save configuration to a file next to the .exe
		/// </summary>
    public void Save() {
      var ser = new DataContractJsonSerializer(typeof(Config));
      var fs = new FileStream(ConfigFile, FileMode.Create);
      ser.WriteObject(fs, this);
      fs.Close(); 
    }

		/// <summary>
		/// Loads configuration from a file next to the .exe
		/// </summary>
		/// <returns><see cref="Config"/> object, holding the loaded configuration</returns>
    public static Config Load() {
			// no file? return an empty config
      if (!File.Exists(ConfigFile))
        return new Config();
			// load config
			var ser = new DataContractJsonSerializer(typeof(Config));
      var fs = new FileStream(ConfigFile, FileMode.Open);
      var o = ser.ReadObject(fs);
      fs.Close();
			// return the data as the right type
      return o as Config;
    }

		/// <summary>
		/// Helper property:
		/// Gets the name of the config file
		/// </summary>
    private static string ConfigFile {
      get { return Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "config"); }
    }
  }
}

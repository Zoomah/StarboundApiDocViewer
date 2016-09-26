using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace StarboundApiDocViewer {
  class ViewTemplate {
		private static Dictionary<string, string> res;

		public static string render(string mdfile) {
			var template = LoadRessource("template.html");
			var script = LoadRessource("script.js");
			var style = LoadRessource("gitlike.css");
			var content = Markdown.ToHtml(File.ReadAllText(mdfile));
			return template
				.Replace("/*--script--*/", script)
				.Replace("/*--style--*/", style)
				.Replace("<!--content-->", content);
    }

		private static string LoadRessource(string name) {
			if (res != null && res.ContainsKey(name))
				return res[name];

			var result = "";
			try {
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Path(typeof(ViewTemplate).Namespace, "Template", name)))
				using (var reader = new StreamReader(stream)) {
					result = reader.ReadToEnd();
				}
			} catch (Exception) {
				return String.Empty;
			}

			if (res == null)
				res = new Dictionary<string, string>();
			res.Add(name, result);

			return result;
		}

		private static string Path(params string[] parts) {
			var i = 0;
			var result = "";
			foreach(var s in parts) {
				if (0 < i++)
					result += ".";
				result += s;
			}
			return result;
		}
  }
}

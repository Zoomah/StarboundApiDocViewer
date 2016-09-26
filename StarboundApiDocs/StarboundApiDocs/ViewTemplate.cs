using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace StarboundApiDocViewer {
	/// <summary>
	/// Handles template merging and rendering
	/// </summary>
	class ViewTemplate {
		/// <summary>
		/// Holds already loaded resources
		/// </summary>
		private static Dictionary<string, string> loadedRessources;

		/// <summary>
		/// Prepares the template and renders the .md file into HTML
		/// </summary>
		/// <param name="mdfile">full path of the .md file</param>
		/// <returns>the rendered HTML as string</returns>
		public static string render(string mdfile) {
			// load embedded ressources
			var template = LoadRessource("Template/template.html");
			var script = LoadRessource("Template/script.js");
			var style = LoadRessource("Template/gitlike.css");
			// pre-render md to HTML
			var content = Markdown.ToHtml(File.ReadAllText(mdfile)).Replace("<hr />", "<div class=\"hr\"></div>");
			// replace the placeholders with actual data and return it
			return template
				.Replace("/*--script--*/", script)
				.Replace("/*--style--*/", style)
				.Replace("<!--content-->", content);
		}

		/// <summary>
		/// Loads an embedded ressource.
		/// </summary>
		/// <param name="path">path to the embedded ressource</param>
		/// <returns>content of the ressource as string</returns>
		private static string LoadRessource(string path) {
			// already loaded? Get it from 'loadedRessources' then...
			if (loadedRessources != null && loadedRessources.ContainsKey(path))
				return loadedRessources[path];
			// load ressource
			var result = "";
			try {
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("{0}.{1}", typeof(ViewTemplate).Namespace, path.Replace("/", ".").Replace("\\", "."))))
				using (var reader = new StreamReader(stream)) {
					result = reader.ReadToEnd();
				}
			} catch (Exception) {
				return String.Empty;
			}
			// create new dictionary if needed
			if (loadedRessources == null)
				loadedRessources = new Dictionary<string, string>();
			// store ressource for later use
			loadedRessources.Add(path, result);
			// return ressource
			return result;
		}
	}
}

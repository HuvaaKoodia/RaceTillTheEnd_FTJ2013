using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Linq;

public static class MyExtensions
{
	public static Stream ToStream(this string str)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(str);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}
}

public class XMLsys : MonoBehaviour {


	public MapController map_db;
	
	//engine logic
	void Awake () {
		readXML();
	}
	
	void OnDestroy(){
		//writeXML();
	}
	
	//game logic
	void readXML(){

		var path="Data/Maps";
#if UNITY_WEBPLAYER

		var files=Resources.LoadAll(path);
		
		foreach (var f in files){

			var Xdoc=new XmlDocument();
			var asset=(TextAsset)f;
			Xdoc.Load(asset.text.ToStream());
			
			//read xml
			
			var root=Xdoc["Root"];
			
			foreach (XmlNode node in root){
				if (node.Name=="Map"){
					var map=new MapData(5,5);
					
					var spl=node.InnerText.Replace(" ","").Replace("\r","").Split('\n');
					int i=0,j=0;
					foreach (var line in spl){
						if (line=="") continue;
						while (j<map.map_data.GetLength(1)){
							var ss=line.Substring(j).ToLower();
							if (ss.StartsWith("x")){
								map.map_data[i,j]=1;
								j++;
							}
							else if (ss.StartsWith("c")){
								map.map_data[i,j]=2;
								j++;
							}
							else 
								j++;
						}
						i++;
						j=0;
					}
					
					
					map_db.Maps.Add(map);
				}
			}
		}

#elif UNITY_STANDALONE

		checkFolder(path);

		var files=Directory.GetFiles(path);

		foreach (var f in files){
			var Xdoc=new XmlDocument();
			Debug.Log("f "+f);
			Xdoc.Load(f);

			//read xml

			var root=Xdoc["Root"];

			foreach (XmlNode node in root){
				if (node.Name=="Map"){
					var map=new MapData(5,5);

					var spl=node.InnerText.Replace(" ","").Replace("\r","").Split('\n');
					int i=0,j=0;
					foreach (var line in spl){
						if (line=="") continue;
						while (j<map.map_data.GetLength(1)){
							var ss=line.Substring(j).ToLower();
							if (ss.StartsWith("x")){
								map.map_data[i,j]=1;
								j++;
							}
							else if (ss.StartsWith("c")){
								map.map_data[i,j]=2;
								j++;
							}
							else 
								j++;
						}
						i++;
						j=0;
					}


					map_db.Maps.Add(map);
				}
			}
		}
#endif

	}
	
	void writeXML(){

		checkFolder("Data/Maps");

		var Xdoc=new XmlDocument();
		var root = Xdoc.CreateElement("Root");
		Xdoc.AppendChild(root);

		var map=addElement(root,"Map");

		map.InnerText="\n" +
					  "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n" +
		              "X---------\n";

		

		Xdoc.Save("Data/Maps/Test.xml");

	}
	


	           
//getting values
   public static string getStr(XmlElement element,string name){
		if (element[name]==null) return "";
		return element[name].InnerText;
	}
	
	public static int getInt(XmlElement element,string name){
		if (element[name]==null) return 0;
		return int.Parse(element[name].InnerText);
	}
	
	public static float getFlt(XmlElement element,string name){
		if (element[name]==null) return 0f;
		return float.Parse(element[name].InnerText);
	}
	
	//adding elements
	public static XmlElement addElement(XmlElement element,string name){
		var node=element.OwnerDocument.CreateElement(name);
		element.AppendChild(node);
		return node;
	}
	
	public static XmlElement addElement(XmlElement element,string name,string val){
		var node=element.OwnerDocument.CreateElement(name);
		node.InnerText=val;
		element.AppendChild(node);
		return node;
	}
	
	public static XmlElement addElement(XmlElement element,string name,int val){
		return addElement(element,name,val.ToString());
	}
	
	public static XmlElement addElement(XmlElement element,string name,float val){
		return addElement(element,name,val.ToString());
	}
	
	//adding attributes	
	public static XmlAttribute addAttribute(XmlElement element,string name,string val){
		var att=element.OwnerDocument.CreateAttribute(name);
		att.Value=val;
		element.Attributes.Append(att);
		return att;
	}
	
	public static XmlAttribute addAttribute(XmlElement element,string name,int val){
		return addAttribute(element,name,val.ToString());
	}
	
	
	//adding comments
	public static XmlAttribute addComment(XmlElement element,string name,string val){
		var att=element.OwnerDocument.CreateAttribute(name);
		att.Value=val;
		element.Attributes.Append(att);
		return att;
	}
	
	
	public static XmlComment addComment(XmlElement element,string comment){
		var n=element.OwnerDocument.CreateComment(comment);
		element.AppendChild(n);
		return n;
	}
	
	//finders
	public static List<XmlNode> getChildrenByTag(XmlNode node,string tag){
		List<XmlNode> nodes=new List<XmlNode>();
		foreach (XmlNode n in node){
			if (n.Name==tag){
				nodes.Add(n);
			}
		}
		return nodes;
	}
	
	
	public static void readAuto(XmlElement element,object obj){
		foreach (var f in obj.GetType().GetFields()){
			if (f.IsPublic){
				if (element[f.Name]!=null){
					f.SetValue(obj,Convert.ChangeType(element[f.Name].InnerText,f.FieldType));
				}
			}
		}
	}
	
	public static void writeAuto(XmlElement element,object obj){
		foreach (var f in obj.GetType().GetFields()){
			addElement(element,f.Name,f.GetValue(obj).ToString());
		}
	}
	
	/// <summary>
	/// Reads an automatically created xml doc and assigns its innards to the object.
	/// </summary>
	public static void readAutoFile(string path,string folder,string file,object obj){
		if (folder!="")
			folder=@"\"+folder;
		if (Directory.Exists(path+folder)){
			file=@"\"+file+".xml";
			
			var Xdoc=new XmlDocument();
			Xdoc.Load(path+folder+file);
			
			var root=Xdoc["Stats"];
			
			readAuto(root,obj);
		}
		
	}
	/// <summary>
	/// Takes an object and autimagically transform it into an xml doc
	/// Public fields became elements with the value as innerText.
	/// </summary>
	public static void writeAutoFile(string path,string folder,string file,object obj){
		if (folder!="")
			folder=@"\"+folder;
		checkFolder(path+folder);
		
		file=@"\"+file+".xml";
		var Xdoc=new XmlDocument();
		var root=Xdoc.CreateElement("Stats");
		
		writeAuto(root,obj);
		
		Xdoc.AppendChild(root);
		Xdoc.Save(path+folder+file);
	}
	
	/// <summary>
	/// Creates a folder if it doesn't exist
	/// </summary>
	public static void checkFolder(string path)
	{
		if (!Directory.Exists(path)){
			Directory.CreateDirectory(path);
		}
	}
	
	public static bool checkFile(string path){
		return File.Exists(path);
	}
}

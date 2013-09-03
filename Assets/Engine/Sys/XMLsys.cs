using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Linq;

public class XMLsys : MonoBehaviour {
	
	
	//engine logic
	void Awake () {
		readXML();
	}
	
	void OnDestroy(){
		writeXML();
	}
	
	//game logic
	void readXML(){
		
		
		
	}
	
	void writeXML(){
		
		
	}
	
	
	//subs
	string getStr(XmlElement element,string name){
		if (element[name]==null) return "";
		return element[name].InnerText;
	}
	
	int getInt(XmlElement element,string name){
		if (element[name]==null) return 0;
		return int.Parse(element[name].InnerText);
	}
	
	float getFlt(XmlElement element,string name){
		if (element[name]==null) return 0f;
		return float.Parse(element[name].InnerText);
	}
	
	void addElement(XmlElement element,string name,string val){
		var node=element.OwnerDocument.CreateElement(name);
		node.InnerText=val;
		element.AppendChild(node);
	}
	
	void addElement(XmlElement element,string name,int val){
		addElement(element,name,val.ToString());
	}
	
	void addElement(XmlElement element,string name,float val){
		addElement(element,name,val.ToString());
	}
		
	void readAuto(XmlElement element,object obj){
		foreach (var f in obj.GetType().GetFields()){
			if (f.IsPublic){
				if (element[f.Name]!=null){
					f.SetValue(obj,Convert.ChangeType(element[f.Name].InnerText,f.FieldType));
				}
			}
		}
	}
	
	void writeAuto(XmlElement element,object obj){
		foreach (var f in obj.GetType().GetFields()){
			addElement(element,f.Name,f.GetValue(obj).ToString());
		}
	}
	
	void readAutoFile(string path,string folder,string file,object obj){
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
	
	void writeAutoFile(string path,string folder,string file,object obj){
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
	void checkFolder(string path){
		if (!Directory.Exists(path)){
			Directory.CreateDirectory(path);
		}
	}
}

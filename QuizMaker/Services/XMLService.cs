using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace QuizMaker.Services
{
    internal class XMLService
    {
        private string XmlPath;
        //base directory path
        private string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private XmlDocument activeDoc;

        public XMLService(params string[] xmlPath)
        {
            XmlPath = BuildPath(xmlPath);
            activeDoc = ReadXML()!;
        }

        private string BuildPath(params string[] xmlPath)
        {
            // Create an array that includes the base directory and the xmlPath elements
            string[] fullPath = new string[xmlPath.Length + 1];
            fullPath[0] = _baseDirectory;
            Array.Copy(xmlPath, 0, fullPath, 1, xmlPath.Length);

            // Combine all the elements into a single path
            return Path.Combine(fullPath);
        }

        public XmlNode? GetNodeByName(string nodePath)
        {
            // Replace dot notation with XPath separator
            string xPath = "//" + nodePath.Replace('.', '/');

            // Query the XML document using the constructed XPath
            return activeDoc.SelectSingleNode(xPath);
        }

        //GetNodeByName(Drinks.Drink.Ingerdients)

        public XmlNode? GetNodeByName(XmlNode rootNode, string nodePath)
        {
            // Replace dot notation with XPath separator
            string xPath = ".//" + nodePath.Replace('.', '/');

            // Query the provided root node using the constructed XPath
            return rootNode.SelectSingleNode(xPath);
        }

        public List<T> GetOptions<T>(string nodeName, Func<string, T> transform)
        {
            var node = GetNodeByName(nodeName);
            if (node == null) throw new Exception($"Node '{nodeName}' not found.");

            return node.ChildNodes.Cast<XmlNode>()
                .Select(childNode => transform(childNode.InnerText))
                .ToList();
        }

        public List<TModel> ToModel<TModel>(string nodeName, Func<XmlNode, TModel> transform)
        {
            XmlNode? parentNode = GetNodeByName(nodeName);

            if (parentNode == null)
                throw new Exception($"Node '{nodeName}' not found.");

            return parentNode.ChildNodes.Cast<XmlNode>()
                .Select(transform)
                .ToList();
        }

        public void CreateXML()
        {
            if (!File.Exists(XmlPath))
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(XmlPath, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Drinks");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }
            }
        }

        // Pseudo-method for creating a node, assuming an abstracted creation process
        public XmlNode CreateNode(string name, string innerText = null)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement(name);
            if (innerText != null)
            {
                node.InnerText = innerText;
            }
            return node;
        }

        public void CreateNodeFromModel<TModel>(TModel model)
        {
            // Initialize the XmlSerializer with the type of the model
            XmlSerializer serializer = new XmlSerializer(typeof(TModel));

            // Use StringWriter for the serialization process
            using (StringWriter stringWriter = new StringWriter())
            {
                // Serialize the model into the StringWriter
                serializer.Serialize(stringWriter, model);

                // Load the serialized model string into an XmlDocument
                XmlDocument serializedDoc = new XmlDocument();
                serializedDoc.LoadXml(stringWriter.ToString());

                // Import the serialized document's root element into the target document
                XmlNode importedNode = activeDoc.ImportNode(serializedDoc.DocumentElement, true);

                // Append the imported node directly to the document's root element or another specific node
                activeDoc.DocumentElement.AppendChild(importedNode);

                // Save the active document
                activeDoc.Save(XmlPath);
            }
        }

        //read xml file
        public XmlDocument? ReadXML()
        {
            XmlDocument xmlDoc = new XmlDocument();

            using (FileStream fileStream = new FileStream(XmlPath, FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(fileStream);
            }
            return xmlDoc;
        }
    }
}

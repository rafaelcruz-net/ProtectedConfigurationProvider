using CriptoProtectedConfigurationProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CriptoConsole
{
    class Program
    {
        static TripleDESProtectedConfigurationProvider provider = new TripleDESProtectedConfigurationProvider();

        static void Main(string[] args)
        {
            var path = String.Empty;
             while (String.IsNullOrEmpty(path))
             {
                 Console.WriteLine("Selecione o caminho do Web.config");
                 path = Console.ReadLine();
             }

             ProtectedConfiguration(path);

            Console.WriteLine("Web Config criptografado com sucesso");
            Console.Read();


        }

        private static void ProtectedConfiguration(string path)
        {

            XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(path);

            //Cria a seção criptografada
            var xmlContent = @"<configProtectedData><providers><add name='TripleDESProtectedConfigurationProvider' type='CriptoProtectedConfigurationProvider.TripleDESProtectedConfigurationProvider, CriptoProtectedConfigurationProvider' /></providers></configProtectedData>";

            XElement element = XDocument.Parse(xmlContent).Root;

            var xmlProtected = new XmlDocument();
            xmlProtected.LoadXml(element.ToString());
            var nodeProtected = xmlProtected.FirstChild;

            XmlNode nodeConfiguration = doc.SelectSingleNode("/configuration");
            var importNodeProvider = doc.ImportNode(nodeProtected, true);
            nodeConfiguration.InsertAfter(importNodeProvider, nodeConfiguration.FirstChild);


            // Pega a seção que será criptografada
            XmlNode node = doc.SelectSingleNode("/configuration/connectionStrings");

            //Encripta os dados do nó
            var criptoNode = provider.Encrypt(node);

            //Deleta todos os childs nodes e attributos
            node.RemoveAll();

            //Importa o no criptografado
            var importNodeCripto = doc.ImportNode(criptoNode, true);

            //Insere no nó de connection string
            node.InsertAfter(importNodeCripto, node.FirstChild);


            //Insere o atributo referencia para o configuration provider
            XmlAttribute attr = doc.CreateAttribute("configProtectionProvider");
            attr.Value = "TripleDESProtectedConfigurationProvider";
            node.Attributes.Append(attr);

            doc.Save(path);
        }
    }
}

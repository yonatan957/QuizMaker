using QuizMaker.Services;
using System.Xml;
using System.Xml.Linq;

namespace QuizMaker
{
    internal class Program
    {

        public static void foo1()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Data.xml");
            XmlElement item = xmlDoc.CreateElement("item");
            XmlElement question = xmlDoc.CreateElement("question");
            XmlElement answer = xmlDoc.CreateElement("answer");
            Console.WriteLine("Enter your question");
            question.InnerText = Console.ReadLine();
            Console.WriteLine("Enter your answer");
            answer.InnerText = Console.ReadLine();
            item.AppendChild(question);
            item.AppendChild(answer);
            xmlDoc.DocumentElement.AppendChild(item);
            xmlDoc.Save("C:\\Users\\yonat\\source\\repos\\QuizMaker\\QuizMaker\\Data.xml");
        }
        
        public static void foo2()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Data.xml");
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                list.Add(node);
                XmlNode nodequestion = node.SelectSingleNode("question");
                Console.WriteLine(nodequestion.InnerText.ToString().Trim());
                string answer = Console.ReadLine();
                XmlNode nodeanswer = node.SelectSingleNode("answer");
                if (nodeanswer.InnerText.ToString().Trim() == answer)
                {
                    Console.WriteLine("great!!!");
                }
                else
                {
                    Console.WriteLine($"you was wrong!!! the real answer is \n {nodeanswer.InnerText.ToString().Trim()}");
                }
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                XmlDocument xmlDoc = new XmlDocument();
                Console.WriteLine("choose 1 to write questions, choose 2 to do quiz");
                string choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        foo1();
                        break;
                    case "2":
                        foo2();
                        break;
                    default:
                        Console.WriteLine("invalid choose");
                        break;
                }
            }
        }       
    }
}

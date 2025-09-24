
namespace GenerativeAI.Console
{
    using System;
    internal class Program
    {
 
        public static async Task Main(string[] args)
        {


            string filePath = "C:\\Amauri\\GitHub\\GeminiKey.txt";
            string apiKey = File.ReadAllText(filePath).Replace("\\\\", "\\");
 
 
            var model = new GenerativeModel(apiKey: apiKey, model: "gemini-1.5-flash-latest");
 
            System.Console.WriteLine("Qual é a sua pergunta para a Gemini?");
            string pergunta = Console.ReadLine();

     
            var response = await model.GenerateContentAsync(pergunta);
 
            Console.WriteLine("\n Resposta do Gemini: ");
            Console.WriteLine(response.Text);
        }
    }
}

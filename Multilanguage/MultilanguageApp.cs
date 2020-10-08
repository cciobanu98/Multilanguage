using Multilanguage.Application.Abstract;
using Multilanguage.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Multilanguage
{
    public class MultilanguageApp
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizerService _stringLocalizerService;

        public MultilanguageApp(IUnitOfWork unitOfWork, IStringLocalizerService stringLocalizerService)
        {
            _unitOfWork = unitOfWork;
            _stringLocalizerService = stringLocalizerService;
        }

        public async Task Run()
        {
            while(true)
            {
                DisplayMenu();
                var options = Console.ReadLine();
                switch(options)
                {
                    case "1": await AddLanguage();
                        break;
                    case "2": await Translate();
                        break;
                    case "9":
                        return;
                    default:
                        break;
                }
            }
        }
        private async Task Translate()
        {
            Console.WriteLine("Translate");
            Console.Write("Text");
            var text = Console.ReadLine();
            _stringLocalizerService.WithCulture(new System.Globalization.CultureInfo("es"));
            var translated = await _stringLocalizerService.Get(text);
            Console.WriteLine(translated);
            Console.ReadLine();
        }
        private async Task AddLanguage()
        {
            Console.WriteLine("Add a language");
            Console.Write("Code: ");
            var code = Console.ReadLine();
            Console.Write("Name: ");
            var name = Console.ReadLine();
            var language = new Language() { Code = code, Name = name };
            _unitOfWork.Set<Language>().Add(language);
            await _unitOfWork.SaveChangesAsync();
        }
        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("1.Add a language");
            Console.WriteLine("2.Translate");
            Console.WriteLine("9.Exit");
        }

        private async Task<string> GetInputAsync()
        {
            return await Task.Run(() => Console.ReadLine());
        }
    }
}

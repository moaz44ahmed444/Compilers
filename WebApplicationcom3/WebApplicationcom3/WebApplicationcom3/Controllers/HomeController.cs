using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationcom3.Models;

namespace WebApplicationcom3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public HomeController(ILogger<HomeController> logger , Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("Home")]
        public IActionResult show(IFormFile file ,string text=null)
        {
            var dir = _env.ContentRootPath;
            string path = null;
            if (file != null)
            {
                using (var fileStream = new FileStream(Path.Combine(dir, file.FileName), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }
                path = file.FileName;
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            string text2="";
            if (text != null)
            {
                text2 = Replace(text);
                
            }
            list = main(text2,path);

            if (list.Count > 0)
            {
                TempData["print"] = list;
            }
            return RedirectToAction("Index");
        }
        public string get(string x)
        {
            return x;
        }
        public List<string> main(string x = "",string path =null)
        {
            string text2="";
            List<string> print = new List<string>();
            if (x != "")
                text2 = x;
            else
            {
                var text = System.IO.File.ReadAllText(path);
                if (string.IsNullOrEmpty(text))
                    return null;
                text2 = text;
            }
            lexer scanner = new lexer(text2);
            List<token> tokens = scanner.lex();
            int ErrorsNumber = 0;
            foreach (token t in tokens)
            {
                if (t.Type == tokenType.BadToken)
                {
                    ErrorsNumber++;
                    print.Add($"Line: {t.Position} Error in token text: {t.Text}");
                }
                else
                    print.Add($"Line: {t.Position} TokenText: {t.Text} TokenType: {t.Type}");
            }
            print.Add("\n\n\n------------------------");
            print.Add($"Total number of errors: {ErrorsNumber}");
            return print;
        }
        public string Replace(string text)
        {
            string text2 = text.Replace("<p>", "");
            text2 = text2.Replace("</p>", "\r\n");
            text2 = text2.Replace("&lt;", "<");
            text2 = text2.Replace("&gt;", "<");
            text2 = text2.Replace("amp;", "");
            text2 = text2.Replace("br", "");
            text2 = text2.Replace("<>", "");
            return text2;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
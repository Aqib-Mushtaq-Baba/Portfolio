using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();

        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EmailSent(string UserName, string UserEmail, string UserSubject, string UserMessage)
        {
            var mime = new MimeMessage();

            mime.From.Add(new MailboxAddress(UserName, "aqibmushtaqbaba@gmail.com"));
            //mime.From.Add(new MailboxAddress("admin", "aqibmushtaqbaba@gmail.com"));
            //mime.To.Add(new MailboxAddress("Receiver Name", email));
            mime.To.Add(new MailboxAddress("Admin as Receiver", "aqibmushtaqbaba@gmail.com"));

            mime.Subject = UserSubject;
            mime.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = UserMessage
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("aqibmushtaqbaba@gmail.com", "iepu oisl aeci aaro");

                smtp.Send(mime);
                smtp.Disconnect(true);
            }
            return RedirectToAction("index");
        }


        public IActionResult DownloadFile()
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string outputFilePath = Path.Combine(webRootPath, "pdf", "Resume_Aqib.pdf");

            if (!System.IO.File.Exists(outputFilePath))
            {
                // Return a 404 Not Found error if the file does not exist
                return NotFound();
            }

            var fileInfo = new System.IO.FileInfo(outputFilePath);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment;filename=\"" + fileInfo.Name + "\"");
            Response.Headers.Add("Content-Length", fileInfo.Length.ToString());

            // Send the file to the client
            return File(System.IO.File.ReadAllBytes(outputFilePath), "application/pdf", fileInfo.Name);
        }
    }
}

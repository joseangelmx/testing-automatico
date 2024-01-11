using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCvSharp.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

public class Program
{
    static Registro[] registros = new Registro[20];
    static int contador = 0;
    static async Task Main()
    {

        int conveyorWidth = 120;
        int conveyorLength = 10;
        int objectLength = 5;
         int sectionWidth = conveyorWidth / 6;
         int objectPosition = 0;
         int steps = 0;
        Random random = new Random();
        IWebDriver driver = new ChromeDriver();
        DrawConveyor("");
        // Abrir la página de Google
        driver.Navigate().GoToUrl("http://localhost:3000/");
        driver.Manage().Window.Maximize();
        IWebElement html = driver.FindElement(By.TagName("html"));
        IWebElement email = driver.FindElement(By.Id("emailLog"));
        email.SendKeys("admin@luxottica.com");
        IWebElement passworrd = driver.FindElement(By.Id("passwordLog"));
        passworrd.SendKeys("3eJ0eMN@*wl9+");
        IWebElement login = driver.FindElement(By.Id("login-button"));
        login.Click();
        System.Threading.Thread.Sleep(4000);
        IAlert alert = driver.SwitchTo().Alert();
        alert.Dismiss();
        System.Threading.Thread.Sleep(1000);

        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        string divertCode = "";
        string zoneDivert = "";

        for (int i = 0; i < 10; i++)
        {

            IWebElement confirmMessage = driver.FindElement(By.Id("confirmMessage"));
            IWebElement zone = driver.FindElement(By.Id("responseVirtualTote"));
            js.ExecuteScript("window.scrollBy(0, 420);");
            IWebElement responseDivertCode = driver.FindElement(By.Id("responseDivertCode"));
            string toteLpnCode = GenerateRandomCode();
            IWebElement camId = driver.FindElement(By.Id("camIdNewTote"));
            camId.SendKeys("Cam01");
            IWebElement trackingId = driver.FindElement(By.Id("trackingIdNewTote"));
            trackingId.SendKeys("1");
            IWebElement toteLpn = driver.FindElement(By.Id("toteLpnNewTote"));
            toteLpn.SendKeys(toteLpnCode);
            System.Threading.Thread.Sleep(1000);

            IWebElement newToteButton = driver.FindElement(By.Id("newToteButton"));
            newToteButton.Click();
            System.Threading.Thread.Sleep(3000);

            for (int j = 4; j <= 8; j++)
            {
                LimpiarCamposDivert();
                js.ExecuteScript("window.scrollBy(0, 90);");
                System.Threading.Thread.Sleep(500);
                string camIdValue = $"Cam{j:00}";
                IWebElement zoneDivertElement = driver.FindElement(By.Id("responseZoneId"));
                // Ingresa el valor actual de CamId
                IWebElement camIdDivert = driver.FindElement(By.Id("camIdDivert"));
                camIdDivert.SendKeys(camIdValue);
                IWebElement trackingIdDivert = driver.FindElement(By.Id("trackingIdDivert"));
                trackingIdDivert.SendKeys("1");
                IWebElement toteLpnDivert = driver.FindElement(By.Id("toteLpnDivert"));
                toteLpnDivert.SendKeys(toteLpnCode);
                IWebElement divert = driver.FindElement(By.Id("divertButton"));
                divert.Click();
                MoveConveyor(camIdValue + " TOTE LPN:" + toteLpnCode);
                System.Threading.Thread.Sleep(3000);
                zoneDivert = zoneDivertElement.Text;
                divertCode = responseDivertCode.Text;
                js.ExecuteScript("window.scrollBy(0, -90);");
                System.Threading.Thread.Sleep(500);
                if (divertCode == "2")
                {
                    IWebElement camIdConfirm = driver.FindElement(By.Id("camIdDivertConfirm"));
                    camIdConfirm.SendKeys(camIdValue);
                    IWebElement trackingIdConfirm = driver.FindElement(By.Id("trackingIdDivertConfirm"));
                    trackingIdConfirm.SendKeys("1");
                    IWebElement divertCodeConfirm = driver.FindElement(By.Id("divertCodeConfirm"));
                    divertCodeConfirm.SendKeys("100");
                    IWebElement confirm = driver.FindElement(By.Id("divertConfirmButton"));
                    confirm.Click();
                    Console.Clear();
                    objectPosition = 0;
                    Console.WriteLine("TOTE LPN: " + toteLpnCode + " DIVERT IN " + camIdValue);
                    AgregarRegistro(toteLpnCode, camIdValue, "DIVERT", zoneDivert);
                    System.Threading.Thread.Sleep(3000);
                    js.ExecuteScript("window.scrollBy(0, -490);");
                    break;
                }
            }

            // Verifica si el resultado no fue 2 después de iterar por Cam04 a Cam09
            if (divertCode != "2")
            {
                IWebElement camIdBorder = driver.FindElement(By.Id("camIdDivertBorder"));
                camIdBorder.SendKeys("Cam10");
                IWebElement trackingIdBorder = driver.FindElement(By.Id("trackingIdDivertBorder"));
                trackingIdBorder.SendKeys("1");
                IWebElement toteLpnBorder = driver.FindElement(By.Id("toteLpnDivertBorder"));
                toteLpnBorder.SendKeys(toteLpnCode);
                js.ExecuteScript("window.scrollBy(0, 80);");
                System.Threading.Thread.Sleep(500);
                IWebElement border = driver.FindElement(By.Id("borderPickingButton"));
                border.Click();
                
                System.Threading.Thread.Sleep(4000);
                js.ExecuteScript("window.scrollBy(0, 700);");
                System.Threading.Thread.Sleep(500);
                IWebElement camIdTransfer = driver.FindElement(By.Id("camIdDivertTransfer"));
                camIdTransfer.SendKeys("Cam02");
                IWebElement trackingIdTransfer = driver.FindElement(By.Id("trackingIdDivertTransfer"));
                trackingIdTransfer.SendKeys("1");
                IWebElement toteLpnTransfer = driver.FindElement(By.Id("toteLpnDivertTransfer"));
                toteLpnTransfer.SendKeys(toteLpnCode);
                IWebElement transfer = driver.FindElement(By.Id("transferInboundButton"));
                
                System.Threading.Thread.Sleep(500);
                transfer.Click();
                Console.Clear();
                objectPosition = 0;
                Console.WriteLine("TOTE LPN: "+ toteLpnCode +" LINE COUNT");
                AgregarRegistro(toteLpnCode,"", "Line Count", zoneDivert);
                System.Threading.Thread.Sleep(3000);
                js.ExecuteScript("window.scrollBy(0, -900);");

            }


            System.Threading.Thread.Sleep(1000);
            js.ExecuteScript("window.scrollBy(0, -400);");
            LimpiarCampos();

        }
        Console.WriteLine("Presiona Enter para salir...");
        Console.Clear() ;
        MostrarRegistros();
        Console.ReadLine();
        void LimpiarCamposDivert()
        {

            IWebElement camIdDivert = driver.FindElement(By.Id("camIdDivert"));
            IWebElement trackingIdDivert = driver.FindElement(By.Id("trackingIdDivert"));
            IWebElement toteLpnDivert = driver.FindElement(By.Id("toteLpnDivert"));

            IWebElement camIdConfirm = driver.FindElement(By.Id("camIdDivertConfirm"));
            IWebElement trackingIdConfirm = driver.FindElement(By.Id("trackingIdDivertConfirm"));
            IWebElement divertCodeConfirm = driver.FindElement(By.Id("divertCodeConfirm"));


            camIdDivert.Clear();
            trackingIdDivert.Clear();
            toteLpnDivert.Clear();

            camIdConfirm.Clear();
            trackingIdConfirm.Clear();
            divertCodeConfirm.Clear();
        }
        void LimpiarCampos()
        {
            IWebElement camId = driver.FindElement(By.Id("camIdNewTote"));
            IWebElement trackingId = driver.FindElement(By.Id("trackingIdNewTote"));
            IWebElement toteLpn = driver.FindElement(By.Id("toteLpnNewTote"));
            IWebElement camIdDivert = driver.FindElement(By.Id("camIdDivert"));
            IWebElement trackingIdDivert = driver.FindElement(By.Id("trackingIdDivert"));
            IWebElement toteLpnDivert = driver.FindElement(By.Id("toteLpnDivert"));
            IWebElement camIdConfirm = driver.FindElement(By.Id("camIdDivertConfirm"));
            IWebElement trackingIdConfirm = driver.FindElement(By.Id("trackingIdDivertConfirm"));
            IWebElement divertCodeConfirm = driver.FindElement(By.Id("divertCodeConfirm"));
            IWebElement camIdBorder = driver.FindElement(By.Id("camIdDivertBorder"));
            IWebElement trackingIdBorder = driver.FindElement(By.Id("trackingIdDivertBorder"));
            IWebElement toteLpnBorder = driver.FindElement(By.Id("toteLpnDivertBorder"));
            IWebElement camIdTransfer = driver.FindElement(By.Id("camIdDivertTransfer"));
            IWebElement trackingIdTransfer = driver.FindElement(By.Id("trackingIdDivertTransfer"));
            IWebElement toteLpnTransfer = driver.FindElement(By.Id("toteLpnDivertTransfer"));

            camId.Clear();
            trackingId.Clear();
            toteLpn.Clear();

            camIdDivert.Clear();
            trackingIdDivert.Clear();
            toteLpnDivert.Clear();

            camIdConfirm.Clear();
            trackingIdConfirm.Clear();
            divertCodeConfirm.Clear();

            camIdBorder.Clear();
            trackingIdBorder.Clear();
            toteLpnBorder.Clear();

            camIdTransfer.Clear();
            trackingIdTransfer.Clear();
            toteLpnTransfer.Clear();
        }
        string GenerateRandomCode()
        {
            const string digits = "0123456789";
            string randomNumberPart = new string(Enumerable.Repeat(digits, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return ObtenerLetraAleatoria() + randomNumberPart;
        }

        char ObtenerLetraAleatoria()
        {
            string letras = "HKTN";
            int indice = random.Next(0, letras.Length);
            return letras[indice];
        }

         void MoveConveyor(string message)
        {

            objectPosition += 19;
            if (objectPosition + objectLength > conveyorWidth - 1)
            {
                objectPosition = 0;
            }
            DrawConveyor(message);
        }


        void DrawConveyor(string message)
        {
            Console.Clear();
            MostrarRegistros();
            for (int i = 0; i < conveyorLength; i++)
            {
                for (int j = 0; j < conveyorWidth; j++)
                {
                    if (i == 0 || i == conveyorLength - 1 || j == 0 || j == conveyorWidth - 1)
                    {
                        Console.Write("#");
                    }
                    else if ((j >= objectPosition && j < objectPosition + objectLength) && (i == conveyorLength - 1))
                    {
                        int section = j / sectionWidth;
                        string camName = GetCamName(section + 4); // Sección comienza en Cam04
                        Console.Write(camName);
                        j += objectLength - 1;
                    }
                    else if (j >= objectPosition && j < objectPosition + objectLength)
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Camara actual: " + message);
        }
    }
    static string GetCamName(int camNumber)
    {
        return "Cam" + camNumber.ToString("D2");
    }
    static void MostrarRegistros()
    {
        Console.WriteLine("Registros almacenados:");
        Console.WriteLine("-----------------------");
        for (int i = 0; i < contador; i++)
        {
            Console.WriteLine($"Registro {i + 1}:");
            Console.WriteLine($"TOTELPN: {registros[i].TOTELPN}");
            Console.WriteLine($"CAMID: {registros[i].CAMID}");
            Console.WriteLine($"ACTION: {registros[i].ACTION}");
            Console.WriteLine($"ZoneDivert: {registros[i].ZoneDivert}");
            Console.WriteLine($"TIMESTAMP: {registros[i].TIMESTAMP}");
            Console.WriteLine("-----------------------");
        }
    }

    static void AgregarRegistro(string totelpn, string camid, string action,string zone)
    {
        if (contador < registros.Length)
        {
            registros[contador] = new Registro
            {
                TOTELPN = totelpn,
                CAMID = camid,
                ACTION = action,
                TIMESTAMP = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                ZoneDivert = zone
            };
            contador++;
        }
        else
        {
            Console.WriteLine("No hay espacio para más registros.");
        }
    }
}


class Registro
{
    public string TOTELPN { get; set; }
    public string CAMID { get; set; }
    public string ACTION { get; set; }
    public string TIMESTAMP { get; set; }
    public string ZoneDivert { get; set; }
}


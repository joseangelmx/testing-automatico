using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_auto
{
    public class Funciones
    {
        IWebDriver driver = new ChromeDriver();
        static int conveyorWidth = 120;
        static int conveyorLength = 10;
        static int objectLength = 5;
        static int sectionWidth = conveyorWidth / 6;
        static int objectPosition = 0;
        static int steps = 0;

        static Registro[] registros = new Registro[20];
        static int contador = 0;
        public void LimpiarCampos()
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
        public static void MostrarRegistros()
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

        public void DrawConveyor(string message)
        {
            Console.Clear();
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
            MostrarRegistros();
        }
        public static void AgregarRegistro(string totelpn, string camid, string action, string zone)
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
        public void LimpiarCamposDivert()
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
        public void Object0()
        {
            objectPosition=0;
        }
        public void MoveConveyor(string message)
        {

            objectPosition += 19;
            if (objectPosition + objectLength > conveyorWidth - 1)
            {
                objectPosition = 0;
            }
            DrawConveyor(message);
        }

        public static string GetCamName(int camNumber)
        {
            return "Cam" + camNumber.ToString("D2");
        }
        class Registro
        {
            public string TOTELPN { get; set; }
            public string CAMID { get; set; }
            public string ACTION { get; set; }
            public string TIMESTAMP { get; set; }
            public string ZoneDivert { get; set; }
        }

    }
}

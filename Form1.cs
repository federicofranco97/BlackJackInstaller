using BlackJackInstaller.GUI.Properties;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJackInstaller.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
            btnInstallCli.Enabled = false;
        }

        private void btnInstallDepends_Click(object sender, EventArgs e)
        {
            if (Instalando) return;
            InstallDependencies();
        }


        private bool Instalando = false;//Indicador para saber si esta en procesode instalacion
        private string[] libraries = new string[] { "playsound", "mttkinter", "Pillow","pymitter" };//Listado de librerias
        private string PythonPath = "";//Variable que almacena el path de instalacion de python
        private string PythonVersion = "";

        /// <summary>
        /// Metodo encargado de ejecutar en consola los comandos suministrados en el parametro
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="path"></param>
        /// <param name="Hidden"></param>
        /// <returns></returns>
        private bool RunCommand(string Command, string path="", bool Hidden=true)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = Hidden ? System.Diagnostics.ProcessWindowStyle.Hidden : System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = path;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = Hidden ? "/C  " + Command :  "/K  " + Command;
            var standardOutput = new StringBuilder();
            using (var process = Process.Start(startInfo))
            {
                while (!process.HasExited)
                {
                    standardOutput.Append(process.StandardOutput.ReadToEnd());
                }
                standardOutput.Append(process.StandardOutput.ReadToEnd());
            }
            if (standardOutput.ToString().ToLower().Contains("error") || string.IsNullOrEmpty(standardOutput.ToString()))
            {
                if(!string.IsNullOrEmpty(standardOutput.ToString())) txtProcess.Text += standardOutput.ToString() + Environment.NewLine;
                return false;
            }
            else if (standardOutput.ToString().ToLower().Contains("already"))
            {
                return true;
            }
            return true;
        }

        /// <summary>
        /// Metodo que ejecuta los sccripts de python utlizando el path de instalacion como entorno
        /// </summary>
        /// <param name="ScriptName"></param>
        /// <param name="PythonPath"></param>
        private void RunPythonSCript(string ScriptName,string PythonPath)
        {
            var info = new ProcessStartInfo();
            info.FileName = PythonPath;
            info.Arguments = ScriptName;
            info.WorkingDirectory = folderBrowserDialog1.SelectedPath + "\\blackjack";            
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Normal;
            txtProcess.Text += PythonPath + Environment.NewLine;
            txtProcess.Text += ScriptName + Environment.NewLine;
            txtProcess.Text += info.WorkingDirectory + Environment.NewLine;
            string errors = "";
            string result2 = "";
            Task.Run(() =>
            {
                using (Process process = Process.Start(info))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        txtProcess.Text += result + Environment.NewLine;
                        errors = process.StandardError.ReadToEnd();
                        result2 = process.StandardOutput.ReadToEnd();
                    }
                    txtProcess.Text += errors + Environment.NewLine;
                    txtProcess.Text += result2 + Environment.NewLine;
                }
            });
            
        }

        /// <summary>
        /// Metodo que instala las dependencias necesitadas para correr el cliente.
        /// </summary>
        public async void InstallDependencies()
        {
            Instalando = true;
            txtProcess.Text += "Instalando Dependencias...."+Environment.NewLine;
            var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            var Discos = System.IO.DriveInfo.GetDrives();
            bool HuboErrores = false;
            bool HayPython = false;
            foreach (var disco in Discos)
            {
                PythonPath = disco.Name + @"Users\" + userName + @"\AppData\Local\Programs\Python\";
                if (Directory.Exists(PythonPath))
                {
                    HayPython = true;
                    string[] VersionesPython = new string[] { "Python38-32", "Python39" };
                    bool encontroVersion = false;
                    foreach (var version in VersionesPython)
                    {
                        if (Directory.Exists(PythonPath + version))
                        {
                            encontroVersion = true;
                            PythonVersion = version + "\\python.exe";
                            foreach (var item in libraries)
                            {
                                var Success = await Task.Run(() =>
                                {
                                    return RunCommand("pip install " + item, PythonPath + version+"\\Scripts");
                                    //return RunCommand("pip install " + item, PythonPath + "Python38-32\\Scripts");
                                });
                                if (Success)
                                {
                                    var Pos = libraries.ToList().IndexOf(item) + 1;
                                    txtProcess.Text += "Dependencia " + Pos + " de " + libraries.Length + " instalada. [" + item + "]" + Environment.NewLine;
                                }
                                else
                                {
                                    txtProcess.Text += "Ocurrio un error al querer instalar la dependencia " + item + "" + Environment.NewLine;
                                    HuboErrores = true;
                                }
                            }
                            break;
                        }            
                        
                    }
                    if(!encontroVersion) txtProcess.Text += "No se encontro ninguna version de Python compatible con el juego" + Environment.NewLine;
                    break;
                }
            }
            if (!HayPython)
            {
                txtProcess.Text += "No se encontro una instalacion de python activa por favor proceda a isntalarlo." + Environment.NewLine;
                HuboErrores = true;
            }
            if (HuboErrores) txtProcess.Text += "Instalacion Incompleta, hubo dependencias que presentaron errores." + Environment.NewLine;
            else txtProcess.Text += "Instalacion Completa" + Environment.NewLine;
            btnInstallCli.Enabled = true;
            Instalando = false;
        }

        /// <summary>
        /// Metodo que llama al RunPython para ejecutar el acrhivo del cliente
        /// </summary>
        private async void StartClient()
        {
            try
            {
                txtProcess.Text += "Inicializando cliente... " + Environment.NewLine;
                RunPythonSCript(folderBrowserDialog1.SelectedPath + "\\blackjack\\Cliente.py", PythonPath + PythonVersion);
                txtProcess.Text += "Se inicio el cliente correcamente. El instalador se cerrara en 10 segundos. " + Environment.NewLine;
                InitializeTimer();
            }
            catch (Exception ex)
            {
                txtProcess.Text += "Ocurrio un error al querer correr el Script de cliente.py. "+ex.Message + Environment.NewLine;
            }
        }

        Timer timer;


        /// <summary>
        /// Metodo que hace el conteo para cerrar el instalador luego de correr el cliente
        /// </summary>
        private void InitializeTimer()
        {
            if (timer == null) timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            Application.Exit();
        }

        private void KillAllPython()
        {
            var allProcesses = Process.GetProcesses().Where(p=>p.ProcessName.ToLower().Contains("python"));
            foreach (var item in allProcesses)
            {
                item.Kill();
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar el boton de isntalar cliente. Descomprime el cliente, y limpia versiones anteriores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstallCli_Click(object sender, EventArgs e)
        {
            
            folderBrowserDialog1.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
            {
                if (File.Exists(folderBrowserDialog1.SelectedPath + "\\blackjack.zip")) File.Delete(folderBrowserDialog1.SelectedPath + "\\blackjack.zip");
                File.WriteAllBytes(folderBrowserDialog1.SelectedPath + "\\blackjack.zip", Resources.blackjack);
                txtProcess.Text += "Descargando Archivos de juego..." + Environment.NewLine;
                DowloadLatestVersion(folderBrowserDialog1.SelectedPath);
                try
                {
                    KillAllPython();
                    var sourceFile = folderBrowserDialog1.SelectedPath + "\\blackjack.zip";
                    if (Directory.Exists(folderBrowserDialog1.SelectedPath + "\\blackjack"))
                    {
                        Directory.Delete(folderBrowserDialog1.SelectedPath + "\\blackjack",true);
                    }
                    ZipFile.ExtractToDirectory(sourceFile, folderBrowserDialog1.SelectedPath);                    
                    txtProcess.Text += "Descomprimiento archivos..." + Environment.NewLine;
                    File.Delete(sourceFile);
                    txtProcess.Text += "El juego ha sido instalado correctamente." + Environment.NewLine;
                    StartClient();
                }
                catch (Exception ex)
                {
                    txtProcess.Text += "Ocurrio un error al intentar instalar el cliente. "+ex.Message + Environment.NewLine;
                }

            }
        }

        private void DowloadLatestVersion(string FilePath)
        {
            string inputfilepath = FilePath+@"\blackjack.zip";
            string ftphost = "181.239.145.84";
            string ftpfilepath = "/blackjack.zip";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("ftpuser", "test123");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
                txtProcess.Text += "Juego Descargado exitosamente" + Environment.NewLine;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace PEP
{
    internal static class Utilidades
    {
        public static List<System.Web.UI.WebControls.HyperLink> aplicaciones { get; set; }
        //public static string path = @"C:\\Users\\Usuario\\OneDrive\\Documents\\Pep arreglo\\documentos//";
        public static string path = @"\\gaia\AppFiles\pep\pruebas\";
        public static string logs_path = path + "logs";


        public static void MensajeBox(string mensaje, string tipo, ClientScriptManager ClientScript, System.Web.UI.Page pag)
        { //Tipo de mensaje puede ser alert,confirm o popup
            string message = mensaje;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append(tipo);
            sb.Append("('");
            sb.Append(message);
            sb.Append("');");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(pag.GetType(), "alert", sb.ToString());
        }

        public static void ErrorBitacora(string e, string m)
        {
            Directory.SetCurrentDirectory(logs_path);
            FileStream archivo = new FileStream("Error.log", FileMode.Open, FileAccess.Write);
            archivo.Seek(0, SeekOrigin.End);
            StreamWriter sw = new StreamWriter(archivo);
            sw.WriteLine("");
            sw.WriteLine("**************************");
            sw.WriteLine(System.DateTime.Now.ToString());
            sw.WriteLine("**************************");
            sw.WriteLine("(######)   " + m + "   (######)");
            sw.WriteLine(e);
            sw.Close();
            archivo.Close();
        }

        public static void SetLogDirectory()
        {
            //if (Directory.Exists(logs_path))
            //{
            //    Directory.SetCurrentDirectory(logs_path);
            //}
            //else
            //{
            //    Directory.CreateDirectory(logs_path);
            //    Directory.SetCurrentDirectory(logs_path);
            //}
            //FileStream archivo = new FileStream("Error.log", FileMode.OpenOrCreate);
            //archivo.Close();

        }

        public static bool IsPDF(string txt)
        {
            return txt.Substring(txt.Length - 4, 4).Equals(".pdf") || txt.Substring(txt.Length - 4, 4).Equals(".PDF");

        }

        public static int SaveFile(System.Web.UI.WebControls.FileUpload FileUpload1, int year, string TextConsecutivo)
        {
            // Obtener el nombre del archivo que desea cargar.
            string archivo = FileUpload1.FileName;
            // Path del directorio donde vamos a guardar el archivo
            string pathToCheck = path + year;

            //Verificamos si existe el directorio, sino existe se crea
            if (!Directory.Exists(pathToCheck))
            {
                Directory.CreateDirectory(pathToCheck);
            }

            // Crear la ruta y el nombre del archivo para comprobar si hay duplicados.
            pathToCheck = pathToCheck + "\\" + TextConsecutivo + ".PDF";
            // Compruebe si ya existe un archivo con el
            // mismo nombre que el archivo que desea cargar .       
            if ((System.IO.File.Exists(pathToCheck)))
            {
                return 1; //El archivo existe
            }
            else
            {
                // Llame al método SaveAs para guardar el archivo
                // guardado en el directorio especificado.
                FileUpload1.SaveAs(pathToCheck);
                return 0;
            }
        }

        public static int SaveFile(HttpPostedFile file, int year, String nombreArchivo, String carpeta)
        {
            // Path del directorio donde vamos a guardar el archivo
            String pathToCheck = path + year;

            //Verificamos si existe el directorio, sino existe se crea
            if (!Directory.Exists(pathToCheck))
            {
                Directory.CreateDirectory(pathToCheck);
            }

            // Path de la carpeta dentro del directorio en donde se va a guardar el archivo
            pathToCheck = pathToCheck + "\\" + carpeta;

            // Verificamos si ya existe la carpeta dentro del directorio, si no existe entonces se crea
            if (!Directory.Exists(pathToCheck))
            {
                Directory.CreateDirectory(pathToCheck);
            }

            // Crear la ruta y el nombre del archivo para comprobar si hay duplicados.
            pathToCheck = pathToCheck + "\\" + nombreArchivo;

            // Compruebe si ya existe un archivo con el
            // mismo nombre que el archivo que desea cargar .       
            if ((System.IO.File.Exists(pathToCheck)))
            {
                return 1; //El archivo existe
            }
            else
            {
                // Llame al método SaveAs para guardar el archivo
                // guardado en el directorio especificado.
                file.SaveAs(pathToCheck);
                return 0;
            }
        }

        // Retorna 1 si logro sobreescribir, 0 si no habia archivo, por lo que no sobreecribio, solo creo un nuevo archivo.
        public static int OverWriteFile(System.Web.UI.WebControls.FileUpload FileUpload1, int year, string TextConsecutivo, string nuevoTextConsecutivo)
        {
            // Path del directorio donde vamos a guardar el archivo
            string pathToCheck = path + " " + year;

            //Verificamos si existe el directorio, sino existe se crea
            if (!Directory.Exists(pathToCheck))
            {
                Directory.CreateDirectory(pathToCheck);
            }

            // Crear la ruta y el nombre del archivo para comprobar si hay duplicados.
            string olFile = pathToCheck + "\\" + TextConsecutivo + ".PDF";
            string newFile = pathToCheck + "\\" + nuevoTextConsecutivo + ".PDF";

            // Compruebe si ya existe un archivo con el
            // mismo nombre que el archivo que desea cargar .       
            if ((System.IO.File.Exists(olFile)))
            {
                System.IO.File.Move(olFile, newFile);
                return 1; //El archivo existe
            }
            else
            {
                // Llame al método SaveAs para guardar el archivo.
                FileUpload1.SaveAs(newFile);
                return 0;
            }
        }

        public static bool existeArchivo(string nombre_archivo, int year)
        {
            bool existe = false;

            string pathToCheck = path + year;

            if (Directory.Exists(pathToCheck))
            {
                pathToCheck = pathToCheck + "\\" + nombre_archivo + ".PDF";

                if ((System.IO.File.Exists(pathToCheck)))
                {
                    existe = true;
                }
            }

            return existe;
        }

        public static List<string> buscarEnLista(string prefixText, List<string> li)
        {
            List<string> result = new List<string>();
            foreach (string s in li)
            {
                if (s.Contains(prefixText) || s.Contains(prefixText.ToUpper()) || s.Contains(prefixText.ToLower()))
                {
                    result.Add(s);
                }
            }

            return result;
        }



        public static void escogerMenu(Page page, int[] rolesPermitidos)
        {
            int rol = page.Session["rol"] == null ? 0 : Int32.Parse(page.Session["rol"].ToString());
            if (page.Session["nombreCompleto"] == null)
            {
                page.Session.RemoveAll();
                page.Session.Abandon();
                page.Session.Clear();
                String url = page.ResolveUrl("~/login.aspx");
                page.Response.Redirect(url);
            }
            /*
             * Rol..................idRol
             * Administrador.........2
             * Usuario......................13
             */
            else if (rol == 2)
            {
                if (rolesPermitidos.Contains(rol))
                {
                    page.Master.FindControl("MenuAdministrador").Visible = true;
                    page.Master.FindControl("MenuUsuario").Visible = false;
                }
                else
                {
                    page.Session.RemoveAll();
                    page.Session.Abandon();
                    page.Session.Clear();
                    String url = page.ResolveUrl("~/login.aspx");
                    page.Response.Redirect(url);
                }
            }else if (rol == 13)
            {
                if (rolesPermitidos.Contains(rol))
                {
                    page.Master.FindControl("MenuAdministrador").Visible = false;
                    page.Master.FindControl("MenuUsuario").Visible = true;
                }
                else
                {
                    page.Session.RemoveAll();
                    page.Session.Abandon();
                    page.Session.Clear();
                    String url = page.ResolveUrl("~/login.aspx");
                    page.Response.Redirect(url);
                }
            }
            else
            {
                page.Session.RemoveAll();
                page.Session.Abandon();
                page.Session.Clear();
                String url = page.ResolveUrl("~/login.aspx");
                page.Response.Redirect(url);
            }
        }

        /*
         * Jonathan Fonseca Vallejos
         * 30/may/2016
         * Efecto: envía un correo con la información indicada en el parámetro 
         * Requiere: la información necesaria para enviar el correo
         * Modifica: -
         * Devuelve: -
         */
        public static Boolean enviarCorreo(Dictionary<String, String> informacionCorreo)
        //public static Boolean enviarCorreo(Dictionary<String, String> informacionCorreo)
        {
            // Código obtenido en http://oscarsotorrio.com/post/2011/01/22/Envio-de-correo-en-NET-con-CSharp.aspx

            /*-------------------------MENSAJE DE CORREO ----------------------*/

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();

            // Obtenemos las direcciones de correo de los destinatarios desde el diccionario informaciónCorreo
            String destinatarios = informacionCorreo["destinatarios"];
            String[] listaDestinatarios = destinatarios.Split(';');

            foreach (String destinatario in listaDestinatarios)
            {
                try
                {
                    //Direccion de correo electronico a la que queremos enviar el mensaje
                    if (destinatario.Trim() != "")
                        mmsg.To.Add(destinatario); //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario
                }
                catch { }
            }

            // Asunto
            mmsg.Subject = informacionCorreo["asunto"];
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            String conCopia = informacionCorreo["conCopia"];
            String[] listaConCopia = conCopia.Split(';');
            if (conCopia.Trim() != "")
            {
                //Direccion de correo electronico que queremos que reciba una copia del mensaje
                //envia una copia del correo
                //mmsg.Bcc.Add(conCopia);

                //adjunta como copia a "X"
                foreach (String conCopiaA in listaConCopia)
                {
                    try
                    {
                        if (conCopiaA.Trim() != "")
                        {
                            mmsg.CC.Add(conCopiaA);
                        }
                    }
                    catch { }
                }
            }

            String conCopiaOculta = informacionCorreo["conCopiaOculta"];
            String[] listaConCopiaOculta = conCopiaOculta.Split(';');
            if (conCopiaOculta.Trim() != "")
            {
                foreach (String conCopiaOcultaA in listaConCopiaOculta)
                {
                    try
                    {
                        //Direccion de correo electronico que queremos que reciba una copia del mensaje oculto
                        //envia una copia del correo
                        if (conCopiaOcultaA.Trim() != "")
                        {
                            mmsg.Bcc.Add(conCopiaOcultaA);
                        }
                    }
                    catch { }
                }
            }

            //Cuerpo del Mensaje
            mmsg.Body = informacionCorreo["cuerpo"];
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true;

            //Correo electronico desde la que enviamos el mensaje
            String remitente = informacionCorreo["remitente"].Split(';')[0];
            mmsg.From = new System.Net.Mail.MailAddress(remitente);

            // Prioridad del mensaje
            mmsg.Priority = MailPriority.High;

            // Si queremos enviar un archivo adjunto
            // mmsg.Attachments.Add(new Attachment(@"G:\LANAMME\Viaticos locales\24022014_Reporte viaticos.pdf"));

            String rutasArchivos = informacionCorreo["archivos"];
            String[] listaRutasArchivos = rutasArchivos.Split(';');

            foreach (String rutaArchivo in listaRutasArchivos)
            {
                //Direccion de correo electronico a la que queremos enviar el mensaje
                if (rutaArchivo.Trim() != "")
                    mmsg.Attachments.Add(new Attachment(rutaArchivo));
            }

            /*---------------------- FIN MENSAJE DE CORREO --------------------*/

            /*-------------------------CLIENTE DE CORREO ----------------------*/

            //Creamos un objeto de cliente de correo
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();

            //Hay que crear las credenciales del correo emisor
            cliente.Credentials = new System.Net.NetworkCredential("correo@gmail.com", "contraseña");
            //cliente.Credentials = new System.Net.NetworkCredential("correo@ucr.ac.cr", "contraseña");

            //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
            cliente.Port = 587;
            cliente.EnableSsl = true;

            cliente.Host = "smtp.gmail.com"; //Para Gmail "smtp.gmail.com";
            //cliente.Host = "smtp.ucr.ac.cr"; //Para UCR "smtp.ucr.ac.cr";

            /*----------------------FIN CLIENTE DE CORREO -------------------*/

            /*-------------------------ENVIO DE CORREO ----------------------*/

            Boolean enviado = true;

            try
            {
                //Enviamos el mensaje      
                cliente.Send(mmsg);
                mmsg.Dispose();
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
                enviado = false;
            }

            /*---------------------- FIN ENVIO DE CORREO --------------------*/

            return enviado;
        }

    }
}
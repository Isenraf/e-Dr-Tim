using System;
using System.IO;
using System.Threading.Tasks;
using BANA.Models;
using iTextSharp.text.pdf;
using RestSharp;




namespace BANA.MyFunction
{
    public class Whatsap4
    {
        public Whatsap4(string patient,string resultat,string sexe,string tel,string pdf)
        { 
            Main(patient,resultat,sexe,tel,pdf);
        }

        public static async Task Main(string patient,string resultat,string sexe,string tel,string pdf)
        {
            string Genre="Monsieur";
            if (sexe=="F")
            {
                Genre="Madamme";
            }

            tel =tel.Replace(" ","").Replace("-","");
            if (tel.Length>9)
            {
                if (tel.Substring(0,3)=="237")
                {
                    tel=tel.Remove(0,3);
                }else if(tel.Substring(0,4)=="+237"){
                    tel=tel.Remove(0,4);
                }
                
            }
            if (tel.Length!=9)
            {
                tel="677459997";
            }

            
           string message=Genre+" "+patient+" \r\n Votre résultat: "+resultat+". est disponible,"+ "\r\n  La Polyclinique de poitiers vous souhaite une bonne guérison.";

            var url = "https://api.ultramsg.com/instance19922/messages/document";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);    
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", "qkzdhg34pgqzzexb");
                request.AddParameter("to", "+237"+tel);
                //request.AddParameter("body", message);
                
                request.AddParameter("filename", resultat+".pdf");
                request.AddParameter("document", pdf);
                request.AddParameter("caption", message);

                
            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            Console.WriteLine(output);

        }

       
        

    }
}

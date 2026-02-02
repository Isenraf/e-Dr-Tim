using System;
using System.IO;
using System.Threading.Tasks;
using BANA.Models;
using iTextSharp.text.pdf;
using RestSharp;




namespace BANA.MyFunction
{
    public class Whatsap3
    {
        public Whatsap3(DateTime datedebut,DateTime datefin,string medecin,string tel,string pdf)
        { 
            Main(datedebut,datefin,medecin,tel,pdf);
        }

        public static async Task Main(DateTime datedebut,DateTime datefin,string medecin,string tel,string pdf)
        {
            tel=tel.Replace(" ","").Replace("-","");
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
                tel="694217600";
            }

            
            string message=medecin+" \r\n Veuillez trouvez ci-dessous vos états pour la période du: "+datedebut+"  au: "+datefin+". \r\n NB: ceci est un message automatique généré par NOLAN HOSPITAL";

            var url = "https://api.ultramsg.com/instance19922/messages/document";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);    
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", "qkzdhg34pgqzzexb");
                request.AddParameter("to", "+237"+tel);
                //request.AddParameter("body", message);
                
                request.AddParameter("filename", medecin.Replace(" ","")+".pdf");
                request.AddParameter("document", pdf);
                request.AddParameter("caption", message);

                
            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            Console.WriteLine(output);

        }

       
        

    }
}

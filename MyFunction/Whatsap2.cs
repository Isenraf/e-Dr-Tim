using System;
using System.IO;
using System.Threading.Tasks;
using BANA.Models;
using RestSharp;




namespace BANA.MyFunction
{
    public class Whatsap2
    {
        public Whatsap2(Resultat resultat)
        { 
            Main2(resultat);
            Main0(resultat);
        }

        public static async Task Main2(Resultat resultat)
        {

            string Genre="Monsieur";
            if (resultat.Genre=="F")
            {
                Genre="Madamme";
            }
            string telephone=resultat.Telephone_patient;
            if (telephone.Length>9)
            {
                if (telephone.Substring(0,3)=="237")
                {
                    telephone=telephone.Remove(0,3);
                }else if(telephone.Substring(0,4)=="+237"){
                    telephone=telephone.Remove(0,4);
                }
                
            }
            string message=Genre+" "+resultat.NomPatient+" \r\n Votre résultat: "+resultat.Nom+". est disponible, vous pouvez le consulter en ligne cliquer sur le lien suivant: https://www.polycliniquedepoitiers.com/Resultat"+ "\r\n  La Polyclinique de poitiers vous souhaite une bonne guérison.";

            var url = "https://api.ultramsg.com/instance19922/messages/chat";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);    
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", "qkzdhg34pgqzzexb");
                request.AddParameter("to", "+237"+telephone);
                request.AddParameter("body", message);


            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            Console.WriteLine(output);

        }


        public static async Task Main0(Resultat monresultat)
        {

            string Genre="Monsieur";
            if (monresultat.Genre=="F")
            {
                Genre="Madamme";
            }
            string telephone=monresultat.Telephone_patient;
            if (monresultat.Telephone_patient.Length>9)
            {
                if (monresultat.Telephone_patient.Substring(0,3)=="237")
                {
                    monresultat.Telephone_patient=monresultat.Telephone_patient.Remove(0,3);
                }else if(monresultat.Telephone_patient.Substring(0,4)=="+237"){
                    monresultat.Telephone_patient=monresultat.Telephone_patient.Remove(0,4);
                }
                
            }

            string message="🌟 Découvrez e-Dr Tim – Votre application de consultation basée sur l’intelligence artificielle"+

                            "\nBonjour "+Genre+" "+monresultat.Nom+","+
                            "\nNous sommes ravis de vous présenter e-Dr Tim, votre nouvel assistant santé ! 📲"+

                            "\n✅ évaluer vos symptômes en ligne en un clic"+
                            "\n✅ téléchargez le rapport de consultation facilement"+

                            "\nTéléchargez dès maintenant et simplifiez votre suivi médical ! 🔽"+

                            "\n👉 https://edoctor-tim.com"+

                            "\nBesoin d’aide ? Répondez à ce message, nous sommes là pour vous. 😊";



            var url = "https://api.ultramsg.com/instance19922/messages/chat";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);    
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", "qkzdhg34pgqzzexb");
                request.AddParameter("to", "+237"+telephone);
                request.AddParameter("body", message);


            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;
            Console.WriteLine(output);

        }

        

    }
}

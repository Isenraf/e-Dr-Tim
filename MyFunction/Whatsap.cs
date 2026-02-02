using System;
using System.IO;
using System.Threading.Tasks;
using BANA.Models;
using RestSharp;




namespace BANA.MyFunction
{
    public class Whatsap
    {
        public Whatsap(Facture mafact)
        { 
            Main(mafact);
            Main0(mafact);
        }

        public static async Task Main(Facture facture)
        {

            string Genre="Monsieur";
            if (facture.Genre=="F")
            {
                Genre="Madamme";
            }
            string telephone=facture.Phone;
            if (facture.Phone.Length>9)
            {
                if (facture.Phone.Substring(0,3)=="237")
                {
                    facture.Phone=facture.Phone.Remove(0,3);
                }else if(facture.Phone.Substring(0,4)=="+237"){
                    facture.Phone=facture.Phone.Remove(0,4);
                }
                
            }
            string message=Genre+" "+facture.Nom+" "+facture.Prenom+" \r\n Numéro de Facture: "+facture.Numero_de_facture+". \r\n Numéro de dossier: "+facture.Numero_dossier+". \r\n Montant Payé: "+facture.Montant_recu_patient+" FCFA \r\n Reste à Payer: "+(facture.Net_a_payer_patient-facture.Montant_recu_patient)+"\r\n La Polyclinique de poitiers vous souhaite une bonne guérison. \r\n Vitsitez notre site web: www.polycliniquedepoitiers.com";

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

        public static async Task Main0(Facture facture)
        {

            string Genre="Monsieur";
            if (facture.Genre=="F")
            {
                Genre="Madamme";
            }
            string telephone=facture.Phone;
            if (facture.Phone.Length>9)
            {
                if (facture.Phone.Substring(0,3)=="237")
                {
                    facture.Phone=facture.Phone.Remove(0,3);
                }else if(facture.Phone.Substring(0,4)=="+237"){
                    facture.Phone=facture.Phone.Remove(0,4);
                }
                
            }

            string message="🌟 Découvrez e-Dr Tim – Votre application de consultation basée sur l’intelligence artificielle"+

                            "\nBonjour "+Genre+" "+facture.Nom+" "+facture.Prenom+","+
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
            string message=Genre+" "+resultat.NomPatient+" \r\n Votre résultat: "+resultat.Nom+". est disponible, vous pouvez le consulter en ligne cliquer sur le lien suivant: https://www.polycliniquedepoitiers.com/Resultat?fact="+resultat.NumeroFacture+"&tel="+resultat.Telephone_patient+ " \r\n La Polyclinique de poitiers vous souhaite une bonne guérison.";

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

using System;
using System.IO;
using System.Threading.Tasks;
using BANA.Models;
using RestSharp;




namespace BANA.MyFunction
{
    public class Sms
    {
        public Sms(Facture mafact)
        { 
            Main(mafact);
        }

        public static async Task Main(Facture facture)
        {   
            string Genre="Monsieur";
            if (facture.Genre=="F")
            {
                Genre="Madamme";
            }
            string Soa="POITIERS";
            string telephone=facture.Phone;
            if (facture.Phone.Length>9)
            {
                if (facture.Phone.Substring(0,3)=="237")
                {
                    facture.Phone=facture.Phone.Remove(0,3);
                }
                
            }
            string message=Genre+" "+facture.Nom+" "+facture.Prenom+" \r\n Numéro de Facture: "+facture.Numero_de_facture+". \r\n Numéro de dossier: "+facture.Numero_dossier+". \r\n Montant Payé: "+facture.Montant_recu_patient+" FCFA \r\n Reste à Payer: "+(facture.Net_a_payer_patient-facture.Montant_recu_patient)+"\r\n La Polyclinique de poitiers vous souhaite une bonne guérison. \r\n Vitsitez notre site web: www.polycliniquedepoitiers.com";
            var url = "https://lmtgroup.dyndns.org/sendsms/sendsmsGold.php?UserName=inosoft&Password=uR9UCJFJ&SOA="+Soa+"&MN=237"+telephone+"&SM="+message;
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Post);    
            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;

        }

    }
}



